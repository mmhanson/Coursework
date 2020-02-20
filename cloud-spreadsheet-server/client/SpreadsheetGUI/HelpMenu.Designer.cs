namespace SpreadsheetGUI
{
    partial class HelpMenu
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
            this.HelpItems = new System.Windows.Forms.ListBox();
            this.CloseHelpMenu = new System.Windows.Forms.Button();
            this.HelpInfo = new System.Windows.Forms.Label();
            this.HelpInfoBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // HelpItems
            // 
            this.HelpItems.FormattingEnabled = true;
            this.HelpItems.ItemHeight = 31;
            this.HelpItems.Items.AddRange(new object[] {
            "Saving",
            "Loading",
            "New Spreadsheet",
            "Cell Selection",
            "Cell Editiing",
            "Formulas",
            "Errors",
            "Colors"});
            this.HelpItems.Location = new System.Drawing.Point(35, 31);
            this.HelpItems.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.HelpItems.Name = "HelpItems";
            this.HelpItems.Size = new System.Drawing.Size(313, 438);
            this.HelpItems.TabIndex = 0;
            this.HelpItems.SelectedIndexChanged += new System.EventHandler(this.HelpItems_SelectedIndexChanged);
            // 
            // CloseHelpMenu
            // 
            this.CloseHelpMenu.Location = new System.Drawing.Point(624, 420);
            this.CloseHelpMenu.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CloseHelpMenu.Name = "CloseHelpMenu";
            this.CloseHelpMenu.Size = new System.Drawing.Size(200, 55);
            this.CloseHelpMenu.TabIndex = 1;
            this.CloseHelpMenu.Text = "Close";
            this.CloseHelpMenu.UseVisualStyleBackColor = true;
            this.CloseHelpMenu.Click += new System.EventHandler(this.CloseHelpMenu_Click);
            // 
            // HelpInfo
            // 
            this.HelpInfo.AutoSize = true;
            this.HelpInfo.Location = new System.Drawing.Point(373, 31);
            this.HelpInfo.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.HelpInfo.Name = "HelpInfo";
            this.HelpInfo.Size = new System.Drawing.Size(0, 32);
            this.HelpInfo.TabIndex = 2;
            // 
            // HelpInfoBox
            // 
            this.HelpInfoBox.Location = new System.Drawing.Point(381, 31);
            this.HelpInfoBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.HelpInfoBox.Multiline = true;
            this.HelpInfoBox.Name = "HelpInfoBox";
            this.HelpInfoBox.ReadOnly = true;
            this.HelpInfoBox.Size = new System.Drawing.Size(436, 369);
            this.HelpInfoBox.TabIndex = 3;
            // 
            // HelpMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 503);
            this.Controls.Add(this.HelpInfoBox);
            this.Controls.Add(this.HelpInfo);
            this.Controls.Add(this.CloseHelpMenu);
            this.Controls.Add(this.HelpItems);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "HelpMenu";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.HelpMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox HelpItems;
        private System.Windows.Forms.Button CloseHelpMenu;
        private System.Windows.Forms.Label HelpInfo;
        private System.Windows.Forms.TextBox HelpInfoBox;
    }
}