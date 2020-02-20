using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
    public delegate void NetworkAction(SocketState ss);

    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        // the socket this socket state is using
        public Socket sock;
        // an identification number associated with this socket state
        public int id_num;
        // delegate used to inform the client when a connection has been established
        public NetworkAction socketConnected;
        // delegate used to inform the client when data has been received
        public NetworkAction dataReceived;

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        public SocketState(Socket sock, int id_num, NetworkAction socketConnected, NetworkAction dataReceived)
        {
            this.sock = sock;
            this.id_num = id_num;
            this.socketConnected = socketConnected;
            this.dataReceived = dataReceived;
        }
    }

    /// <summary>
    /// This class encapsulates and abstracts networking operations required
    /// to send and receive strings from a remote server over TCP/IP.
    /// 
    /// <remarks>
    /// Throughout documentation of this class, the term 'client' is used. This
    /// term, in this scope, refers to the program using this library and not to
    /// the client in a client-server networking architecture.
    /// </remarks>
    /// </summary>
    public class Networking
    {
        public const int DEFAULT_PORT = 11000;

        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }

        /// <summary>
        /// Connect to a host
        /// 
        /// <remarks>
        /// The reason I decided to include parameters for both of the callback included here is this:
        /// the client does not need to tell the network controller that it wants to receive data, the
        /// network controller only needs to tell the client whenever data has arrived. To require the
        /// client to do this extra work partially defeats the separtion of concerns that this library
        /// was meant to deal with, in my opinion. Therefore, a method to inform the network controller
        /// to start getting data is unnecessary, the client must only tell the controller to connect
        /// to the server, and provide callbacks to deal with the significant events involved in that,
        /// for which each event has a unique delegate passed to this method.
        /// </remarks>
        /// </summary>
        /// <param name="hostname">The host to connect to (no port)</param>
        /// <param name="connectionCallback">
        /// The delegate to inform the client that the connection has been estalised.
        /// </param>
        /// <param name="dataReceivedCallback">
        /// THe delegate to inform the client that data has been received on a connection.
        /// </param>
        /// <returns>the socket state associated with the new connection</returns>
        public static SocketState ConnectToServer(String hostname, NetworkAction connectionCallback, NetworkAction dataReceivedCallback)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostname);

            // create the 'nomadic' socket state
            Socket socket;
            IPAddress ipAddress;
            Networking.MakeSocket(hostname, out socket, out ipAddress);
            SocketState sockState = new SocketState(socket, -1, connectionCallback, dataReceivedCallback);  // id not relevant for now, 

            // begin the connection process and call 'ConnectedToServer' when the connection is completed
            socket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, sockState);

            return sockState;
        }

        /// <summary>
        /// Send data on a connection.
        /// </summary>
        /// <param name="message">
        /// Message to send. Note that this message is assumed to not be terminated with a newline character,
        /// since this is protocol, it will be automatically added. This is just a raw string.
        /// </param>
        /// <param name="connection">The connection to send the data on</param>
        public static void Send(String message, SocketState connection)
        {
            // Append a newline, since that is our protocol's terminating character for a message.
            byte[] messageBytes = Encoding.UTF8.GetBytes(message + "\n");
            // start sending the message and call 'SendCallback' when done with the connection
            connection.sock.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, connection);
        }

        private static void ConnectedToServer(IAsyncResult ar)
        {
            // get socket state from the async result (passed to beginconnect)
            SocketState ss = (SocketState)ar.AsyncState;

            // complete the execution and handle the case where it cannot be completed
            try
            {
                ss.sock.EndConnect(ar);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            // inform client that the connection was successfully estalished
            ss.socketConnected(ss);

            // automatically start receiving data from the server
            ss.sock.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);
        }

        /// <summary>
        /// Callback for when a socket receives data.
        /// </summary>
        /// <param name="ar">AsyncResult for the data, in this case it contains a socket state</param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState sock_state = (SocketState)ar.AsyncState;

            // end the receiving process and record the amount of bytes read from the socket
            int bytesRead = sock_state.sock.EndReceive(ar);

            // If there is data to handle
            if (bytesRead > 0)
            {
                // convert the bytes received to string characters
                string theMessage = Encoding.UTF8.GetString(sock_state.messageBuffer, 0, bytesRead);
                // Append the received data to the growable buffer. This is useful in the case that incomplete
                // data was receieved
                sock_state.sb.Append(theMessage);

                // notify the client that data has arrived
                sock_state.dataReceived(sock_state);
            }

            // start listening for more data coming in
            sock_state.sock.BeginReceive(sock_state.messageBuffer, 0, sock_state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, sock_state);
        }

        /// <summary>
        /// This callback is invoked when the message is sent.
        /// </summary>
        /// <param name="ar">result for the send operation</param>
        private static void SendCallback(IAsyncResult ar)
        {
            SocketState connection = (SocketState)ar.AsyncState;
            connection.sock.EndSend(ar);        // just need to conclude the operation
        }
    }
}
