using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkController;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        /// <summary>
        ///  the server that this client is connected to
        /// </summary>
        private SocketState server;

        public Form1()
        {
            InitializeComponent();
            // register the handler for the pressing of the messageToSendBox
            messageToSendBox.KeyDown += new KeyEventHandler(MessageEnterHandler);
        }

        /// <summary>
        /// Handler for when the connectButton is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            // TODO: This needs better error handling. Left as an exercise.
            // If the server box is empty, it gives a message, but doesn't allow us to try to reconnect.
            // It also doesn't handle unreachable addresses.
            if (serverAddress.Text == "")
            {
                MessageBox.Show("Please enter a server address");
                return;
            }

            // Disable the controls and try to connect
            connectButton.Enabled = false;
            serverAddress.Enabled = false;

            // attempt a connection
            this.server = Networking.ConnectToServer(serverAddress.Text, ConnectionCallback, DataReceievedCallback);
        }

        /// <summary>
        /// This method is invoked by the network controller when the connection is established.
        /// </summary>
        /// <param name="sockState">The socket state associated with the connection that was established</param>
        private void ConnectionCallback(SocketState sockState)
        {
            // TODO (just copied from 
        }

        /// <summary>
        /// This method is invoked by the network controller when data is received on the connection.
        /// </summary>
        /// <param name="sockState"></param>
        private void DataReceievedCallback(SocketState sockState)
        {
            // TODO (just copied from ProcessMessage old method)
            string totalData = sockState.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

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

                // Display the message
                // "messages" is the big message text box in the form.
                // We must use a MethodInvoker, because only the thread that created the GUI can modify it.
                this.Invoke(new MethodInvoker(
                  () => messages.AppendText(p)));

                // Then remove it from the SocketState's growable buffer
                sockState.sb.Remove(0, p.Length);
            }
        }

        /// <summary>
        /// This is the event handler when the enter key is pressed in the messageToSend box
        /// </summary>
        /// <param name="sender">The Form control that fired the event</param>
        /// <param name="e">The key event arguments</param>
        private void MessageEnterHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // extract the message and send it
                string message = messageToSendBox.Text;
                messageToSendBox.Text = "";     // erase the message box
                Networking.Send(message, this.server);
            }
        }

    }
}
