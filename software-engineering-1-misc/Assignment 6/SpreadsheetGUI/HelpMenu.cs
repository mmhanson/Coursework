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
    public partial class HelpMenu : Form
    {
        public HelpMenu()
        {
            InitializeComponent();
        }

        private void CloseHelpMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HelpInfoBox.ScrollBars.Equals(ScrollBars.Vertical) && HelpItems.SelectedIndex != 6)
            {
                HelpInfoBox.ScrollBars = ScrollBars.None;
            }
            switch (HelpItems.SelectedIndex)
            {
                case 0:
                    HelpInfoBox.Text = "To save, simply click on the floppy disc icon. Choose your desired filename and save location and click \"Save\". \n\nYou will be prompted if you want to overwrite the existing file, if your file already exists. Select yes to overwrite or no to cancel the save.";
                    break;
                case 1:
                    HelpInfoBox.Text = "To load or open an existing spreadsheet, click the folder icon on the top of the page. Locate the desired .sprd file and click \"Open\". The spreadsheet will open, replacing the current spreadsheet. You may be prompted to save your changes, select yes to save or no to discard or cancel to stop the load operation.";
                    break;
                case 2:
                    HelpInfoBox.Text = "To open a new spreadsheet, click on the plus-shaped icon at the top left of the window. It will open an empty spreadsheet in a new window.";
                    break;
                case 3:
                    HelpInfoBox.Text = "There are two ways to select a different cell. The first is simply by clicking on it. You can also hold down shift and use the arrow keys to change the selected cell.";
                    break;
                case 4:
                    HelpInfoBox.Text = "To edit a cell, place the desired cell content into the text box directly above the cell grid and click the \"Set\" button or press enter.";
                    break;
                case 5:
                    HelpInfoBox.Text = "To create a formula that you want the spreadsheet to evaluate, simply place a '=' symbol before your expression in the cell content bar.";
                    break;
                case 6:
                    HelpInfoBox.ScrollBars = ScrollBars.Vertical;
                    HelpInfoBox.Text = "There are a variety of reasons a formula failed to evaluate. If the formula syntax is directly invalid, a popup will be created explaining why. If the formula is syntactically valid but fails to evaluate, the cell value will display as \"#ERROR\". To determine what went wrong, simply select the cell with the error and an explanation will be displayed by the cell value box.";
                    break;
                case 7:
                    HelpInfoBox.Text = "To set the color of a cell, select it and press the 'Set Color' button. You will select a color and press 'ok', then the cell will adopt that color!\n Note that currently you can only set the color of a nonempty cell.";
                    break;
                default:
                    HelpInfoBox.Text = "Select an item on the left to learn more about it";
                    break;
            }
        }

        private void HelpMenu_Load(object sender, EventArgs e)
        {
            HelpInfoBox.Text = "Select an item on the left to learn more about it";
        }
    }
}
