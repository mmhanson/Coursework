using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkController;

namespace Testing
{
    /// <summary>
    /// Test the NetworkController class
    /// </summary>
    [TestClass]
    public class NetworkControllerUnitTests
    {
        /*
        // connect on port 11000
        private static int serverListeningPort = 11000;

        // === Black-Box Tests ===

        /// <summary>
        /// Test that a single client can connect.
        /// 
        /// Tests that the client connection callback is invoked and the server
        /// connection callback is invoked.
        /// </summary>
        [TestMethod]
        public void TestSingleClientConnection()
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            bool serverCallbackInvoked = false;
            bool clientCallbackInvoked = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            // start accepting new clients and set 'callbackInvoked' to true when one connects
            NetworkAction serverCb = (SocketState ss) => serverCallbackInvoked = true;
            Networking.StartAcceptingNewClients(serverCb);

            // connect and set 'clientCallbackInvoked' to true when it connects
            DummyGameClient c = new DummyGameClient(11000);
            NetworkAction clientCb = (SocketState ss) => clientCallbackInvoked = true;
            c.Connect("localhost", clientCb);

            // assert results
            Assert.IsTrue(serverCallbackInvoked);
            Assert.IsTrue(clientCallbackInvoked);
        }

        /// <summary>
        /// Test that a client cannot connect when the StartAcceptingNewClients is not called.
        /// </summary>
        [TestMethod]
        public void TestAttemptedConnectWhenFailedStart()
        {
            // TODO determine what happens when client tries to connect but server not up
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            bool clientCallbackInvoked = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            // connect and set 'clientCallbackInvoked' to true when it connects
            DummyGameClient c = new DummyGameClient(11000);
            NetworkAction clientCb = (SocketState ss) => clientCallbackInvoked = true;
            c.Connect("localhost", clientCb);

            // assert results
            Assert.IsFalse(clientCallbackInvoked);
        }

        /// <summary>
        /// Test that several clients can connect when StartAcceptingNewClients is called.
        /// </summary>
        [TestMethod]
        public void TestManyClientConnections()
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            bool serverCallbackInvoked = false;
            bool client0CallbackInvoked = false;
            bool client1CallbackInvoked = false;
            bool client2CallbackInvoked = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            // start accepting new clients and set 'callbackInvoked' to true when one connects
            NetworkAction serverCb = (SocketState ss) => serverCallbackInvoked = true;
            Networking.StartAcceptingNewClients(serverCb);

            // connect first client
            DummyGameClient c0 = new DummyGameClient(11000);
            NetworkAction client0Cb = (SocketState ss) => client0CallbackInvoked = true;
            c0.Connect("localhost", client0Cb);
            // connect second client
            DummyGameClient c1 = new DummyGameClient(11000);
            NetworkAction client1Cb = (SocketState ss) => client1CallbackInvoked = true;
            c1.Connect("localhost", client1Cb);
            // connect third client
            DummyGameClient c2 = new DummyGameClient(11000);
            NetworkAction client2Cb = (SocketState ss) => client2CallbackInvoked = true;
            c2.Connect("localhost", client2Cb);

            // assert results
            Assert.IsTrue(serverCallbackInvoked);
            Assert.IsTrue(client0CallbackInvoked);
            Assert.IsTrue(client1CallbackInvoked);
            Assert.IsTrue(client2CallbackInvoked);
        }

        /// <summary>
        /// Test that a client can connect a few seconds after StartAcceptingNewClients is called.
        /// </summary>
        [TestMethod]
        public void TestDelayedConnection()
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            bool serverCallbackInvoked = false;
            bool clientCallbackInvoked = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            // start accepting new clients and set 'callbackInvoked' to true when one connects
            NetworkAction serverCb = (SocketState ss) => serverCallbackInvoked = true;
            Networking.StartAcceptingNewClients(serverCb);

            System.Threading.Thread.Sleep(3000);

            // connect and set 'clientCallbackInvoked' to true when it connects
            DummyGameClient c = new DummyGameClient(11000);
            NetworkAction clientCb = (SocketState ss) => clientCallbackInvoked = true;
            c.Connect("localhost", clientCb);

            // assert results
            Assert.IsTrue(serverCallbackInvoked);
            Assert.IsTrue(clientCallbackInvoked);
        }
    }

    // TODO: finish
    internal class DummyGameClient
    {
        /// <summary>
        /// Create a dummy game client that connects on on a port.
        /// </summary>
        /// <param name="port">port this client will connect to.</param>
        public DummyGameClient(int port)
        {

        }

        /// <summary>
        /// Connect this client to a server.
        /// </summary>
        /// <param name="hostname">hostname of the server to connect to</param>
        /// <param name="callback">callback to invoke after connection is made</param>
        public void Connect(String hostname, NetworkAction callback)
        {

        }

        /// <summary>
        /// Called when the client has connected.
        /// </summary>
        private void Connected()
        {

        }

    */
    }
}
