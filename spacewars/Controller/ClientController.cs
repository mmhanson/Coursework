// This is a controller class for the Space Wars game. It is responsible for the manipuation of various aspects
// of the game's view as well as managing the client-side portion of network connections.
//
// Written by Joshua Cragun (u1025691) for CS 3500 at the University of Utah on November 9th, 2018
using NetworkController;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpaceWars;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Controller
{
    public class ClientController
    {
        // === EVENT DELEGATE TYPES ===
        public delegate void ConnectionFailedHandler(string errorMessage);
        public delegate void ModelChangedDel();
        public delegate void WorldMadeDel(SpaceWarsWorld world, string name, int id);
        // === EVENTS ===
        public event ConnectionFailedHandler ConnectionFailed;      // when a connection cannot be established
        public event ModelChangedDel ModelChangedEvent;             // when the model has been modified by the controller
        public event WorldMadeDel ConnectionEstablished;            // when the world info is received and construction

        /// <summary>
        /// The socket used to connect to the server
        /// </summary>
        private Socket Server;
        /// <summary>
        /// The model
        /// </summary>
        private SpaceWarsWorld World;
        /// <summary>
        /// THe name of the client. It is the same as the username in the game.
        /// </summary>
        public string ClientName { get; internal set; }
        /// <summary>
        /// THe client's ID. Both this and the client name is used in rendering the user's custom ship.
        /// </summary>
        public int? ClientID { get; internal set; }

        // === KEY PROPERTIES ===
        // these are true if the corresponding keys are being pressed and false if not
        private Boolean firePressed;
        private Boolean leftPressed;
        private Boolean rightPressed;
        private Boolean thrustPressed;

        /// <summary>
        /// Default constructor for the client controller
        /// </summary>
        public ClientController()
        {
            ClientName = null;
            // Used in determining what has, and has not been sent by the server yet
            ClientID = null;
            World = null;
            // set all key strokes to not being pressed
            firePressed = false;
            leftPressed = false;
            rightPressed = false;
            thrustPressed = false;
        }

        /// <summary>
        /// Called by the network controller whenever a message is received from the server.
        /// 
        /// <remarks>
        /// This is only called for regular traffic after the startup sequence.
        /// </remarks>
        /// </summary>
        /// <param name="connection">The SocketState on which the message was received</param>
        private void ProcessMessage(SocketState connection)
        {
            //if (connection.socket.Poll(1000, SelectMode.SelectRead) && connection.socket.Available == 0)
            //{
            //    ConnectionFailed("Lost connection to server.");
            //    return;
            //}
            // get a list of complete json strings from the network controller
            LinkedList<String> jsonStrings = new LinkedList<String>();
            lock (connection.overflowBuffer)        // ensure overflow buffer is not modified by other threads while were processing it
            {
                // complete json string are extracted to the list and any incomplete objects on the end are put back
                String dataReceived = connection.overflowBuffer.ToString();
                connection.overflowBuffer.Clear();
                jsonStrings = SplitCompoundJsonString(ref dataReceived, "\n");
                connection.overflowBuffer.Append(dataReceived);
            }

            lock (this.World)       // lock the model so the view wont read from it while were editing it
            {
                // entity timeout logic
                foreach (Ship ship in World.Ships)  // update timer in all ships
                {
                    ship.CountDown();
                }
                World.Ships.RemoveWhere(ship => ship.TimedOut());   // remove all timed out ships
                foreach (Star star in World.Stars)
                {
                    star.CountDown();
                }
                World.Stars.RemoveWhere(star => star.TimedOut());   // remove all timed out stars

                this.UpdateModelFromJson(jsonStrings);
            }

            // Let the view know that the model was changed
            ModelChangedEvent();

            // Send the user input to the server
            if (KeyIsPressed())
            {
                // construct a string which is the action codes for any/all keys currently pressed surrounded by parenthesis
                String inputString = "(";
                if (firePressed)
                {
                    inputString += "F";
                }
                if (rightPressed)
                {
                    inputString += "R";
                }
                if (leftPressed)
                {
                    inputString += "L";
                }
                if (thrustPressed)
                {
                    inputString += "T";
                }
                inputString += ")\n";       // \n important as it delimits the command

                Networking.Send(inputString, connection);
            }

            Networking.GetData(connection);
        }

        /// <summary>
        /// Take a list of json strings representing entities in the world and update the world to reflect them.
        /// </summary>
        /// <param name="jsonStrings">strings to update the world to reflect</param>
        private void UpdateModelFromJson(LinkedList<String> jsonStrings)
        {
            foreach (string jsonString in jsonStrings)
            {
                /*
                 * Classify the json string, create an object, and add it to the game model
                 * We can determine the type simply by looking at the 4th character of the JSON object
                 * {"ship": ... 4th char = h -> we have a ship
                 * {"star": ... 4th char = t -> we have a start
                 * {"proj": ... 4th char = r -> we have a projectile
                 */
                if (jsonString[3] == 'h')
                {
                    Ship ship = JsonConvert.DeserializeObject<Ship>(jsonString);
                    // replace the ship info if it's already in the world
                    if (World.Ships.Contains(ship))
                    {
                        World.Ships.Remove(ship);
                    }
                    World.Ships.Add(ship);
                }
                else if (jsonString[3] == 't')
                {
                    Star star = JsonConvert.DeserializeObject<Star>(jsonString);
                    // Replace the star info if it's already in the world
                    if (World.Stars.Contains(star))
                    {
                        World.Stars.Remove(star);
                    }
                    World.Stars.Add(star);
                }
                else if (jsonString[3] == 'r')
                {
                    Projectile proj = JsonConvert.DeserializeObject<Projectile>(jsonString);
                    // Replace the ship info if it's already in the world
                    if (World.Projectiles.Contains(proj))
                    {
                        World.Projectiles.Remove(proj);
                    }
                    // onl add active projectiles
                    if (proj.IsActive)
                    {
                        World.Projectiles.Add(proj);
                    }
                }
                // if json string cannot be classified to an entity, ignore it
            }
        }

        /// <summary>
        /// Split a string of contiguous json strings into a list of individual json strings.
        /// <remarks>
        /// Used as a helper method for ProcessMessage.
        /// Any non-json-formatted strings that are delimited by the parameterized delimiter will
        /// be discarded and not returned in the list. incomplete data on the end of the compound
        /// json parameter will be left in that object when the method returns.
        /// </remarks>
        /// </summary>
        /// <param name="compoundJson">
        /// The string containing one long json string. Note that this string is allowed to contain
        /// an incomplete json object at the end, in which case this method will remove only the complete
        /// json objects from the front and keep the last, incomplete json string here.
        /// </param>
        /// <param name="delimiter">The seqence of characters separating each individual json string</param>
        /// <returns>A linked list of the extracted, individual json-formatted strings</returns>
        private static LinkedList<String> SplitCompoundJsonString(ref String compoundJson, String delimiter)
        {
            // NOTE this process can be done with less code if regexes are used, but its slower and harder to replace incomplete data

            // extract json substrings into a list
            LinkedList<String> jsonStrings = new LinkedList<String>();
            while (compoundJson.Contains(delimiter))     // since \n delimits json objects, iterate until no delimiters and extract each object
            {
                int delimiterIdx = compoundJson.IndexOf(delimiter);
                String jsonCandidate = compoundJson.Substring(0, delimiterIdx); // extracts chars in range [0, delimiterIdx)

                // if it is considered valid json, then append it to the list of json objets
                // json is considered valid if it starts with '{' and ends with '}'. This may need to be updated to a more rigorous definition.
                if (jsonCandidate.StartsWith("{") && jsonCandidate.EndsWith("}"))
                {
                    jsonStrings.AddLast(jsonCandidate);
                }

                // remove the json substring from dataReceived whether its valid or not
                compoundJson = compoundJson.Remove(0, delimiterIdx + 1);       // '+1' is so the \n char removed too
            }

            return jsonStrings;
        }

        /// <summary>
        /// Callboack for successful connection to server
        /// </summary>
        /// <param name="ss"></param>
        private void SocketConnected(SocketState ss)
        {
            // Send client name to server
            Networking.Send(ClientName + "\n", ss);
            ss.informClient = WorldInfoReceived;
            Networking.GetData(ss);

        }


        /// <summary>
        /// One time delegate use for building the world. It parses the message looking for a player ID and a world size.
        /// It constructs the world and triggers an event in the View. Afterwards, it modifies the dataReceived callback to its standard method.
        /// </summary>
        /// <param name="ss"></param>
        private void WorldInfoReceived(SocketState ss)
        {
            // TODO: first # is guaranteed to be id, and second is always world size. Code can be simplified.
            string currentData = ss.overflowBuffer.ToString();
            string[] parts = Regex.Split(currentData, @"(?<=[\n])");    // basically splits the buffer by '\n' into an array

            foreach (string dataChunk in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (dataChunk.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (dataChunk[dataChunk.Length - 1] != '\n')
                    break;
                // The first message should be a client ID
                // First check to see if we haven't already gotten the ID (in case we received incomplete data)
                if (ReferenceEquals(ClientID, null))
                {
                    // Remove the tail \n
                    string possibleID = dataChunk.Substring(0, dataChunk.Length - 1);
                    int ID;
                    if (Int32.TryParse(possibleID, out ID))
                    {
                        ClientID = ID;
                        // Remove the data chunk from the buffer if we had use for it
                        ss.overflowBuffer.Remove(0, dataChunk.Length);
                    }
                }
                // Now if we have the ID, check to see if can find the world size
                else if (ReferenceEquals(World, null))
                {
                    // Remove the tail \n
                    string possibleWorldSize = dataChunk.Substring(0, dataChunk.Length - 1);
                    int Size;
                    if (Int32.TryParse(possibleWorldSize, out Size))
                    {
                        World = new SpaceWarsWorld(Size);
                        // Remove the data chunk from the buffer if we had use for it
                        ss.overflowBuffer.Remove(0, dataChunk.Length);
                    }
                }
            }

            // If we haven't received our client ID and the world size, keep waiting until we do
            if (ReferenceEquals(World, null) || ReferenceEquals(ClientID, null))
            {
                Networking.GetData(ss);
                return;
            }

            // now use ProcessMessage to handle all other traffic
            ss.informClient = ProcessMessage;
            ConnectionEstablished(World, ClientName, ClientID.Value);
            Networking.GetData(ss);
        }

        /// <summary>
        /// This method connects the controller to a server. It requires that the client supply his/her callname as well.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="name"></param>
        public void Connect(string ip, string name)
        {
            ClientName = name;
            // Try to connect to the server
            try
            {
                Server = Networking.ConnectToServer(ip, SocketConnected).socket;
            }
            catch (Exception e)
            {
                // If there was a problem, trigger the ConnectionFailed event
                ConnectionFailed(e.Message);
            }
        }

        /// <summary>
        /// This method is triggerd by the view when a key is pressed.
        /// </summary>
        /// <param name="actionCode">
        /// actionCode is F(ire), R(ight), L(eft), or T(hurst) depending on what the user presses
        /// </param>
        public void KeyPressed(String actionCode)
        {
            switch(actionCode)
            {
                case "F":
                    this.firePressed = true;
                    break;
                case "R":
                    this.rightPressed = true;
                    break;
                case "L":
                    this.leftPressed = true;
                    break;
                case "T":
                    this.thrustPressed = true;
                    break;
            }
        }

        /// <summary>
        /// This method is triggerd by the view when a key is pressed.
        /// </summary>
        /// <param name="actionCode">
        /// actionCode is F(ire), R(ight), L(eft), or T(hurst) depending on what the user presses
        /// </param>
        public void KeyReleased(String actionCode)
        {
            switch (actionCode)
            {
                case "F":
                    this.firePressed = false;
                    break;
                case "R":
                    this.rightPressed = false;
                    break;
                case "L":
                    this.leftPressed = false;
                    break;
                case "T":
                    this.thrustPressed = false;
                    break;
            }
        }

        /// <summary>
        /// Helper method to determine if the user is pressing any keys;
        /// </summary>
        /// <returns></returns>
        private Boolean KeyIsPressed()
        {
            return (this.firePressed || this.rightPressed || this.leftPressed || this.thrustPressed);
        }
    }
}
