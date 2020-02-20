using Model;
using NetworkController;
using System;

namespace Server
{
    /// <summary>
    /// A client that is connected to the server.
    /// </summary>s
    class Client
    {
        /// <summary>
        /// The name that the client requested from the server.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The clients connection to the server.
        /// </summary>
        public SocketState Connection { get; set; }

        /// <summary>
        /// The id number of the client.
        /// </summary>
        public Int32 IdNum { get; set; }

        /// <summary>
        /// The clients ship.
        /// </summary>
        public Ship Ship
        {
            internal set; get;
        }

        public Client(Int32 idNum, String name, Ship ship, SocketState connection)
        {
            IdNum = idNum;
            Name = name;
            Connection = connection;
            // setting the id in the connection allows networking callbacks to find the sender
            connection.id_num = idNum;
            Ship = ship;
        }

        /// <summary>
        /// Send a message to this client
        /// </summary>
        /// <param name="message"></param>
        public void SendTo(String message)
        {
            try
            {
                Networking.Send(message, Connection);
            }
            // thrown when client disconnects between the send operation starting and finishing
            // ignore it
            catch (System.Net.Sockets.SocketException e)
            {
                return;
            }
        }

        /// <summary>
        /// Start receiving data from a client.
        /// </summary>
        public void GetData()
        {
            try
            {
                Networking.GetData(this.Connection);
            }
            // thrown when client disconnects between the send operation starting and finishing
            // ignore it
            catch (System.Net.Sockets.SocketException e)
            {
                return;
            }
        }
    }
}
