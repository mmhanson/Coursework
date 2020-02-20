using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class OpenSpreadsheetsWindow : Form
    {
        SpreadsheetState state;
        string spreadsheetNameSelected = "";

        public delegate void CallUponClose();
        private event CallUponClose CloseHandler;

        public OpenSpreadsheetsWindow(SpreadsheetState state)
        {
            InitializeComponent();
            this.state = state;

            foreach (string spreadsheet in state.AvailableSpreadsheets)
            {
                SpreadsheetListBox.Items.Add(spreadsheet);
            }
        }

        /// <summary>
        /// Registers callback methods for the event of the form closing with valid input.
        /// </summary>
        /// <param name="toRegister"></param>
        public void RegisterCloseHandler(CallUponClose toRegister)
        {
            CloseHandler += toRegister;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (!UsernameBox.Text.Equals("") && !PasswordBox.Text.Equals(""))
            {
                if (ValidName(AlternateEntryBox.Text))
                {
                    state.Username = UsernameBox.Text;
                    state.Password = PasswordBox.Text;
                    spreadsheetNameSelected = AlternateEntryBox.Text;
                    state.SelectedSpreadsheet = spreadsheetNameSelected;
                    CloseHandler();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter spreadsheet name that does not include characters such as \"#\" or \"~\".");
                }
            }
            else
            {
                MessageBox.Show("Please enter a non-empty username and password");
            }
        }

        private bool ValidName(string name)
        {
            bool valid = true;
            if (name[0].Equals('#') || name[0].Equals("~"))
                valid = false;
            if (name[name.Length - 1].Equals('#') || name[name.Length - 1].Equals("~"))
                valid = false;
            return valid;
        }

        private void SpreadsheetListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpreadsheetListBox.SelectedItem != null)
            {
                AlternateEntryBox.Text = SpreadsheetListBox.SelectedItem.ToString();
                AlternateEntryBox.Enabled = false;
                spreadsheetNameSelected = SpreadsheetListBox.SelectedItem.ToString();
            }
        }
    }
}
