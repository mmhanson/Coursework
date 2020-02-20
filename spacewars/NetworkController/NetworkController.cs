// This is a library for sending and receiving strings to/from a remote server over TCP/IP.
// As much of the functionality and tedium of networking has been put into this library as
// possible, making it relatively simple to implement networking in other projects.
//
// Written by Maxwell Hanson (u0985911) for CS 3500 at the University of Utah on November 8th, 2018

// Update 1.1 notes (Nov 13, 2018)
// ==============================
// These classes were updated and the API was changed significantly. Clients must now manually 
// request more data after it processes it. Also, the socket is now locked when it is written
// to. This was done to avoid potential race conditions with the client. See class doc for more
// information and detail.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
    /// <summary>
    /// This delegate type is used to define delegates for informing the client
    /// of an event that occured on their connection that they must handle.
    /// </summary>
    /// <param name="ss"></param>
    public delegate void NetworkAction(SocketState ss);

    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        /// <summary>
        /// The socket that this socket state uses
        /// </summary>
        public Socket socket;

        /// <summary>
        /// An identification number assigned to this socket state.
        /// </summary>
        public int id_num;

        /// <summary>
        /// Delegate used to inform the client when a networking event has occured.
        /// 
        /// <remarks>
        /// The client will assign this delegate to specific methods at specific times according
        /// to the protocol agreed upon by the client and the server. For example, the client
        /// might initially assign this delegate to a method for handling the connection being
        /// established, then it might assign it to handle a startup sequence, then it will assign
        /// it to a method for handling a communication loop.
        /// The reason there is only one delegate for all these handlers is, one, so that this
        /// network controller will be more general. And two, because for most uses, the client
        /// will only be communicating one thing at a time to the server. So the client can just
        /// use one delegate and assign it to handle whatever is being communicated at that time.
        /// </remarks>
        /// </summary>
        public NetworkAction informClient;

        /// <summary>
        /// This buffer will keep data taken directly from the socket.
        /// </summary>
        public byte[] messageBuffer = new byte[4096];

        /// <summary>
        /// This is a larger, overflow buffer for use in the case that a single data packet
        /// does not contain the whole message. Data from the array buffer will be placed in here
        /// and the client must decide if the message is complete or not depending on its protocol.
        /// </summary>
        public StringBuilder overflowBuffer = new StringBuilder();

        /// <summary>
        /// Determine if this socket state is connected.
        /// </summary>
        public Boolean IsConnected
        {
            get
            {
                return this.socket.Connected;
            }
            internal set { }
        }

        /// <summary>
        /// Create a socket state.
        /// </summary>
        /// <param name="socket">The socket to associate with this socket state</param>
        /// <param name="id_num">The ID of this socket state</param>
        /// <param name="informClient">
        /// A delegate to allow the network controller operating the socket state to communicate
        /// with the client using the network controler. See the field doc for more info.
        /// </param>
        public SocketState(Socket socket, int id_num, NetworkAction informClient)
        {
            this.socket = socket;
            this.id_num = id_num;
            this.informClient = informClient;
        }
    }

    /// <summary>
    /// This class holds the information to represent an incoming connection.
    /// 
    /// This class is used like a socket state but for listening for incoming
    /// connections.
    /// </summary>
    public class ConnectionState
    {
        /// <summary>
        /// Listener the incoming connection is on.
        /// </summary>
        public TcpListener listener;

        /// <summary>
        /// Delegate to invoke the client when
        /// TODO
        /// </summary>
        public NetworkAction informClient;
    }

    /// <summary>
    /// This class encapsulates and abstracts networking operations required to send and receive strings
    /// to/from a remote server over TCP/IP.
    /// 
    /// <para>
    /// Please read to following to ensure proper usage. To connect to a server, call the ConnectToServer
    /// method and assign the delegate in the socket state to handle the connection failing or being
    /// established. In this delegate, handle this event accordingly and start receiving data from the
    /// server via the GetData method and assign the delegate to handle receiving data. At this point
    /// you can assign the delegate to an intermittent handler that will handle a specific startup
    /// sequence between the client and the server, or you can start handling the communication loop
    /// directly. See the references for more info on how to use the delegate, GetData, ConnectToServer
    /// methods.
    /// </para>
    /// 
    /// <remarks>
    /// Throughout documentation of this class, the term 'client' is used. This
    /// term, in this scope, refers to the program using this library and not to
    /// the client in a client-server networking architecture.
    /// </remarks>
    /// <see cref="Networking.ConnectToServer(string, NetworkAction)"/>
    /// <see cref="Networking.GetData(SocketState)"/>
    /// <see cref="SocketState.informClient"/>
    /// </summary>
    public class Networking
    {
        /// <summary>
        /// The port used in all connections is hardwired for now, for simplicity.
        /// </summary>
        public const int DEFAULT_PORT = 11000;

        /// <summary>
        /// Creates a Socket object for the given host string.
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static Socket MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
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

                return socket;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }

        /// <summary>
        /// Connect to a host.
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
        /// 
        /// The reason I decided to return a SocketState representing the connection instead of returning
        /// void is for simplicity. The client needs at least a socket to send data with the controller
        /// since the controller can handle multiple connections. If the client did not supply this then
        /// the controller would not know which connection to send it on. So, this gives the decision to
        /// require the client to track a socket or a socket state for their connection(s). I decided to
        /// require them to track a socket state since a socket state includes all the functionality of a
        /// socket and more. Furthermore, the use of a socket state to represent a connection in all parts
        /// of this solution will introduce more regularity than if some parts of the solution used a socket
        /// and some parts used a socket state. This decision made, the question is how does the client
        /// get this socket state? Though they could make one themselves, I decided to program the network
        /// controller to do it in this method and return it to further decouple the client from the
        /// tedium of networking operations.
        /// </remarks>
        /// </summary>
        /// <param name="hostname">The host to connect to (no port)</param>
        /// <param name="informClient">
        /// Delegate to allow this network controller to communicate with the client (invoking this constructor).
        /// See the doc for the informClient delegate of the SocketState class for specific info on usage.
        /// <see cref="SocketState.informClient"/>
        /// </param>
        /// <returns>the socket state associated with the new connection</returns>
        public static SocketState ConnectToServer(String hostname, NetworkAction informClient)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostname);

            // create the 'nomadic' socket state
            Socket socket;
            IPAddress ipAddress;
            Networking.MakeSocket(hostname, out socket, out ipAddress);
            SocketState connection = new SocketState(socket, -1, informClient);  // id not relevant for now as client on has one connection

            // begin the connection process and call 'ConnectedToServer' when the connection is completed
            IAsyncResult result = socket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, connection);
            return connection;
        }

        /// <summary>
        /// Send data on a connection.
        /// </summary>
        /// <param name="message">Message to send. No protocol is used here, it just sends this exact string</param>
        /// <param name="connection">The connection to send the data on</param>
        public static void Send(String message, SocketState connection)
        {
            // turn string into bytes for sending
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            // start sending the message and call 'SendCallback' when done with the connection
            connection.socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, connection);
        }

        /// <summary>
        /// Start receiving data from the server.
        /// 
        /// <remarks>
        /// The decision to make this method came from the possible race conditions that may arise
        /// from the event in which the client is processing data from the socket state buffer
        /// while the network controller is receiving data from the server and writing to the buffer.
        /// If the client must manually start receiving more data, the intention is that the client
        /// will only recieve data in times where it is not modifying the buffer. Further, if the
        /// client must use this method, it will allow for the client to have a higher level of control
        /// over the protocol between it and the server since it will have the power to ask for data
        /// and process it in a programmed order.
        /// For these reasons, this method was introduced and it is therefore the client's responsibility
        /// to call this method whenever they want to receive more data.
        /// </remarks>
        /// </summary>
        /// <param name="connection">the connection to start receiving data on</param>
        public static void GetData(SocketState connection)
        {
            // start receiving more data on the connection given and call 'ReceiveCallback' when data comes in
            connection.socket.BeginReceive(connection.messageBuffer, 0,
                connection.messageBuffer.Length, SocketFlags.None, ReceiveCallback, connection);
        }

        /// <summary>
        /// Start listening for and accepting new clients on the default port.
        /// 
        /// Will call provided delegate with a socket state when a client connects.
        /// The socket state provided will need the id num to be changed. It will be 0.
        /// </summary>
        /// <param name="informClient">callback to invoke when a client connects</param>
        public static void StartAcceptingNewClients(NetworkAction informClient)
        {
            TcpListener clientListener = new TcpListener(DEFAULT_PORT);
            clientListener.Start();

            ConnectionState connState = new ConnectionState();
            connState.listener = clientListener;
            connState.informClient = informClient;

            clientListener.BeginAcceptSocket(AcceptNewClient, connState);
        }

        /// <summary>
        /// Callback used to accept a client trying to connect.
        /// 
        /// <see cref="StartAcceptingNewClients(NetworkAction)"/>
        /// </summary>
        /// <param name="ar">wrapper for connection state object</param>
        private static void AcceptNewClient(IAsyncResult ar)
        {
            ConnectionState connState = (ConnectionState)ar.AsyncState;
            TcpListener listener = connState.listener;
            Socket clientSocket = listener.EndAcceptSocket(ar);

            SocketState clientSs = new SocketState(clientSocket, 0, connState.informClient);
            clientSs.informClient(clientSs);

            listener.BeginAcceptSocket(AcceptNewClient, connState);     // continue listening event loop
        }

        private static void ConnectedToServer(IAsyncResult ar)
        {
            // get socket state from the async result (passed to beginconnect)
            SocketState connection = (SocketState)ar.AsyncState;

            // complete the execution and handle the case where it cannot be completed
            try
            {
                connection.socket.EndConnect(ar);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            // inform client that the connection was successfully estalished
            connection.informClient(connection);

            // note client must manually continue the networking loop via the GetData method. See method doc for more info.
        }

        /// <summary>
        /// Callback for when a socket receives data.
        /// </summary>
        /// <param name="ar">AsyncResult for the data, in this case it contains a socket state</param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState connection = (SocketState)ar.AsyncState;

            // end the receiving process and record the amount of bytes read from the socket
            int bytesRead = 0;
            try
            {
                bytesRead = connection.socket.EndReceive(ar);
            } catch (SocketException)
            {
                connection.informClient(connection);
            }

            // If there is data to handle
            if (bytesRead > 0)
            {
                // lock the connection while writing to buffers to eliminate race conditions with the client 
                lock (connection)
                {
                    // get data, put in buffer, notify client
                    string message = Encoding.UTF8.GetString(connection.messageBuffer, 0, bytesRead);
                    connection.overflowBuffer.Append(message);
                    connection.informClient(connection);
                }
            }
            // if the server didn't send any data, the connection was likely closed
            else
            {
                // TODO ensure it is safe to close socket here
                connection.socket.Close();
            }
            
            // note client must manually continue the networking loop via the GetData method. See method doc for more info.
        }

        /// <summary>
        /// This callback is invoked when the message is sent.
        /// </summary>
        /// <param name="ar">result for the send operation</param>
        private static void SendCallback(IAsyncResult ar)
        {
            SocketState connection = (SocketState)ar.AsyncState;
            connection.socket.EndSend(ar);        // just need to conclude the send operation
        }
    }
}
