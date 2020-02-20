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
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.LoadButton = new System.Windows.Forms.ToolStripButton();
            this.NewSpreadsheetButton = new System.Windows.Forms.ToolStripButton();
            this.HelpButton = new System.Windows.Forms.ToolStripButton();
            this.FormulaErrorLabel = new System.Windows.Forms.Label();
            this.SpreadsheetCellGrid = new SS.SpreadsheetPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.SpreadsheetToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetCellContentsBox
            // 
            this.SetCellContentsBox.Location = new System.Drawing.Point(3, 56);
            this.SetCellContentsBox.Name = "SetCellContentsBox";
            this.SetCellContentsBox.Size = new System.Drawing.Size(162, 20);
            this.SetCellContentsBox.TabIndex = 1;
            this.SetCellContentsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetCellContentsBox_KeyDown);
            // 
            // CurrentCellNameBox
            // 
            this.CurrentCellNameBox.Location = new System.Drawing.Point(3, 30);
            this.CurrentCellNameBox.Name = "CurrentCellNameBox";
            this.CurrentCellNameBox.ReadOnly = true;
            this.CurrentCellNameBox.Size = new System.Drawing.Size(40, 20);
            this.CurrentCellNameBox.TabIndex = 2;
            this.CurrentCellNameBox.TabStop = false;
            // 
            // CurrentCellValueBox
            // 
            this.CurrentCellValueBox.Location = new System.Drawing.Point(49, 30);
            this.CurrentCellValueBox.Name = "CurrentCellValueBox";
            this.CurrentCellValueBox.ReadOnly = true;
            this.CurrentCellValueBox.Size = new System.Drawing.Size(116, 20);
            this.CurrentCellValueBox.TabIndex = 3;
            this.CurrentCellValueBox.TabStop = false;
            // 
            // SetCellContentsButton
            // 
            this.SetCellContentsButton.Location = new System.Drawing.Point(171, 54);
            this.SetCellContentsButton.Name = "SetCellContentsButton";
            this.SetCellContentsButton.Size = new System.Drawing.Size(75, 23);
            this.SetCellContentsButton.TabIndex = 4;
            this.SetCellContentsButton.TabStop = false;
            this.SetCellContentsButton.Text = "Set";
            this.SetCellContentsButton.UseVisualStyleBackColor = true;
            this.SetCellContentsButton.Click += new System.EventHandler(this.SetCellContentsButton_Click);
            // 
            // SpreadsheetToolStrip
            // 
            this.SpreadsheetToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.SpreadsheetToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.LoadButton,
            this.NewSpreadsheetButton,
            this.HelpButton});
            this.SpreadsheetToolStrip.Location = new System.Drawing.Point(3, 3);
            this.SpreadsheetToolStrip.Name = "SpreadsheetToolStrip";
            this.SpreadsheetToolStrip.Size = new System.Drawing.Size(104, 25);
            this.SpreadsheetToolStrip.TabIndex = 5;
            this.SpreadsheetToolStrip.Text = "toolStrip1";
            // 
            // SaveButton
            // 
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(23, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LoadButton.Image = ((System.Drawing.Image)(resources.GetObject("LoadButton.Image")));
            this.LoadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(23, 22);
            this.LoadButton.Text = "Load";
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // NewSpreadsheetButton
            // 
            this.NewSpreadsheetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewSpreadsheetButton.Image = ((System.Drawing.Image)(resources.GetObject("NewSpreadsheetButton.Image")));
            this.NewSpreadsheetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewSpreadsheetButton.Name = "NewSpreadsheetButton";
            this.NewSpreadsheetButton.Size = new System.Drawing.Size(23, 22);
            this.NewSpreadsheetButton.Text = "New";
            this.NewSpreadsheetButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HelpButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpButton.Image")));
            this.HelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(23, 22);
            this.HelpButton.Text = "Help";
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // FormulaErrorLabel
            // 
            this.FormulaErrorLabel.AutoSize = true;
            this.FormulaErrorLabel.Location = new System.Drawing.Point(171, 37);
            this.FormulaErrorLabel.Name = "FormulaErrorLabel";
            this.FormulaErrorLabel.Size = new System.Drawing.Size(0, 13);
            this.FormulaErrorLabel.TabIndex = 6;
            // 
            // SpreadsheetCellGrid
            // 
            this.SpreadsheetCellGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SpreadsheetCellGrid.BackColor = System.Drawing.SystemColors.Control;
            this.SpreadsheetCellGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.SpreadsheetCellGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpreadsheetCellGrid.Location = new System.Drawing.Point(0, 80);
            this.SpreadsheetCellGrid.Margin = new System.Windows.Forms.Padding(1);
            this.SpreadsheetCellGrid.Name = "SpreadsheetCellGrid";
            this.SpreadsheetCellGrid.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.SpreadsheetCellGrid.Size = new System.Drawing.Size(722, 365);
            this.SpreadsheetCellGrid.TabIndex = 0;
            this.SpreadsheetCellGrid.TabStop = false;
            this.SpreadsheetCellGrid.SelectionChanged += new SS.SelectionChangedHandler(this.SpreadsheetCellGrid_SelectionChanged);
            this.SpreadsheetCellGrid.Load += new System.EventHandler(this.SpreadsheetCellGrid_Load);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 54);
            this.button1.Margin = new System.Windows.Forms.Padding(1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.TabStop = false;
            this.button1.Text = "Set Color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SetColorButton_Click);
            // 
            // Spreadsheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(722, 445);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.FormulaErrorLabel);
            this.Controls.Add(this.SpreadsheetToolStrip);
            this.Controls.Add(this.SetCellContentsButton);
            this.Controls.Add(this.CurrentCellValueBox);
            this.Controls.Add(this.CurrentCellNameBox);
            this.Controls.Add(this.SetCellContentsBox);
            this.Controls.Add(this.SpreadsheetCellGrid);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Spreadsheet";
            this.Padding = new System.Windows.Forms.Padding(0, 80, 0, 0);
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
        private System.Windows.Forms.ToolStripButton SaveButton;
        private System.Windows.Forms.ToolStripButton LoadButton;
        private System.Windows.Forms.ToolStripButton NewSpreadsheetButton;
        private System.Windows.Forms.Label FormulaErrorLabel;
        private System.Windows.Forms.ToolStripButton HelpButton;
        private System.Windows.Forms.Button button1;
    }
}

