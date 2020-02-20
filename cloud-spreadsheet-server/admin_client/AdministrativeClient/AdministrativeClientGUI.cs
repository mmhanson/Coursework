using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministrativeClient
{
    public partial class AdministrativeClientGUI : Form
    {
        private AdminState state;
        private AdminClientController controller;

        public AdministrativeClientGUI()
        {
            InitializeComponent();
            state = new AdminState();
            controller = new AdminClientController(state);
            controller.RegisterUpdateHandler(UpdateCameIn);
            controller.RegisterClientHandler(ClientListCameIn);
            controller.RegisterCleanGuiHandler(CleanGui);
            controller.RegisterConnectionHandler(ConnectionMade);
            controller.RegisterErrorHandler(ErrorInNetworking);
            ClientBox.ReadOnly = true;
            UpdatesBox.ReadOnly = true;
            RecentActionsBox.ReadOnly = true;

            // Set all buttons to disabled until connection is made
            DisableButtons();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (ConnectButton.Text.Equals("Connect") && state.Connected == false)
                controller.ConnectToServer(AddressBox.Text);
            else if (ConnectButton.Text.Equals("Disconnect") && state.Connected == true)
                controller.CloseConnection();
            ToggleConnectButton();
        }

        private void CreateClientButton_Click(object sender, EventArgs e)
        {
            controller.SendCreateClientRequest(AccountManagementBox.Text);
            RecentActionsBox.Text += "Admin requested to create client " + AccountManagementBox.Text + "\n";

            AccountManagementBox.Clear();
        }

        private void DeleteClientButton_Click(object sender, EventArgs e)
        {
            controller.SendDeleteClientRequest(AccountManagementBox.Text);
            RecentActionsBox.Text += "Admin requested to delete client " + AccountManagementBox.Text + "\n";

            AccountManagementBox.Clear();
        }

        private void SpreadsheetCreateButton_Click(object sender, EventArgs e)
        {
            controller.SendCreateSpreadsheetRequest(SpreadsheetNameBox.Text);
            RecentActionsBox.Text += "Admin requested to create sheet " + SpreadsheetNameBox.Text + "\n";

            SpreadsheetNameBox.Clear();
        }

        private void SpreadsheetDeleteButton_Click(object sender, EventArgs e)
        {
            controller.SendDeleteSpreadsheetRequest(SpreadsheetNameBox.Text);
            RecentActionsBox.Text += "Admin requested to delete sheet " + SpreadsheetNameBox.Text + "\n";

            SpreadsheetNameBox.Clear();
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            controller.SendChangePasswordRequest(UsernameBox.Text, PasswordBox.Text);
            RecentActionsBox.Text += "Admin requested to change the password for " + UsernameBox.Text + " to " + PasswordBox.Text + "\n";

            UsernameBox.Clear();
            PasswordBox.Clear();
        }

        private void ShutdownButton_Click(object sender, EventArgs e)
        {
            controller.SendShutdownMessage();
            controller.CloseConnection();
        }

        private void UpdateCameIn()
        {
            MethodInvoker m = new MethodInvoker(UpdateUpdates);
            this.Invoke(m);
        }

        private void UpdateUpdates()
        {
            this.UpdatesBox.Text += state.GetLastChange();    
        }

        private void ClientListCameIn()
        {
            MethodInvoker m = new MethodInvoker(UpdateClients);
            this.Invoke(m);
        }

        private void UpdateClients()
        {
            ClientBox.Clear();
            foreach (string client in state.GetClientList().Keys)
            {
                ClientBox.Text += client + " is on sheet " + state.GetClientList()[client] +"\n";
            }
        }

        private void CleanGui()
        {
            this.ClientBox.Clear();
            this.UpdatesBox.Clear();
            this.RecentActionsBox.Clear();
        }

        private void ErrorInNetworking(Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            MethodInvoker m = new MethodInvoker(() => { ConnectButton.Enabled = true; ToggleConnectButton(); });
            this.Invoke(m);
        }

        private void ConnectionMade()
        {
            MethodInvoker m = new MethodInvoker(EnableButtons);
            this.Invoke(m);
        }

        private void EnableButtons()
        {
            this.ChangePasswordButton.Enabled = true;
            this.CreateClientButton.Enabled = true;
            this.DeleteClientButton.Enabled = true;
            this.SpreadsheetCreateButton.Enabled = true;
            this.SpreadsheetDeleteButton.Enabled = true;
        }

        private void DisableButtons()
        {
            this.ChangePasswordButton.Enabled = false;
            this.CreateClientButton.Enabled = false;
            this.DeleteClientButton.Enabled = false;
            this.SpreadsheetCreateButton.Enabled = false;
            this.SpreadsheetDeleteButton.Enabled = false;
        }

        private void ToggleConnectButton()
        {
            if (ConnectButton.Text.Equals("Connect"))
                ConnectButton.Text = "Disconnect";
            else if (ConnectButton.Text.Equals("Disconnect"))
                ConnectButton.Text = "Connect";
        }
    }
}