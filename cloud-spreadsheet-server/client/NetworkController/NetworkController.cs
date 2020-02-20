using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    //Delgates for network actions
    public delegate void NetworkAction(SocketState ss);
    public delegate void ErrorAction(Exception e);


    /// <summary>
    /// SocketState encapsulates information about a given socket. 
    /// </summary>
    public class SocketState
    {       
        //Holds the socket for this state
        public Socket theSocket;

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        //Delegates for network actions
        public NetworkAction callMe { get; private set; }
        public ErrorAction errorCallMe { get; private set; }

        //Stores unique ID
        private string ID;

        /// <summary>
        /// Create a new SocketState for param s
        /// </summary>
        /// <param name="s"></param>
        public SocketState(Socket s)
        {
            theSocket = s;
            ID = "";
        }

        /// <summary>
        /// Assign an action for the callback
        /// </summary>
        /// <param name="c"></param>
        public void SetNetworkAction(NetworkAction c)
        {
            this.callMe = c;
        }

        /// <summary>
        /// Assign an action for the error callback
        /// </summary>
        /// <param name="c"></param>
        public void SetErrorAction(ErrorAction c)
        {
            this.errorCallMe = c;
        }

        /// <summary>
        /// sets the Socket's ID
        /// </summary>
        /// <param name="id"></param>
        public void SetID(string id)
        {
            this.ID = id;
        }

        /// <summary>
        /// Returns the Socket's ID
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return int.Parse(this.ID);
        }

    }

    public class ConnectionState
    {
        //Holds the TCPListener for this state
        public TcpListener theListener;

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        //Delegates for network actions
        public NetworkAction callMe { get; private set; }
        public ErrorAction errorCallMe { get; private set; }

        //Stores unique ID
        private string ID;

        /// <summary>
        /// Create a new SocketState for param s
        /// </summary>
        /// <param name="listener"></param>
        public ConnectionState()
        {
            ID = "";
        }

        /// <summary>
        /// Assign an action for the callback
        /// </summary>
        /// <param name="callMe"></param>
        public void SetNetworkAction(NetworkAction callMe)
        {
            this.callMe = callMe;
        }

        /// <summary>
        /// Assign an action for the error callback
        /// </summary>
        /// <param name="callMe"></param>
        public void SetErrorAction(ErrorAction callMe)
        {
            this.errorCallMe = callMe;
        }

        /// <summary>
        /// Assigne the listener for this state
        /// </summary>
        /// <param name="listener"></param>
        public void SetTCPListener(TcpListener listener)
        {
            this.theListener = listener;
        }

        /// <summary>
        /// sets the Socket's ID
        /// </summary>
        /// <param name="id"></param>
        public void SetID(string id)
        {
            this.ID = id;
        }

    }

    /// <summary>
    /// NetworkController class handles establishing and managing a connection with a given server. 
    /// </summary>
    public static class NetworkController
    {
        private const int DEFAULT_PORT = 2112;

        /// <summary>
        /// ConnectToServer attempts to connect to the server via a provided hostname. 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="em"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction cm, ErrorAction em, string hostName)
        {
            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            MakeSocket(hostName, out socket, out ipAddress);

            SocketState ss = new SocketState(socket);

            ss.SetNetworkAction(cm);
            ss.SetErrorAction(em);

            //Begin event loop
            socket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, ss);

            return socket;
        }

        /// <summary>
        /// This function is referenced by the BeginConnect method above and is called by the OS when the socket connects to the server.
        /// </summary>
        /// <param name="stateAsArObject"></param>
        private static void ConnectedCallback(IAsyncResult stateAsArObject)
        {
            SocketState ss = (SocketState)stateAsArObject.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(stateAsArObject);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                ss.errorCallMe(e);
                return;
            }

            // Invoke call me given by socket state
            ss.callMe(ss);
        }

        /// <summary>
        /// GetData will request the server send information through the given SocketState object. 
        /// </summary>
        /// <param name="state"></param>
        public static bool GetData(SocketState state)
        {
            bool success = true;
            try
            {
                state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Private helper method which processes the information received from server. 
        /// </summary>
        /// <param name="stateAsArObject"></param>
        private static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            SocketState ss = (SocketState)stateAsArObject.AsyncState;

            try
            {
                int bytesRead = ss.theSocket.EndReceive(stateAsArObject);
                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    ss.sb.Append(theMessage);

                    ss.callMe(ss);
                }
            }
             catch (Exception e)
            {
                Console.WriteLine(e.Message);  
            }
        }

        /// <summary>
        /// Sends data from param to the server from the given socket. 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static bool Send(Socket socket, String data)
        {
            SocketState socketState = new SocketState(socket);

            byte[] byteDataToSend = Encoding.UTF8.GetBytes(data);

            bool success = true;
            try
            {
                if (socket.Connected)
                {
                    socket.BeginSend(byteDataToSend, 0, byteDataToSend.Length, SocketFlags.None, SendCallback, socketState);
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {                
                socket.Close(); 
                success = false;
                Console.WriteLine("Invalid socket. " + e.Message);
            }

            return success;
        }

        /// <summary>
        /// Private helper method to complete BeginSend. 
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            SocketState s = (SocketState)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            try
            {
                if (s.theSocket.Connected)
                {
                    s.theSocket.EndSend(ar);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client disconnected. " + e.Message);
                if (s.theSocket.Connected)
                {
                    s.theSocket.Close();
                }
            }
        }

        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        private static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
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
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }
    }
}
