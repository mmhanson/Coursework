using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace AdministrativeClient
{
    public class AdminClientController
    {
        public delegate void ReceivedUpdate();
        private event ReceivedUpdate UpdateHandler;

        public delegate void ReceivedClientList();
        private event ReceivedClientList ClientHandler;

        public delegate void DeleteInformation();
        private event DeleteInformation CleanGuiHandler;

        public delegate void ConnectionMade();
        private event ConnectionMade ConnectionHandler;

        public delegate void Error(Exception e);
        private event Error ErrorHandler;

        private AdminState state;
        private Socket ourSocket;

        public AdminClientController(AdminState state)
        {
            this.state = state;
        }

        public void RegisterUpdateHandler(ReceivedUpdate toRegister)
        {
            UpdateHandler += toRegister;
        }
        public void RegisterCleanGuiHandler(DeleteInformation toRegister)
        {
            CleanGuiHandler += toRegister;
        }

        public void RegisterClientHandler(ReceivedClientList toRegister)
        {
            ClientHandler += toRegister;
        }

        public void RegisterConnectionHandler(ConnectionMade toRegister)
        {
            ConnectionHandler += toRegister;
        }

        public void RegisterErrorHandler(Error toRegister)
        {
            ErrorHandler += toRegister;
        }

        public void ConnectToServer(string address)
        {
            NetworkController.ConnectToServer(ConnectedCallback, ErrorCallback, address);
        }

        private void ConnectedCallback(SocketState ss)
        {
            this.ourSocket = ss.theSocket;
            NetworkController.Send(ourSocket, "{\"type\": \"admin connect\"}" + "\n\n");
            ss.SetNetworkAction(ReceiveUpdates);
            NetworkController.GetData(ss);
            state.Connected = true;
            ConnectionHandler();
        }

        private void ErrorCallback(Exception e)
        {
            Console.WriteLine("Error connecting to server " + e.Message);
            ErrorHandler(e);
        }

        private void ReceiveUpdates(SocketState ss)
        {
            List<string> data = (List<string>)ProcessMessage(ss);
            foreach(string message in data)
            {
                if (message.Contains("update"))
                    ProcessandDisplayUpdate(message);
                else if (message.Contains("clientlist"))
                    ProcessandDisplayClients(message);
            }
            NetworkController.GetData(ss);
        }

        public void SendShutdownMessage()
        {
            CleanGuiHandler();
            ShutDown shutdown = new ShutDown();
            string jsonMessage = JsonConvert.SerializeObject(shutdown) + "\n\n";
            NetworkController.Send(ourSocket, jsonMessage);
        }

        public void SendDeleteClientRequest(string username)
        {
            ManageClient deleteClient = new ManageClient("delete", username);
            string jsonMessage = JsonConvert.SerializeObject(deleteClient) + "\n\n";
            NetworkController.Send(ourSocket, jsonMessage);
        }

        public void SendCreateClientRequest(string username)
        {
            ManageClient createClient = new ManageClient("create", username);
            string jsonMessage = JsonConvert.SerializeObject(createClient) + "\n\n";
            NetworkController.Send(ourSocket, jsonMessage);
        }

        public void SendCreateSpreadsheetRequest(string sheetName)
        {
            ManageSheet createSheet = new ManageSheet("create", sheetName);
            string jsonMessage = JsonConvert.SerializeObject(createSheet) + "\n\n";
            NetworkController.Send(ourSocket, jsonMessage);
        }

        public void SendDeleteSpreadsheetRequest(string sheetName)
        {
            ManageSheet deleteSheet = new ManageSheet("delete", sheetName);
            string jsonMessage = JsonConvert.SerializeObject(deleteSheet) + "\n\n";
            NetworkController.Send(ourSocket, jsonMessage);
        }

        public void SendChangePasswordRequest(string username, string newPassword)
        {
            ChangePassword newpwrd = new ChangePassword(username, newPassword);
            string jsonMessage = JsonConvert.SerializeObject(newpwrd) + "\n\n";
            NetworkController.Send(ourSocket, jsonMessage);
        }

        public void ProcessandDisplayUpdate(string message)
        {
            Update newUpdate = JsonConvert.DeserializeObject<Update>(message);
            state.AddChange(newUpdate.username + " performed action \"" + newUpdate.status + "\" and changed cell " + newUpdate.cell + "\n");
            UpdateHandler();
        }

        public void ProcessandDisplayClients(string message)
        {
            ClientList list = JsonConvert.DeserializeObject<ClientList>(message);
            state.SetClientList(list.GetList());
            ClientHandler();
        }

        /// <summary>
        /// Closes the socket connection after shutting down socket from sending and receiving.
        /// </summary>
        public void CloseConnection()
        {
            ourSocket.Shutdown(SocketShutdown.Both);
            ourSocket.Close();
            state.Connected = false;
        }

        /// <summary>
        /// Helper method to append messages and filter JSON strings sent from server. 
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        private IEnumerable<string> ProcessMessage(SocketState ss)
        {
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=\n{2})");

            List<string> messages = new List<string>();

            // Loop until we have processed all messages.
            // We may have received more than one.
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                //Add to message list
                messages.Add(p);

                // Then remove it from the SocketState's growable buffer
                ss.sb.Remove(0, p.Length);
            }

            return messages;
        }
    }
}
