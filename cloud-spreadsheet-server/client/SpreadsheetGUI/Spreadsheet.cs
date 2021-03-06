﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetGUI
{
    public partial class Spreadsheet : Form
    {
        // The spreadsheet model
        private SS.Spreadsheet spreadsheet;
        // The spreadsheet controller
        private SpreadsheetController controller;
        // The State of the spreadsheet
        private SpreadsheetState state;
        // Boolean for arrow key movement
        private bool MoveMode;
        // Delegate for handling updates to active cell boxes
        private delegate void SpreadsheetUpdateActiveCellTextBoxes();
        // Delegate for updating the spreadsheet's contents (and therefore value)
        private delegate void SpreadsheetContentsModifier(string contents, int row, int col);
        // Delegate for saving the spreadsheet
        private delegate void SaveDialog();
        // Delegate for loading a new spreadsheet from file
        private delegate void LoadDialog();
        // Delegate for opening a new, empty spreadsheet in a new window
        private delegate void NewSpreadsheetDel();
        // Delegate for changing the color of a cell
        private delegate void ChangeColorDel();

        // Event for updating the spreadsheets active cell box info
        private event SpreadsheetUpdateActiveCellTextBoxes UpdateActiveCellBoxesHandler;
        // Event for changing info in the spreadsheet
        private event SpreadsheetContentsModifier SpreadsheetChangedHandler;
        // Event for saving the spreadsheet
        private event SaveDialog SaveButtonClickHandler;
        // Event for loading a new spreadsheet from file
        private event LoadDialog LoadButtonClickHandler;
        // eEvent for creating a new, empty spreadsheet
        private event NewSpreadsheetDel NewSpreadsheetHandler;
        // Event for changing the color of the cell
        private event ChangeColorDel SetColorButtonClickHandler;

        //Allows for the updating of cells by clicking off of them
        private int[] lastCellCoordinates;
        private int[] currentCellCoordinates;

        /// <summary>
        /// From the client part, connect to server
        /// </summary>
        /// <param name="ipAddress"></param>
        private void ConnectToServer()
        {
            string ipAddress = AddressBox.Text;

            // Pass off handling of connection to server.
            controller.ConnectToServer(ipAddress);

            ConnectButton.Enabled = false;
        }

        private void DisconnectFromServer()
        {
            controller.CloseConnection();
            ConnectButton.Enabled = true;
            ToggleConnectButton();

            //Blank out every cell.
            foreach (string cell in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                int row;
                int col;
                this.GetCellCoords(out col, out row, cell);

                SpreadsheetCellGrid.SetValue(col, row, "");
            }
        }

        private void ToggleConnectButton()
        {
            if (ConnectButton.Text.Equals("Connect"))
                ConnectButton.Text = "Disconnect";
            else if (ConnectButton.Text.Equals("Disconnect"))
                ConnectButton.Text = "Connect";
        }

        private void GotConnection()
        {
            MethodInvoker m = new MethodInvoker(PrintSpreadsheetsToScreen);
            this.Invoke(m);
            MethodInvoker t = new MethodInvoker(() =>
            {
                ConnectButton.Enabled = true;
                ToggleConnectButton();
            });
            this.Invoke(t);
        }

        private void PrintSpreadsheetsToScreen()
        {
            OpenSpreadsheetsWindow sprWindow = new OpenSpreadsheetsWindow(state);

            sprWindow.RegisterCloseHandler(SendOpenRequest);
            sprWindow.Show();
            sprWindow.BringToFront();
        }

        private void SendOpenRequest()
        {
            controller.SendOpenRequest();
        }

        private void ErrorMessageHandler(Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            MethodInvoker m = new MethodInvoker(() => ConnectButton.Enabled = true);
            this.Invoke(m);
        }


        // Add handlers
        /// <summary>
        /// Updates the CurrentCellNameBox to show the name of the currently selected cell
        /// </summary>
        private void UpdateActiveCellBoxes()
        {
            int row;
            int col;
            SpreadsheetCellGrid.GetSelection(out col, out row);
            // Convert cell coordinates into a letter followed by a number between 1-99
            string cell = (char)(col + 65) + "" + (row + 1);
            // Update the active cell text boxes
            CurrentCellNameBox.Text = cell;
            // If the cell value is a formula error, instead made sure that it simply indicates an error
            if (spreadsheet.GetCellValue(cell) is FormulaError)
            {
                CurrentCellValueBox.Text = "#ERROR";
                FormulaError error = (FormulaError)spreadsheet.GetCellValue(cell);
                // Discloses the actual cause of the error next to the value field
                FormulaErrorLabel.Text = "(" + error.Reason + ")";
            }
            else
            {
                // Set the field to the proper value
                CurrentCellValueBox.Text = spreadsheet.GetCellValue(cell).ToString();
                // Reset the FormulaError text
                FormulaErrorLabel.Text = "";
            }
            // Next make sure that, if the contents of the cell is a formula, then the string representation has '=' prepended to it
            if (spreadsheet.GetCellContents(cell) is Formula)
            {
                SetCellContentsBox.Text = "=" + spreadsheet.GetCellContents(cell).ToString();
            }
            else
            {
                SetCellContentsBox.Text = spreadsheet.GetCellContents(cell).ToString();
            }
        }

        /// <summary>
        /// Moves the active cell in the spreadsheet in the specified direction.
        /// </summary>
        /// <param name="e"></param>
        private void MoveActiveCell(KeyEventArgs e)
        {
            // First get the cell coordinates
            int col;
            int row;
            SpreadsheetCellGrid.GetSelection(out col, out row);
            // Adjust the active cell accordingly if we're in move mode:
            if (MoveMode)
            {
                if (e.KeyCode == Keys.Up)
                {
                    SpreadsheetCellGrid.SetSelection(col, --row);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    SpreadsheetCellGrid.SetSelection(col, ++row);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    SpreadsheetCellGrid.SetSelection(++col, row);
                }
                else if (e.KeyCode == Keys.Left)
                {
                    SpreadsheetCellGrid.SetSelection(--col, row);
                }
            }
        }

        private void HandleUpdate()
        {
            MethodInvoker m = new MethodInvoker(ImplementChanges);
            this.Invoke(m);
        }

        /// <summary>
        /// After receiving an update from the server, this method will implement the changes to the spreadsheet.
        /// This method does not check for circular dependencies and assumes all changes are valid as the server has accepted them.
        /// </summary>
        private void ImplementChanges()
        {
            // Get the spreadsheet and display it
            lock (state)
            {
                Dictionary<string, string> newSpreadsheet = state.RecentChanges;
                foreach (string nonemptyCell in newSpreadsheet.Keys)
                {
                    int row;
                    int col;
                    GetCellCoords(out col, out row, nonemptyCell);
                    spreadsheet.SetContentsOfCell(nonemptyCell, newSpreadsheet[nonemptyCell]);
                    SpreadsheetCellGrid.SetValue(col, row, spreadsheet.GetCellValue(nonemptyCell).ToString());
                }
            }
            SetCellContentsBox.Focus();
        }

        /// <summary>
        /// Handles the modification of spreadsheet cells in the form. Takes in only one argument, which is the contents of the cell
        /// </summary>
        /// <param name="contents"></param>
        public bool SetSpreadsheetCellContents(string contents)
        {
            // Get the cell coordinates
            int row;
            int col;

            SpreadsheetCellGrid.GetSelection(out col, out row);

            // Get the cell name (should be the active cell)
            string cell = CurrentCellNameBox.Text;
            bool noErrors = true;

            // Set the contents and then upate the active cell info
            ISet<String> affectedCells;
            try
            {
                affectedCells = spreadsheet.SetContentsOfCell(cell, contents);
            }
            catch (Exception e)
            {
                // Create an error dialog explaining the issue
                string ErrorDialogCaption = "Invalid Input";
                string MessageboxText = "There was a problem handling your spreadsheet input.\n(" + e.Message + ")\n\nNote: no changes were made to the spreadsheet.";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;
                MessageBox.Show(MessageboxText, ErrorDialogCaption, button, icon);
                return false;
            }

            // update the cell itself
            if (spreadsheet.GetCellValue(cell) is FormulaError)
            {
                // If the value is an error, express it and update the formula error label
                CurrentCellValueBox.Text = "#ERROR";
                SpreadsheetCellGrid.SetValue(col, row, "#ERROR");
                FormulaError error = (FormulaError)spreadsheet.GetCellValue(cell);
                FormulaErrorLabel.Text = "(" + error.Reason + ")";
            }
            else
            {
                // Otherwise just display the proper value and empty the FormulaErrorLabel
                CurrentCellValueBox.Text = spreadsheet.GetCellValue(cell).ToString();
                SpreadsheetCellGrid.SetValue(col, row, spreadsheet.GetCellValue(cell).ToString());
                FormulaErrorLabel.Text = "";
            }

            // update the dependencies
            if(noErrors)
                noErrors = UpdateDependents(cell, affectedCells);

            return noErrors;
        }

        /// <summary>
        /// Load a spreadsheet from file, replacing the current spreadsheet.
        /// </summary>
        private void LoadSpreadsheet()
        {
            // give user message if spreadsheet is not saved
            if (spreadsheet.Changed)
            {
                bool shouldCancel = SaveWarning();
                if (shouldCancel)   // true if the user wants to cancel the load operation
                {
                    return;
                }
            }

            // displays a OpenFileDialog for loading a spreadsheet from file
            OpenFileDialog openSpreadsheet = new OpenFileDialog();

            // Filter for what files are displayed, defaults to '.sprd' extension but can select all files
            openSpreadsheet.Filter = "Spreadsheet File (.sprd)|*.sprd|All Files|*.*";
            openSpreadsheet.Title = "Open Spreadsheet";
            openSpreadsheet.ShowDialog();

            // If the file name is not an empty string, then create a new Form class with the file path
            // this will open a new spreadsheet window
            if (openSpreadsheet.FileName != "")
            {
                Spreadsheet newSpreadsheetForm = new Spreadsheet(openSpreadsheet.FileName);
                //spreadsheet = new SS.Spreadsheet(openSpreadsheet.FileName, s => Regex.IsMatch(s, "^[A-Z][0-9]{1,2}$"), s => s.ToUpper(), "ps6");
                newSpreadsheetForm.Show();
                newSpreadsheetForm.UpdateAllNonemptyCells();

                // this 'closes' the current form
                // should just replace the panels in the current form, but time constraints dont allow
                //newSpreadsheetForm.Closed += (s, args) => this.Close();
                //this.Hide();
            }
        }

        private void SetCellContentsOnClick(SpreadsheetPanel ss)
        {
            // Get the cell coordinates
            int row;
            int col;
            SpreadsheetCellGrid.GetSelection(out col, out row);

            if (row != lastCellCoordinates[0] || col != lastCellCoordinates[1])
            {
                string contents;

                string name = GetCellName(lastCellCoordinates[1], lastCellCoordinates[0]);

                contents = spreadsheet.GetCellValue(name).ToString();
                
                SpreadsheetCellGrid.SetValue(lastCellCoordinates[1], lastCellCoordinates[0], contents);
            }

            lastCellCoordinates[0] = row;
            lastCellCoordinates[1] = col;

            SetCellContentsBox.Focus();
        }

        /// <summary>
        /// Open a dialog to save the spreadsheet in the current form
        /// </summary>
        private void SaveSpreadsheet()
        {
            // display a SaveDialog for saving the spreadsheet to file
            SaveFileDialog saveSpreadsheetDialog = new SaveFileDialog();
            // Filter for what files are displayed, defaults to '.sprd' extension but can select all files
            saveSpreadsheetDialog.Filter = "Spreadsheet File (.sprd)|*.sprd|All Files|*.*";
            saveSpreadsheetDialog.Title = "Save Spreadsheet";
            saveSpreadsheetDialog.ShowDialog();

            // If the file name is not an empty string, then save it with the spreadsheet class
            String filePath = saveSpreadsheetDialog.FileName;
            if (filePath != "")
            {
                spreadsheet.Save(saveSpreadsheetDialog.FileName);
            }
        }

        /// <summary>
        /// Load new, empty spreadsheet in new window, keeping the current spreadsheet open.
        /// </summary>
        private void NewSpreadsheet()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet();
            newSpreadsheet.Show();
        }

        /// <summary>
        /// Open a dialog for setting the color of the currently selected cell.
        /// </summary>
        private void ChangeColor()
        {
            // get a color from the user
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            Color selectedColor = colorDialog.Color;

            // get the currently selected cell
            int row;
            int col;
            SpreadsheetCellGrid.GetSelection(out col, out row);
            String cellName = GetCellName(col, row);

            // set the color of the selected cell in the spreadsheet
            bool colorChanged = spreadsheet.SetCellColor(cellName, selectedColor);
            // returns false if cell that color was changed is empty
            if (!colorChanged)
            {
                MessageBox.Show("Cannot currently set the color of an empty cell.\nPlease try to set the color of a cell with a value.");
                return;
            }

            // draw the color on the GUI
            SpreadsheetCellGrid.SetColor(col, row, selectedColor);
        }

        /// <summary>
        /// Go through all nonempty cells in the spreadsheet and update each cell GUI object to reflect their value.
        /// 
        /// <remarks>
        /// This method is meant to be called when a new spreadsheet is created from file. All the cell values are read
        /// from file into the spreadsheet class, but it is still necessary to take the cell values in the spreadsheet
        /// and update the cell objects to reflect their value.
        /// </remarks>
        /// </summary>
        public void UpdateAllNonemptyCells()
        {
            // iterate over all nonempty cells in spreadsheet class
            foreach (String nonEmptyCellName in this.spreadsheet.GetNamesOfAllNonemptyCells())
            {
                // get the cell information
                int cellCol, cellRow;
                GetCellCoords(out cellCol, out cellRow, nonEmptyCellName);
                object cellValue = spreadsheet.GetCellValue(nonEmptyCellName);

                // update the GUI
                String cellText;
                if (cellValue is FormulaError)
                {
                    // If the value is an error, express it and update the formula error label
                    cellText = "#ERROR";
                    FormulaError error = (FormulaError)cellValue;
                    FormulaErrorLabel.Text = "(" + error.Reason + ")";
                }
                else
                {
                    // Otherwise just display the proper value and empty the FormulaErrorLabel
                    cellText = cellValue.ToString();
                    FormulaErrorLabel.Text = "";
                }
                SpreadsheetCellGrid.SetValue(cellCol, cellRow, cellText);
            }
        }

        /// <summary>
        /// Basic constructor for the spreadsheet window. Registers the needed events, and initializes the spreadsheet model.
        /// </summary>
        ///
        public Spreadsheet()
        {
            InitializeComponent();
            AddressBox.Focus();

            // Regex matches A-Z followed by 1 or 2 digits
            spreadsheet = new SS.Spreadsheet(s => Regex.IsMatch(s, "^[A-Z][0-9]{1,2}$"), s => s.ToUpper(), "ps6");
            state = new SpreadsheetState();
            controller = new SpreadsheetController(state, spreadsheet);

            UpdateActiveCellBoxesHandler += UpdateActiveCellBoxes;
            //SpreadsheetChangedHandler += SetSpreadsheetCellContents;
            SaveButtonClickHandler += SaveSpreadsheet;
            LoadButtonClickHandler += LoadSpreadsheet;
            NewSpreadsheetHandler += NewSpreadsheet;
            SetColorButtonClickHandler += ChangeColor;
            SpreadsheetCellGrid.SelectionChanged += SetCellContentsOnClick;

            controller.RegisterErrorHandler(ErrorMessageHandler);
            controller.RegisterConnectionHandler(GotConnection);
            controller.RegisterUpdateHandler(HandleUpdate);
            controller.RegisterFirstUpdateHandler(EnableEditing);

            AddressBox.Text = "lab2-24.eng.utah.edu";
            lastCellCoordinates = new int[] { 0, 0 };
            currentCellCoordinates = new int[] { 0, 0 };
            SetCellContentsBox.Enabled = false;
        }

        private void EnableEditing()
        {
            MethodInvoker m = new MethodInvoker(() =>
            {                
                SetCellContentsBox.Enabled = true;
                SetCellContentsBox.Focus();
            });

            this.Invoke(m);            
        }

        /// <summary>
        /// Create a new form with a spreadsheetFilePath.
        /// 
        /// <remarks>
        /// The spreadsheet file path is a path to a file that the constructor will atttempt to open
        /// and create a new spreadsheet with
        /// </remarks>
        /// </summary>
        /// <param name="spreadsheetFilePath">Spreadsheet file to open and load</param>
        public Spreadsheet(String spreadsheetFilePath) : this()
        {
            // this constructor calls the zero-argument constructor (': this()' at end of method declaration line)
            // and then overwrites the spreadsheet member with the new spreadsheet, which loads the file path
            Func<String, bool> validator = s => Regex.IsMatch(s, "^[A-Z][0-9]{1,2}$");
            Func<String, String> normalizer = s => s.ToUpper();
            String version = "ps6";

            this.spreadsheet = new SS.Spreadsheet(spreadsheetFilePath, validator, normalizer, version);
        }

        // connected to 'save' (floppy disk icon) button press
        private void SaveButton_Click(object sender, EventArgs e)
        {   /*
            SaveButtonClickHandler();
            */
        }

        // connected to 'load' (file icon) button press
        private void LoadButton_Click(object sender, EventArgs e)
        {
            /*
            LoadButtonClickHandler();
            */
        }

        // connected to 'new' (plus icon) button press
        private void NewButton_Click(object sender, EventArgs e)
        {
            NewSpreadsheetHandler();
        }

        // connected to 'set color' button
        private void SetColorButton_Click(object sender, EventArgs e)
        {
            SetColorButtonClickHandler();
        }

        /// <summary>
        /// Opens the help menu dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            using (HelpMenu help = new HelpMenu())
            {
                help.ShowDialog();
            }
        }


        /// <summary>
        /// Updates the text boxes at the top of the window at startup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetCellGrid_Load(object sender, EventArgs e)
        {
            UpdateActiveCellBoxesHandler();
        }

        /// <summary>
        /// Updates the text boxes at the top of the window whenever a new spreadsheet cell is selected
        /// </summary>
        /// <param name="sender"></param>
        private void SpreadsheetCellGrid_SelectionChanged(SpreadsheetPanel sender)
        {
            UpdateActiveCellBoxesHandler();
        }

        /// <summary>
        /// If updates are legal for the server, send edit request to server. 
        /// </summary>
        private void CheckInputBeforeSending()
        {
            // Get the cell coordinates
            int row;
            int col;
            SpreadsheetCellGrid.GetSelection(out col, out row);

            bool validInput = true;
            string name = GetCellName(col, row);
            string oldContent = spreadsheet.GetCellContents(name).ToString();
            try
            {
                spreadsheet.SetContentsOfCell(name, SetCellContentsBox.Text);
            }
            catch (Exception error)
            {
                this.ErrorMessageHandler(error);
                validInput = false;
            }
            if (spreadsheet.GetCellValue(name) is FormulaError)
                validInput = false;
            IEnumerable<string> dependents = spreadsheet.SetContentsOfCell(name, oldContent);

            if (validInput)
                controller.SendEditRequest(GetCellName(col, row), SetCellContentsBox.Text, dependents.ToList());

            if (!validInput)
                MessageBox.Show("Please enter a valid input.");

            //Move to next cell like in excel
            /*
            SetCellContentsBox.Clear();
            SpreadsheetCellGrid.SetValue(col, row, oldContent);
            SpreadsheetCellGrid.SetSelection(col, row + 1);
            */
        }

        /// <summary>
        /// Handles updates to the contents of the spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCellContentsButton_Click(object sender, EventArgs e)
        {
            if (state.Connected)
            {
                CheckInputBeforeSending();
                SetCellContentsBox.Focus();
            }
            else
            {
                //SpreadsheetChangedHandler(SetCellContentsBox.Text, row, col);
            }
        }

        /// <summary>
        /// Allows the enter key to update cell contents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCellContentsBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (state.Connected)
                {
                    CheckInputBeforeSending();
                    SetCellContentsBox.Focus();
                }
                else
                {
                    //SpreadsheetChangedHandler(SetCellContentsBox.Text, row, col);
                    //e.SuppressKeyPress = true;
                }
            }
        }


        /// <summary>
        /// Handles keydown operations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            // If shift is being pressed down, enter into move mode
            if (e.KeyCode == Keys.ShiftKey)
            {
                MoveMode = true;
                SetCellContentsBox.Focus();
            }
            // If an arrow key is pressed, handle cell movement
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                // The check to see if movemode is true is handled in the method
                MoveActiveCell(e);
                UpdateActiveCellBoxesHandler();
            }
            */
        }

        /// <summary>
        /// Handles keyup operations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // If shift is released, exit movemode 
            if (e.KeyCode == Keys.ShiftKey)
            {
                MoveMode = false;
            }
        }

        private void Spreadsheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = SaveWarning();
            }
        }

        /// <summary>
        /// Event listener for entering in data to the cell through the EntryBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryBox_TextChanged(object sender, EventArgs e)
        {
            if (state.Connected)
            {
                ShowTextEntry();
            }
        }

        /// <summary>
        /// Private helper method to show the text that is being entered in the EntryBox within the cell selected.
        /// </summary>
        private void ShowTextEntry()
        {
            int col;
            int row;

            SpreadsheetCellGrid.GetSelection(out col, out row);

            SpreadsheetCellGrid.SetValue(col, row, SetCellContentsBox.Text);
        }

        // === Helper methods ===

        /// <summary>
        /// Convert a cell name into a row and col.
        /// 
        /// <remarks>
        /// This is meant to be a helper method to the UpdateAllNonemptyCells method. It determines
        /// rows and cols of cell names when it is assigning cell values from names.
        /// </remarks>
        /// </summary>
        /// <param name="cellCol">Variable to assign calculated column to</param>
        /// <param name="cellRow">Variable to assign calculated row to</param>
        /// <param name="cellName">Name of cell to use in calculations</param>
        private void GetCellCoords(out int cellCol, out int cellRow, String cellName)
        {
            Char cellNameLetter = cellName[0];              // first char of cell name
            String cellNameDigits = cellName.Substring(1);  // second char onward of cell name

            // convert to ascii and count positions from 'A'
            //   note lowercase letters need not be accounted for as the normalizer converts lower to uppercase
            //   note col numbering starts at zero
            cellCol = (int)cellNameLetter - 65;
            // for row, simply convert string of digits to an integer and subtract 1 (to be zero-based)
            cellRow = Convert.ToInt32(cellNameDigits) - 1;
        }

        /// <summary>
        /// Get the name of the cell with a row and column.
        /// 
        /// Example: (1, 1) -> A1
        /// Cell column and rows are zero-based.
        /// </summary>
        /// <param name="cellCol">Col to use</param>
        /// <param name="cellRow">Row to use</param>
        /// <returns></returns>
        private String GetCellName(int cellCol, int cellRow)
        {
            // calculate letter portion
            // add 65 to column number to get ascii value of cell letter (capital) such that col 1 -> A, col 26 -> Z
            Char cellLetterChar = (char)(cellCol + 65);
            String cellLetter = cellLetterChar.ToString();
            // number portion is just string of row
            String cellNumber = (cellRow + 1).ToString();

            return cellLetter + cellNumber;
        }

        /// <summary>
        /// Warn the user if the spreadsheet is not saved with an option to save or discard it or cancel the operation
        /// 
        /// If the user wants to save, it will be saved here and return false. If the user wants to discard it or cancel the operation,
        /// then the method will return false or true respectively.
        /// <remarks>
        /// This is a helper method for use when a spreadsheet is about to be discarded.
        /// </remarks>
        /// </summary>
        /// <returns>true if operation should be cancelled, false if it should continue</returns>
        private bool SaveWarning()
        {   /*
            if (spreadsheet.Changed)
            {
                DialogResult result = MessageBox.Show("There are unsaved changes, do you want to save?", "Confirmation", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    this.SaveSpreadsheet();
                    return false;
                }
                else if (result == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            */
            return false;
        }
        
        /// <summary>
        /// Recalculate the values of a cell's dependents, indirect and direct
        /// </summary>
        /// <param name="cellName">name of cell directly modified</param>
        /// <param name="depndentCellNames">Set of dependenet cells, returned by SetContentsOfCell method</param>
        private bool UpdateDependents(String cellName, ISet<String> dependentCellNames)
        {
            bool noErrors = true;
            foreach(String dependentCellName in dependentCellNames)
            {
                object dependentCellValue = spreadsheet.GetCellValue(dependentCellName);
                // calculate row and col of cell
                int cellRow, cellCol;
                GetCellCoords(out cellCol, out cellRow, dependentCellName);

                if (dependentCellValue is FormulaError)
                {
                    // If the value is an error, express it and update the formula error label
                    CurrentCellValueBox.Text = "#ERROR";
                    SpreadsheetCellGrid.SetValue(cellCol, cellRow, "#ERROR");
                    FormulaError error = (FormulaError)dependentCellValue;
                    FormulaErrorLabel.Text = "(" + error.Reason + ")";
                    noErrors = false;
                }
                else
                {
                    // Otherwise just display the proper value and empty the FormulaErrorLabel
                    CurrentCellValueBox.Text = dependentCellValue.ToString();
                    SpreadsheetCellGrid.SetValue(cellCol, cellRow, dependentCellValue.ToString());
                    FormulaErrorLabel.Text = "";
                }
            }
            return noErrors;
        }

        // Handle user clicking the connect/disconnect button
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (ConnectButton.Text.Equals("Connect") && state.Connected == false)
                ConnectToServer();
            else if (ConnectButton.Text.Equals("Disconnect") && state.Connected == true)
                DisconnectFromServer();
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            if (state.Connected)
                controller.SendUndoRequest();
            SetCellContentsBox.Focus();
        }

        private void Revert_Click(object sender, EventArgs e)
        {
            if (state.Connected)
            {
                // Get the cell coordinates
                int row;
                int col;
                SpreadsheetCellGrid.GetSelection(out col, out row);
                string name = GetCellName(col, row);
                controller.SendRevertRequest(name);
            }
            SetCellContentsBox.Clear();
            SetCellContentsBox.Focus();
        }
    }
}
