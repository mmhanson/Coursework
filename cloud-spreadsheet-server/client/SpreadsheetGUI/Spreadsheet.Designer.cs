namespace SpreadsheetGUI
{
    partial class Spreadsheet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spreadsheet));
            this.SetCellContentsBox = new System.Windows.Forms.TextBox();
            this.CurrentCellNameBox = new System.Windows.Forms.TextBox();
            this.CurrentCellValueBox = new System.Windows.Forms.TextBox();
            this.SetCellContentsButton = new System.Windows.Forms.Button();
            this.SpreadsheetToolStrip = new System.Windows.Forms.ToolStrip();
            this.NewSpreadsheetButton = new System.Windows.Forms.ToolStripButton();
            this.HelpButton = new System.Windows.Forms.ToolStripButton();
            this.FormulaErrorLabel = new System.Windows.Forms.Label();
            this.SpreadsheetCellGrid = new SS.SpreadsheetPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.Undo = new System.Windows.Forms.Button();
            this.Revert = new System.Windows.Forms.Button();
            this.AddressBox = new System.Windows.Forms.TextBox();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.SpreadsheetToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetCellContentsBox
            // 
            this.SetCellContentsBox.Location = new System.Drawing.Point(5, 108);
            this.SetCellContentsBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.SetCellContentsBox.Name = "SetCellContentsBox";
            this.SetCellContentsBox.Size = new System.Drawing.Size(320, 31);
            this.SetCellContentsBox.TabIndex = 1;
            this.SetCellContentsBox.TextChanged += new System.EventHandler(this.EntryBox_TextChanged);
            this.SetCellContentsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetCellContentsBox_KeyDown);
            // 
            // CurrentCellNameBox
            // 
            this.CurrentCellNameBox.Enabled = false;
            this.CurrentCellNameBox.Location = new System.Drawing.Point(5, 58);
            this.CurrentCellNameBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CurrentCellNameBox.Name = "CurrentCellNameBox";
            this.CurrentCellNameBox.ReadOnly = true;
            this.CurrentCellNameBox.Size = new System.Drawing.Size(76, 31);
            this.CurrentCellNameBox.TabIndex = 2;
            this.CurrentCellNameBox.TabStop = false;
            // 
            // CurrentCellValueBox
            // 
            this.CurrentCellValueBox.Enabled = false;
            this.CurrentCellValueBox.Location = new System.Drawing.Point(97, 58);
            this.CurrentCellValueBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CurrentCellValueBox.Name = "CurrentCellValueBox";
            this.CurrentCellValueBox.ReadOnly = true;
            this.CurrentCellValueBox.Size = new System.Drawing.Size(228, 31);
            this.CurrentCellValueBox.TabIndex = 3;
            this.CurrentCellValueBox.TabStop = false;
            // 
            // SetCellContentsButton
            // 
            this.SetCellContentsButton.Location = new System.Drawing.Point(341, 104);
            this.SetCellContentsButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.SetCellContentsButton.Name = "SetCellContentsButton";
            this.SetCellContentsButton.Size = new System.Drawing.Size(149, 44);
            this.SetCellContentsButton.TabIndex = 4;
            this.SetCellContentsButton.TabStop = false;
            this.SetCellContentsButton.Text = "Set";
            this.SetCellContentsButton.UseVisualStyleBackColor = true;
            this.SetCellContentsButton.Click += new System.EventHandler(this.SetCellContentsButton_Click);
            // 
            // SpreadsheetToolStrip
            // 
            this.SpreadsheetToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.SpreadsheetToolStrip.ImageScalingSize = new System.Drawing.Size(24, 18);
            this.SpreadsheetToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewSpreadsheetButton,
            this.HelpButton});
            this.SpreadsheetToolStrip.Location = new System.Drawing.Point(5, 6);
            this.SpreadsheetToolStrip.Name = "SpreadsheetToolStrip";
            this.SpreadsheetToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.SpreadsheetToolStrip.Size = new System.Drawing.Size(70, 25);
            this.SpreadsheetToolStrip.TabIndex = 5;
            this.SpreadsheetToolStrip.Text = "toolStrip1";
            // 
            // NewSpreadsheetButton
            // 
            this.NewSpreadsheetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewSpreadsheetButton.Image = ((System.Drawing.Image)(resources.GetObject("NewSpreadsheetButton.Image")));
            this.NewSpreadsheetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewSpreadsheetButton.Name = "NewSpreadsheetButton";
            this.NewSpreadsheetButton.Size = new System.Drawing.Size(28, 22);
            this.NewSpreadsheetButton.Text = "New";
            this.NewSpreadsheetButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HelpButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpButton.Image")));
            this.HelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(28, 22);
            this.HelpButton.Text = "Help";
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // FormulaErrorLabel
            // 
            this.FormulaErrorLabel.AutoSize = true;
            this.FormulaErrorLabel.Location = new System.Drawing.Point(341, 71);
            this.FormulaErrorLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.FormulaErrorLabel.Name = "FormulaErrorLabel";
            this.FormulaErrorLabel.Size = new System.Drawing.Size(0, 25);
            this.FormulaErrorLabel.TabIndex = 6;
            // 
            // SpreadsheetCellGrid
            // 
            this.SpreadsheetCellGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SpreadsheetCellGrid.BackColor = System.Drawing.SystemColors.Control;
            this.SpreadsheetCellGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.SpreadsheetCellGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpreadsheetCellGrid.Location = new System.Drawing.Point(0, 154);
            this.SpreadsheetCellGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpreadsheetCellGrid.Name = "SpreadsheetCellGrid";
            this.SpreadsheetCellGrid.Padding = new System.Windows.Forms.Padding(0, 115, 0, 0);
            this.SpreadsheetCellGrid.Size = new System.Drawing.Size(1444, 702);
            this.SpreadsheetCellGrid.TabIndex = 0;
            this.SpreadsheetCellGrid.TabStop = false;
            this.SpreadsheetCellGrid.SelectionChanged += new SS.SelectionChangedHandler(this.SpreadsheetCellGrid_SelectionChanged);
            this.SpreadsheetCellGrid.Load += new System.EventHandler(this.SpreadsheetCellGrid_Load);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(500, 104);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 44);
            this.button1.TabIndex = 7;
            this.button1.TabStop = false;
            this.button1.Text = "Set Color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SetColorButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(1211, 104);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(4);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(144, 45);
            this.ConnectButton.TabIndex = 8;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // Undo
            // 
            this.Undo.Location = new System.Drawing.Point(656, 104);
            this.Undo.Margin = new System.Windows.Forms.Padding(4);
            this.Undo.Name = "Undo";
            this.Undo.Size = new System.Drawing.Size(144, 44);
            this.Undo.TabIndex = 10;
            this.Undo.Text = "Undo";
            this.Undo.UseVisualStyleBackColor = true;
            this.Undo.Click += new System.EventHandler(this.Undo_Click);
            // 
            // Revert
            // 
            this.Revert.Location = new System.Drawing.Point(808, 104);
            this.Revert.Margin = new System.Windows.Forms.Padding(4);
            this.Revert.Name = "Revert";
            this.Revert.Size = new System.Drawing.Size(144, 44);
            this.Revert.TabIndex = 11;
            this.Revert.Text = "Revert";
            this.Revert.UseVisualStyleBackColor = true;
            this.Revert.Click += new System.EventHandler(this.Revert_Click);
            // 
            // AddressBox
            // 
            this.AddressBox.Location = new System.Drawing.Point(959, 107);
            this.AddressBox.Margin = new System.Windows.Forms.Padding(4);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(244, 31);
            this.AddressBox.TabIndex = 9;
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(959, 82);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(166, 25);
            this.AddressLabel.TabIndex = 12;
            this.AddressLabel.Text = "Server Address:";
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1444, 856);
            this.Controls.Add(this.AddressLabel);
            this.Controls.Add(this.Revert);
            this.Controls.Add(this.Undo);
            this.Controls.Add(this.AddressBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.FormulaErrorLabel);
            this.Controls.Add(this.SpreadsheetToolStrip);
            this.Controls.Add(this.SetCellContentsButton);
            this.Controls.Add(this.CurrentCellValueBox);
            this.Controls.Add(this.CurrentCellNameBox);
            this.Controls.Add(this.SetCellContentsBox);
            this.Controls.Add(this.SpreadsheetCellGrid);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Spreadsheet";
            this.Padding = new System.Windows.Forms.Padding(0, 154, 0, 0);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Spreadsheet Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Spreadsheet_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.SpreadsheetToolStrip.ResumeLayout(false);
            this.SpreadsheetToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel SpreadsheetCellGrid;
        private System.Windows.Forms.TextBox SetCellContentsBox;
        private System.Windows.Forms.TextBox CurrentCellNameBox;
        private System.Windows.Forms.TextBox CurrentCellValueBox;
        private System.Windows.Forms.Button SetCellContentsButton;
        private System.Windows.Forms.ToolStrip SpreadsheetToolStrip;
        private System.Windows.Forms.ToolStripButton NewSpreadsheetButton;
        private System.Windows.Forms.Label FormulaErrorLabel;
        private System.Windows.Forms.ToolStripButton HelpButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button Undo;
        private System.Windows.Forms.Button Revert;
        private System.Windows.Forms.TextBox AddressBox;
        private System.Windows.Forms.Label AddressLabel;
    }
}

