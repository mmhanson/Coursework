using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkController;
using System.Text.RegularExpressions;

namespace ChatServer
{

  /// <summary>
  /// A simple server for receiving simple text messages from multiple clients
  /// </summary>
  class ChatServer
  {
    static void Main(string[] args)
    {
      ChatServer server = new ChatServer();
      server.StartServer();

      // Sleep to prevent the program from closing,
      // since all the real work is done in separate threads
      // StartServer is non-blocking
      Console.Read();
    }

    // A list of clients that are connected.
    private List<SocketState> clients;
    
    private TcpListener listener;

    public ChatServer()
    {
      listener = new TcpListener(IPAddress.Any, Networking.DEFAULT_PORT);

      clients = new List<SocketState>();
    }

    /// <summary>
    /// Start accepting Tcp sockets connections from clients
    /// </summary>
    public void StartServer()
    {
      Console.WriteLine("Server waiting for client");

      listener.Start();

      // This begins an "event loop".
      // ConnectionRequested will be invoked when the first connection arrives.
      listener.BeginAcceptSocket(ConnectionRequested, null);
    }

    /// <summary>
    /// A callback for invoking when a socket connection is accepted
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectionRequested(IAsyncResult ar)
    {
      Console.WriteLine("Contact from client");
      
      // Get the socket
      Socket s = listener.EndAcceptSocket(ar);

      // Save the socket in a SocketState, 
      // so we can pass it to the receive callback, so we know which client we are dealing with.
      SocketState newClient = new SocketState(s, clients.Count, null, null);

      // Can't have the server modifying the clients list if it's braodcasting a message.
      lock (clients)
      {
        clients.Add(newClient);
      }

      // Start listening for a message
      // When a message arrives, handle it on a new thread with ReceiveCallback
      //                                  the buffer          buffer offset        max bytes to receive                         method to call when data arrives    "state" object representing the socket
      newClient.sock.BeginReceive(newClient.messageBuffer,     0,         newClient.messageBuffer.Length, SocketFlags.None,        ReceiveCallback,                      newClient);

      // Continue the "event loop" that was started on line 53
      // Start listening for the next client, on a new thread
      listener.BeginAcceptSocket(ConnectionRequested, null);

    }

    /// <summary>
    /// Callback method for when data is received (started from line 80)
    /// </summary>
    /// <param name="ar">The result that includes the "state" parameter from BeginReceive</param>
    private void ReceiveCallback(IAsyncResult ar)
    {

      // Get the socket state out of the AsyncState
      // This is the object that we passed to BeginReceive that represents the socket
      SocketState sender = (SocketState)ar.AsyncState;

      int bytesRead = sender.sock.EndReceive(ar);

      // If the socket is still open
      if (bytesRead > 0)
      {
        string theMessage = Encoding.UTF8.GetString(sender.messageBuffer, 0, bytesRead);
        // Append the received data to the growable buffer.
        // It may be an incomplete message, so we need to start building it up piece by piece
        sender.sb.Append(theMessage);

        // TODO: If we had an "EventProcessor" delagate associated with the socket state,
        //       We could call that here, instead of hard-coding this method to call.
        ProcessMessage(sender);
      }

      // Continue the "event loop" that was started on line 80.
      // Start listening for more parts of a message, or more new messages
      sender.sock.BeginReceive(sender.messageBuffer, 0, sender.messageBuffer.Length, SocketFlags.None, ReceiveCallback, sender);

    }

    /// <summary>
    /// Given the data that has arrived so far, 
    /// potentially from multiple receive operations, 
    /// determine if we have enough to make a complete message,
    /// and process it (print it).
    /// </summary>
    /// <param name="sender">The SocketState that represents the client</param>
    private void ProcessMessage(SocketState sender)
    {
      string totalData = sender.sb.ToString();

      string[] parts = Regex.Split(totalData, @"(?<=[\n])");

      // Loop until we have processed all messages.
      // We may have received more than one.
      foreach(string p in parts)
      {

        // Ignore empty strings added by the regex splitter
        if (p.Length == 0)
          continue;
        // The regex splitter will include the last string even if it doesn't end with a '\n',
        // So we need to ignore it if this happens. 
        if (p[p.Length-1] != '\n')
          break;

        Console.WriteLine("received message: \"" + p + "\"");

        byte[] messageBytes = Encoding.UTF8.GetBytes(p);
        
        // Remove it from the SocketState's growable buffer
        sender.sb.Remove(0, p.Length);

        // Broadcast the message
        // Can't have new connections popping up while looping through the clients list.
        lock (clients)
        {
          foreach (SocketState client in clients)
            client.sock.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, client);

        }

      }

    }

    private void SendCallback(IAsyncResult ar)
    {
      SocketState ss = (SocketState)ar.AsyncState;
      // Nothing much to do here, just conclude the send operation so the socket is happy.
      ss.sock.EndSend(ar);
    }
  }
}
