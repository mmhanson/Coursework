using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SS;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace SpreadsheetGUI
{
    public class SpreadsheetController
    {
        private SpreadsheetState state;
        private Spreadsheet formSpreadsheet;

        public bool authorizing;
        private bool firstUpdate;

        private Socket clientSocket;
        public delegate void ControllerErrorAction(Exception e);
        private event ControllerErrorAction ErrorHandler;

        public delegate void GUIConnectionOption();
        private event GUIConnectionOption ConnectionHandler;

        public delegate void UpdateCameIn();
        private event UpdateCameIn UpdateHandler;

        public delegate void ValidUserReceivedUpdate();
        private event ValidUserReceivedUpdate FirstUpdateHandler;

        public SpreadsheetController(SpreadsheetState state, Spreadsheet formSpreadsheet)
        {
            state.Connected = false;
            firstUpdate = true;
            clientSocket = null;
            this.state = state;
            this.formSpreadsheet = formSpreadsheet;
        }

        /// <summary>
        /// Used to register error handlers for the GUI.
        /// These handlers will be used to display error messages when they are generated during runtime.
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterErrorHandler(ControllerErrorAction handler)
        {
            ErrorHandler += handler;
        }

        public void RegisterConnectionHandler(GUIConnectionOption handler)
        {
            ConnectionHandler += handler;
        }

        public void RegisterUpdateHandler(UpdateCameIn handler)
        {
            UpdateHandler += handler;
        }

        public void RegisterFirstUpdateHandler(ValidUserReceivedUpdate handler)
        {
            FirstUpdateHandler += handler;
        }

        /// <summary>
        /// Attempts to make a connection with the specified server by the ipAddress.
        /// Expects non blank and valid entry for username and password.
        /// Expects non blank and valid entry for ipAddress to existing server running Send_It protocol.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if connection has been made</returns>
        public void ConnectToServer(string ipAddress)
        {
            try
            {
                clientSocket = NetworkController.ConnectToServer(OnConnection, ErrorConnecting, ipAddress); // the connect button to connect to the server;
            }
            catch (Exception e)
            {
                ErrorConnecting(e);
            }
        }

        private void OnConnection(SocketState ss)
        {
            clientSocket = ss.theSocket;
            ss.SetNetworkAction(ReceiveSpreadsheets);
            NetworkController.GetData(ss);
            state.Connected = true;
            authorizing = true;
        }

        private void ReceiveSpreadsheets(SocketState ss)
        {
            List<string> data = (List<string>)ProcessMessage(ss);
            SpreadsheetList sprdList = JsonConvert.DeserializeObject<SpreadsheetList>(data[0].TrimEnd());
            foreach (string str in sprdList.spreadsheets)
            {
                Console.WriteLine(str);
            }
            lock (state)
            {
                state.AvailableSpreadsheets = sprdList.spreadsheets;
            }

            ss.SetNetworkAction(ReceiveUpdates);
            NetworkController.GetData(ss);

            ConnectionHandler();
        }

        private void ReceiveUpdates(SocketState ss)
        {
            if (firstUpdate)
            {
                FirstUpdateHandler();
                firstUpdate = false;
            }

            List<string> data = (List<string>)ProcessMessage(ss);
            string message = data[0];

            if (message.Contains("error"))
            {
                Error error = JsonConvert.DeserializeObject<Error>(message);

                if (authorizing && error.GetCode() == 1)
                {
                    ss.SetNetworkAction(ReceiveSpreadsheets);
                    NetworkController.GetData(ss);
                    return;
                }

                else if (!authorizing && error.GetCode() == 2)
                {
                    // Display error for circular dependency
                    ErrorHandler(new Exception("Change has been rejected for causing a circular dependancy. Please enter a different value."));
                }
            }

            if (message.Contains("full send"))
            {
                authorizing = false;
                FullSend changes = JsonConvert.DeserializeObject<FullSend>(message);

                // Get the spreadsheet and display it
                Dictionary<string, string> newSpreadsheet = changes.GetSpreadsheet();
                state.RecentChanges = newSpreadsheet;
                UpdateHandler();
            }

            ss.SetNetworkAction(ReceiveUpdates);
            NetworkController.GetData(ss);
        }

        private void ErrorConnecting(Exception e)
        {
            ErrorHandler(new Exception("Failed to connect to server. " + e.Message));
        }

        /// <summary>
        /// Closes the socket connection after shutting down socket from sending and receiving.
        /// </summary>
        public void CloseConnection()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            state.Connected = false;
        }

        public void SendOpenRequest()
        {
            Open openRequest = new Open(state.SelectedSpreadsheet, state.Username, state.Password);

            string jsonMessage = JsonConvert.SerializeObject(openRequest);

            NetworkController.Send(clientSocket, jsonMessage + "\n\n");
        }

        public void SendEditRequest(string cell, string content, List<string> dependencies)
        {
            Edit editRequest = new Edit(cell, content, dependencies);

            string jsonMessage = JsonConvert.SerializeObject(editRequest);

            NetworkController.Send(clientSocket, jsonMessage + "\n\n");
        }

        public void SendRevertRequest(string cell)
        {
            Revert revertRequest = new Revert(cell);

            string jsonMessage = JsonConvert.SerializeObject(revertRequest);

            NetworkController.Send(clientSocket, jsonMessage + "\n\n");
        }

        public void SendUndoRequest()
        {
            Undo undoRequest = new Undo();

            string jsonMessage = JsonConvert.SerializeObject(undoRequest);

            NetworkController.Send(clientSocket, jsonMessage + "\n\n");
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
