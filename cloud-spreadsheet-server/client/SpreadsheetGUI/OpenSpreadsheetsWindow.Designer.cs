namespace SpreadsheetGUI
{
    partial class OpenSpreadsheetsWindow
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
            this.OpenButton = new System.Windows.Forms.Button();
            this.AlternateEntryBox = new System.Windows.Forms.TextBox();
            this.SpreadsheetListBox = new System.Windows.Forms.ListBox();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.ListLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(552, 342);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(134, 46);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // AlternateEntryBox
            // 
            this.AlternateEntryBox.Location = new System.Drawing.Point(106, 267);
            this.AlternateEntryBox.Name = "AlternateEntryBox";
            this.AlternateEntryBox.Size = new System.Drawing.Size(580, 31);
            this.AlternateEntryBox.TabIndex = 3;
            // 
            // SpreadsheetListBox
            // 
            this.SpreadsheetListBox.FormattingEnabled = true;
            this.SpreadsheetListBox.ItemHeight = 25;
            this.SpreadsheetListBox.Location = new System.Drawing.Point(106, 44);
            this.SpreadsheetListBox.Name = "SpreadsheetListBox";
            this.SpreadsheetListBox.Size = new System.Drawing.Size(580, 179);
            this.SpreadsheetListBox.TabIndex = 4;
            this.SpreadsheetListBox.SelectedIndexChanged += new System.EventHandler(this.SpreadsheetListBox_SelectedIndexChanged);
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(222, 322);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(306, 31);
            this.UsernameBox.TabIndex = 5;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(222, 373);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(306, 31);
            this.PasswordBox.TabIndex = 6;
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(106, 325);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(110, 25);
            this.UsernameLabel.TabIndex = 7;
            this.UsernameLabel.Text = "Username";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(106, 373);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(106, 25);
            this.PasswordLabel.TabIndex = 8;
            this.PasswordLabel.Text = "Password";
            // 
            // ListLabel
            // 
            this.ListLabel.AutoSize = true;
            this.ListLabel.Location = new System.Drawing.Point(106, 13);
            this.ListLabel.Name = "ListLabel";
            this.ListLabel.Size = new System.Drawing.Size(593, 25);
            this.ListLabel.TabIndex = 9;
            this.ListLabel.Text = "Choose one of the following spreadsheets from the list below";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(566, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "Or enter the name of the spreadsheet you want in this box";
            // 
            // OpenSpreadsheetsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ListLabel);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.SpreadsheetListBox);
            this.Controls.Add(this.AlternateEntryBox);
            this.Controls.Add(this.OpenButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(826, 521);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(826, 521);
            this.Name = "OpenSpreadsheetsWindow";
            this.Text = "OpenSpreadsheetsWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.TextBox AlternateEntryBox;
        private System.Windows.Forms.ListBox SpreadsheetListBox;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label ListLabel;
        private System.Windows.Forms.Label label1;
    }
}