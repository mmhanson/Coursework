// This is a controller class for the Space Wars game. It is responsible for the manipuation of various aspects
// of the game's view as well as managing the client-side portion of network connections.
//
// Written by Joshua Cragun (u1025691) for CS 3500 at the University of Utah on November 9th, 2018
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpaceWars;
using Model;
using Testing;
using Newtonsoft.Json.Linq;
using System.Threading;
using NetworkController;

namespace Testing
{
    /// <summary>
    /// This class functions as a dummy controller for testing purposes.
    /// It includes the same interface as the functional controller, but will only include minimal
    /// logic to get the game actually running.
    /// 
    /// <remarks>
    /// This class is used for testing and developing the view.
    /// Even though this class could use the test network controller, it is not necessary.
    /// The test network controller was created for testing the controller, and this class
    /// was created for testing the view. Incorporating network control logic into here
    /// would be unnecessary. Instead, network traffic is simulated in this class and updates
    /// to the world are sent to the view periodically.
    /// </remarks>
    /// </summary>
    public class TestController
    {
        // === EVENT DELEGATE TYPES ===
        public delegate void ConnectionFailedDel(String errorMsg);
        public delegate void ConnectionEstablishedDel(SpaceWarsWorld world);
        public delegate void ModelChangedDel();
        // === EVENTS ===
        public event ConnectionFailedDel ConnectionFailed;
        public event ConnectionEstablishedDel ConnectionEstablished;
        public event ModelChangedDel ModelChangedEvent;

        private SocketState Server;
        private SpaceWarsWorld World;

        /// <summary>
        /// Create a test controller.
        /// </summary>
        public TestController()
        {
            this.Server = null;
            this.World = null;
        }

        /// <summary>
        /// 'Connect' to the server.
        /// 
        /// <remarks>
        /// This does not actually connect to the server, it just creates the model and starts
        /// simulating network traffic for the view
        /// </remarks>
        /// </summary>
        /// <param name="ip"></param>
        public void Connect(string ip, String name)
        {
            this.World = new SpaceWarsWorld(500);       // create the world

            Ship player = new Ship(1);       // create the lone ship
            this.World.Ships.Add(player);

            // start a new thread that will slowly move the ship down and notify the view
            new Thread(UpdateShip).Start() ;

            // inform client connection has been established
            this.ConnectionEstablished(this.World);

            return;
        }

        /// <summary>
        /// Slowly move the player down and update the view.
        /// </summary>
        private void UpdateShip()
        {
            // wait one second
            Thread.Sleep(1000);

            // move ship down 3 pixels
            Ship player = this.World.Ships.First();   // works since there is only one ship in the test model
            Vector2D loc = player.Location;        
            Vector2D newLoc = new Vector2D(loc.GetX(), loc.GetY() + 3);
            player.Location = newLoc;
            
            // notify the view
            int id = player.ShipID;
            String name = player.PlayerName;
            Vector2D location = player.Location;
            Vector2D dir = player.Direction;
            Boolean thrust = player.IsThrusting;
            int hp = player.HitPoints;
            int score = player.Score;
            this.ModelChangedEvent();

            // repeat
            this.UpdateShip();
        }

        /// <summary>
        /// This method is triggerd by the view when a key is pressed.
        /// </summary>
        /// <param name="actionCode">
        /// actionCode is F(ire), R(ight), L(eft), or T(hurst) depending on what the user presses
        /// </param>
        public void KeyPressed(String actionCode)
        {
            // TODO
        }
    }
}
