using Model;
using NetworkController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Server
{
    /// <summary>
    /// A game server.
    /// 
    /// <remarks>
    /// This server works for both gamemodes. The server class acts as the 'controller'
    /// portion of the MVC architecture. Since the server doesn't have a view, there
    /// is not really a 'V' part of MVC here. However, the Server class acts as the
    /// controller and the World classes act as the model for their respective
    /// gamemodes.
    /// 
    /// The server handles everything relating to dealing with clients and timing the
    /// frames. The world handles everything relating to the mechanics of the game.
    /// </remarks>
    /// </summary>
    class Server
    {
        /// <summary>
        /// The id that the next client that connects will have.
        /// 
        /// <remarks>
        /// This is incremented for each client that connects.
        /// </remarks>
        /// </summary>
        private int nextId;

        /// <summary>
        /// Dictionary of clients connected to this server.
        /// 
        /// <remarks>
        /// Maps an id to a client. Client classes consist of a name, a connection,
        /// and a Ship. The ship is needed to quickly determine what ship to update
        /// when the client sends a command. Each client in this set receives
        /// updates from the server every frame.
        /// </remarks>
        /// </summary>
        private Dictionary<Int32, Client> clients;

        /// <summary>
        /// The model of the world.
        /// 
        /// <remarks>
        /// The model of the game is different depending on the game type. Any model
        /// can be created an used here as long as it implemnets the World interface.
        /// </remarks>
        /// </summary>
        private readonly IWorld world;

        /// <summary>
        /// How many milliseconds to wait between model updates.
        /// 
        /// <remarks>
        /// This is how long the server waits between completing a frame and
        /// computing the next frame.
        /// </remarks>
        /// </summary>
        private Int32 msPerFrame;

        /// <summary>
        /// The gamemode.
        /// 
        /// <remarks>
        /// The only supported gamemodes are: "SpaceWars" and "SpaceInvaders".
        /// See the readme for information about the gamemodes.
        /// </remarks>
        /// </summary>
        private String gameMode;

        /// <summary>
        /// Relative path for the xml settings file.
        /// </summary>
        private readonly static String xmlSettingsPath = @"..\..\..\Resources\settings.xml";

        /// <summary>
        /// Start the server.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(String[] args)
        {
            Server server = new Server(xmlSettingsPath);
            // game loop
            while (true)
            {
                server.Update();
            }
        }

        /// <summary>
        /// Create a default server.
        /// 
        /// <remarks>
        /// See the readme for information on what the default settings are.
        /// </remarks>
        /// </summary>
        public Server()
        {
            this.nextId = 0;
            this.clients = new Dictionary<Int32, Client>();

            // server settings
            this.gameMode = "SpaceWars";
            this.msPerFrame = 15;
        }

        /// <summary>
        /// Create a server and listen for clients.
        /// </summary>
        public Server(String xmlSettingsPath) : this()
        {
            ParseXmlSettings(xmlSettingsPath);

            // create the appropriate world
            if (this.gameMode.Equals("SpaceInvaders"))
            {
                this.world = new SpaceInvadersWorld(xmlSettingsPath);
            }
            // if new gamemodes are added, then map them to the correct models here
            /*
             * else if (this.gameMode.Equals(*custom gamemode name*)
             * {
             *     this.world = new *Custom model*
             * }
             */
            else
            {
                // use default gamemode (SpaceWars)
                this.world = new SpaceWarsWorld(xmlSettingsPath);
            }

            Console.WriteLine("The chosen gamemode is: " + this.gameMode);
            Console.WriteLine("Server is up, listening for clients.");

            // start listening for new clients and call 'handleNewClient' when one connects
            Networking.StartAcceptingNewClients(HandleNewClient);
        }

        /// <summary>
        /// Send the world to each client and advance the model by one instant (frame).
        /// 
        /// <remarks>
        /// This method is meant to be called in an infinite loop. It will update the model
        /// and then wait for the time interval specified in the settings to return.
        /// </remarks>
        /// </summary>
        public void Update()
        {
            // serialize the entire world and send to each client
            String worldJsonString = "";
            lock (this.world)
            {
                foreach (Ship ship in this.world.Ships)
                {
                    String jsonString = GetJsonString(ship) + '\n';
                    worldJsonString += jsonString;
                }
                foreach (Projectile proj in this.world.Projectiles)
                {
                    String jsonString = GetJsonString(proj) + '\n';
                    worldJsonString += jsonString;
                }
                foreach (Star star in this.world.Stars)
                {
                    String jsonString = GetJsonString(star) + '\n';
                    worldJsonString += jsonString;
                }
            }
            SendToAllClients(worldJsonString);

            // advance needs to happen after sending otherwise texture bugs arise
            this.world.Advance();
            Thread.Sleep(msPerFrame);
        }

        // === Network Event Handlers ===

        /// <summary>
        /// Called when a new client connects to the server.
        /// 
        /// <remarks>
        /// After this method is called, the server will wait for the client
        /// to send their name.
        /// </remarks>
        /// </summary>
        /// <param name="connection">The socket state the client is on.</param>
        private void HandleNewClient(SocketState connection)
        {
            // wait for the client to send its name
            connection.informClient = ReceiveName;
            // if not connected, do nothing
            if (connection.IsConnected)
            {
                Networking.GetData(connection);
            }
        }

        /// <summary>
        /// Called when the server receives a client's name.
        /// 
        /// <remarks>
        /// If the clients name is invalid, then the connection is terminated. 
        /// Only empty strings are invalid names.
        /// This method will setup the client to receive updates and start receiving
        /// input from the client.
        /// </remarks>
        /// </summary>
        /// <param name="connection">Socket state of the client</param>
        private void ReceiveName(SocketState connection)
        {
            // get the name
            String name;
            lock (connection.overflowBuffer)
            {
                name = connection.overflowBuffer.ToString();
                connection.overflowBuffer.Clear();
            }

            // validate the name
            if (!IsValidName(name))
            {
                TerminateClient(connection);
            }

            // spawn the new players ship
            Ship playerShip;
            lock (this.world)
            {
                playerShip = this.world.SpawnNewShip(nextId, name);
            }

            // create a client instance
            Client newClient = new Client(nextId, name, playerShip, connection);
            lock (this.clients)
            {
                clients.Add(nextId, newClient);             // map the id to the client
            }
            nextId++;                                   // increment id for next client

            // send world data
            String worldData = newClient.Connection.id_num + "\n" + this.world.Size + "\n";
            newClient.SendTo(worldData);


            // start receiving input from the client
            newClient.Connection.informClient = ReceiveInput;
            newClient.GetData();
        }

        /// <summary>
        /// Called whenever a client sends input (commands) to the server.
        /// 
        /// <remarks>
        /// "Input" commands take the form of "(*chars*)" where *chars* is a sequence
        /// of uppercase characters from the set {F, R, L, T} where F is 'fire', T is
        /// 'thrust', R is 'right turn', and L is 'left turn'. There can be, at most, 
        /// one of each character between the parenthesis and, at least, zero of each
        /// character between the parethesis.
        /// 
        /// The connection is terminated if the client does not follow this protocol.
        /// </remarks>
        /// </summary>
        /// <param name="connection">connection input was sent on</param>
        private void ReceiveInput(SocketState connection)
        {
            Int32 clientId = connection.id_num;
            Client client;
            Ship clientShip;

            if (clients.ContainsKey(clientId))
            {
                client = clients[clientId];
                clientShip = client.Ship;
            }
            else
            {
                // discard input if no corresponding client exists
                // sometimes input is sent after client is deleted when a client disconnects
                return;
            }

            // separate each command
            //   if a key is held down, sometimes several commands is sent in one input.
            String compoundInput;
            lock (connection.overflowBuffer)
            {
                compoundInput = connection.overflowBuffer.ToString();
                connection.overflowBuffer.Clear();
            }
            LinkedList<String> inputs = SplitClientInputString(compoundInput);

            // one input is a multitude of actions wrapped by parenthesis e.g. "(F)"
            // handle each input
            foreach (String input in inputs)
            {

                if (!IsValidInput(input))
                {
                    TerminateClient(connection);
                    return;
                }

                // handle each action
                // one action is one letter e.g. 'T'
                String actions = input.Substring(1, input.Length - 1);  // strip parenthesis
                // dont need lock here since each action handler doesnt modify world
                foreach (Char action in actions)
                {
                    HandleAction(action, clientShip);
                }
            }

            // remove player and return if no longer connected
            if (client.Connection.IsConnected)
            {
                client.GetData();
            }
            else
            {
                TerminateClient(connection);
            }
        }

        /// <summary>
        /// Determine if a client name is valid.
        /// 
        /// <remarks>
        /// A client name is valid iff it is not an empty string.
        /// </remarks>
        /// </summary>
        /// <param name="name">name to validate</param>
        /// <returns></returns>
        private Boolean IsValidName(String name)
        {
            return (!name.Equals(""));
        }

        /// <summary>
        /// Make the appropriate call to the model based off of an action from a client.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="clientShip">The ship to apply the action to</param>
        private void HandleAction(Char action, Ship clientShip)
        {
            switch (action)
            {
                case ('F'):
                    this.world.Fire(clientShip);
                    break;
                case ('T'):
                    this.world.Thrust(clientShip);
                    break;
                case ('R'):
                    this.world.Turn(clientShip, true);
                    break;
                case ('L'):
                    this.world.Turn(clientShip, false);
                    break;
            }
        }

        // === Static Helper Methods ===

        /// <summary>
        /// Determine if an input from a client follows protocol.
        /// 
        /// <remarks>
        /// A command is considerd valid if it
        ///   * is at least 2 characters
        ///   * is enclosed by parenthesis
        ///   * has no trailing whitespace
        ///   * only contains  'F' OR 'T' OR 'L' OR 'R' besides the enclosing parenthesis
        /// </remarks>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Boolean IsValidInput(String input)
        {
            if (input.Length < 2)
            {
                return false;
            }

            // check parenthesis
            if (!input[0].Equals('(') || !input[input.Length - 1].Equals(')'))
            {
                return false;
            }

            // check containing characters
            for (int idx = 1; idx < input.Length - 2; idx++)
            {
                if (!"FTLR".Contains(input[idx]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Serialize an object into a '\n'-terminated, json-formatted string.
        /// </summary>
        /// <param name="obj">objec to serialize</param>
        /// <returns>json string representing object, terminated with '\n'</returns>
        private static String GetJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// Split several inputs into a list of inputs.
        /// 
        /// <remarks>
        /// Returned list of inputs may or may not be valid, it falls
        /// upon the caller to validate that.
        /// Returned inputs are not delimited by '\n'.
        /// </remarks>
        /// </summary>
        /// <param name="compoundInputs">
        /// string of inputs to split. This is defined as one or more '\n'-terminated
        /// substrings.
        /// </param>
        private static LinkedList<String> SplitClientInputString(String compoundInputs)
        {
            // NOTE this process can be done with less code if regexes are used, but its slower and harder to replace incomplete data

            // extract json substrings into a list
            LinkedList<String> inputs = new LinkedList<String>();
            while (compoundInputs.Contains('\n'))     // since \n delimits inputs, iterate until no delimiters and extract each input
            {
                // extract each '\n'-terminated substring ('\n' is not included)
                int delimiterIdx = compoundInputs.IndexOf('\n');
                String input = compoundInputs.Substring(0, delimiterIdx); // extracts chars in range [0, delimiterIdx)
                compoundInputs = compoundInputs.Remove(0, delimiterIdx + 1);       // '+1' is so the \n char removed too

                inputs.AddFirst(input);
            }

            return inputs;
        }

        // === Nonstatic Helper Methods ===

        /// <summary>
        /// Terminate the connection to a client.
        /// 
        /// <remarks>
        /// This is done if any traffic from a client violates protocol or
        /// if the client times out.
        /// </remarks>
        /// </summary>
        /// <param name="connection"></param>
        private void TerminateClient(SocketState connection)
        {
            lock (this.clients)
            {
                this.clients.Remove(connection.id_num);
                connection.socket.Close();
            }
        }

        /// <summary>
        /// Remove a client from the server.
        /// </summary>
        /// <param name="client"></param>
        private void RemoveClient(Client client)
        {
            // remove client from send list
            Int32 clientId = client.IdNum;
            lock (this.clients)
            {
                this.clients.Remove(clientId);
            }

            // remove clients ship and projectiles from world
            this.world.RemovePlayer(clientId);
        }

        /// <summary>
        /// Broadcast a message to all clients connected to the server.
        /// </summary>
        /// <param name="message"></param>
        private void SendToAllClients(String message)
        {
            lock (this.clients)
            {
                foreach (int clientId in clients.Keys)
                {
                    Client client = clients[clientId];
                    if (client.Connection.IsConnected)
                    {
                        client.SendTo(message);
                    }
                    else
                    {
                        // remove if not connected
                        this.RemoveClient(client);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Parse an XML settings file and assign server settings accordingly.
        /// 
        /// <remarks>
        /// This method attempts to read the parameterized xml file and replaces
        /// each default setting with each setting that can be found in the xml
        /// file. This method is meant to be called after every setting is assigned
        /// a default value.
        /// </remarks>
        /// </summary>
        /// <param name="xmlSettingsFilePath"></param>
        private void ParseXmlSettings(String xmlSettingsFilePath)
        {
            // look for the MsPerFrame and GameMode values, replace the corresponding
            //   attributes in the server if they are found
            XDocument settingsDoc = XDocument.Load(xmlSettingsFilePath);
            XElement settings = settingsDoc.Element("settings");

            String XmlMsPerFrame = GetXmlValue(settings, "MsPerFrame");
            if (!ReferenceEquals(null, XmlMsPerFrame))
            {
                this.msPerFrame = Convert.ToInt32(XmlMsPerFrame);
            }

            String XmlGameMode = GetXmlValue(settings, "GameMode");
            if (!ReferenceEquals(null, XmlGameMode))
            {
                this.gameMode = XmlGameMode;
            }
        }

        /// <summary>
        /// Get the value of a tag from an XML document.
        /// 
        /// <remarks>
        /// Note that a clone of this method is available in the world classes, a
        /// better design might consolidate these methods but I wont worry about it.
        /// </remarks>
        /// </summary>
        /// <param name="xml">Xml to get the tag from</param>
        /// <param name="tag">name of the tag</param>
        /// <returns>
        /// value of the tag or null
        /// </returns>
        private static String GetXmlValue(XElement xml, String tag)
        {
            XElement element = xml.Element(tag);
            if (element == null)
            {
                return null;
            }
            else
            {
                return element.Value;
            }
        }
    }
}
