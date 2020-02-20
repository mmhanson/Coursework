namespace AdministrativeClient
{
    partial class AdministrativeClientGUI
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
            this.ClientBox = new System.Windows.Forms.RichTextBox();
            this.UpdatesBox = new System.Windows.Forms.RichTextBox();
            this.AddressBox = new System.Windows.Forms.TextBox();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.ClientSideLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.ShutdownButton = new System.Windows.Forms.Button();
            this.AccountManagementBox = new System.Windows.Forms.TextBox();
            this.CreateClientButton = new System.Windows.Forms.Button();
            this.DeleteClientButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.PasswordManagementLabel = new System.Windows.Forms.Label();
            this.ChangePasswordButton = new System.Windows.Forms.Button();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.SpreadsheetNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SpreadsheetCreateButton = new System.Windows.Forms.Button();
            this.SpreadsheetDeleteButton = new System.Windows.Forms.Button();
            this.RecentActionsBox = new System.Windows.Forms.RichTextBox();
            this.RecentActionsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ClientBox
            // 
            this.ClientBox.Location = new System.Drawing.Point(55, 130);
            this.ClientBox.Name = "ClientBox";
            this.ClientBox.Size = new System.Drawing.Size(667, 425);
            this.ClientBox.TabIndex = 0;
            this.ClientBox.Text = "";
            // 
            // UpdatesBox
            // 
            this.UpdatesBox.Location = new System.Drawing.Point(728, 130);
            this.UpdatesBox.Name = "UpdatesBox";
            this.UpdatesBox.Size = new System.Drawing.Size(667, 425);
            this.UpdatesBox.TabIndex = 1;
            this.UpdatesBox.Text = "";
            // 
            // AddressBox
            // 
            this.AddressBox.Location = new System.Drawing.Point(55, 59);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(285, 31);
            this.AddressBox.TabIndex = 2;
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(55, 31);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(160, 25);
            this.AddressLabel.TabIndex = 3;
            this.AddressLabel.Text = "Server Address";
            // 
            // ClientSideLabel
            // 
            this.ClientSideLabel.AutoSize = true;
            this.ClientSideLabel.Location = new System.Drawing.Point(55, 102);
            this.ClientSideLabel.Name = "ClientSideLabel";
            this.ClientSideLabel.Size = new System.Drawing.Size(198, 25);
            this.ClientSideLabel.TabIndex = 4;
            this.ClientSideLabel.Text = "Client Management";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(356, 59);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(157, 40);
            this.ConnectButton.TabIndex = 5;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // UpdateLabel
            // 
            this.UpdateLabel.AutoSize = true;
            this.UpdateLabel.Location = new System.Drawing.Point(723, 102);
            this.UpdateLabel.Name = "UpdateLabel";
            this.UpdateLabel.Size = new System.Drawing.Size(92, 25);
            this.UpdateLabel.TabIndex = 6;
            this.UpdateLabel.Text = "Updates";
            // 
            // ShutdownButton
            // 
            this.ShutdownButton.Location = new System.Drawing.Point(1180, 54);
            this.ShutdownButton.Name = "ShutdownButton";
            this.ShutdownButton.Size = new System.Drawing.Size(215, 40);
            this.ShutdownButton.TabIndex = 7;
            this.ShutdownButton.Text = "Shutdown Server";
            this.ShutdownButton.UseVisualStyleBackColor = true;
            this.ShutdownButton.Click += new System.EventHandler(this.ShutdownButton_Click);
            // 
            // AccountManagementBox
            // 
            this.AccountManagementBox.Location = new System.Drawing.Point(55, 624);
            this.AccountManagementBox.Name = "AccountManagementBox";
            this.AccountManagementBox.Size = new System.Drawing.Size(402, 31);
            this.AccountManagementBox.TabIndex = 8;
            // 
            // CreateClientButton
            // 
            this.CreateClientButton.Location = new System.Drawing.Point(477, 619);
            this.CreateClientButton.Name = "CreateClientButton";
            this.CreateClientButton.Size = new System.Drawing.Size(100, 40);
            this.CreateClientButton.TabIndex = 9;
            this.CreateClientButton.Text = "Create";
            this.CreateClientButton.UseVisualStyleBackColor = true;
            this.CreateClientButton.Click += new System.EventHandler(this.CreateClientButton_Click);
            // 
            // DeleteClientButton
            // 
            this.DeleteClientButton.Location = new System.Drawing.Point(595, 619);
            this.DeleteClientButton.Name = "DeleteClientButton";
            this.DeleteClientButton.Size = new System.Drawing.Size(100, 40);
            this.DeleteClientButton.TabIndex = 10;
            this.DeleteClientButton.Text = "Delete";
            this.DeleteClientButton.UseVisualStyleBackColor = true;
            this.DeleteClientButton.Click += new System.EventHandler(this.DeleteClientButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 596);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Client Management";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(171, 732);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(418, 31);
            this.UsernameBox.TabIndex = 12;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(171, 789);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(418, 31);
            this.PasswordBox.TabIndex = 13;
            // 
            // PasswordManagementLabel
            // 
            this.PasswordManagementLabel.AutoSize = true;
            this.PasswordManagementLabel.Location = new System.Drawing.Point(55, 695);
            this.PasswordManagementLabel.Name = "PasswordManagementLabel";
            this.PasswordManagementLabel.Size = new System.Drawing.Size(237, 25);
            this.PasswordManagementLabel.TabIndex = 14;
            this.PasswordManagementLabel.Text = "Password Management";
            // 
            // ChangePasswordButton
            // 
            this.ChangePasswordButton.Location = new System.Drawing.Point(595, 755);
            this.ChangePasswordButton.Name = "ChangePasswordButton";
            this.ChangePasswordButton.Size = new System.Drawing.Size(103, 43);
            this.ChangePasswordButton.TabIndex = 15;
            this.ChangePasswordButton.Text = "Change";
            this.ChangePasswordButton.UseVisualStyleBackColor = true;
            this.ChangePasswordButton.Click += new System.EventHandler(this.ChangePasswordButton_Click);
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(55, 735);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(110, 25);
            this.UsernameLabel.TabIndex = 16;
            this.UsernameLabel.Text = "Username";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(55, 792);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(106, 25);
            this.PasswordLabel.TabIndex = 17;
            this.PasswordLabel.Text = "Password";
            // 
            // SpreadsheetNameBox
            // 
            this.SpreadsheetNameBox.Location = new System.Drawing.Point(728, 624);
            this.SpreadsheetNameBox.Name = "SpreadsheetNameBox";
            this.SpreadsheetNameBox.Size = new System.Drawing.Size(435, 31);
            this.SpreadsheetNameBox.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(723, 596);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 25);
            this.label2.TabIndex = 19;
            this.label2.Text = "Spreadsheet Management";
            // 
            // SpreadsheetCreateButton
            // 
            this.SpreadsheetCreateButton.Location = new System.Drawing.Point(1180, 619);
            this.SpreadsheetCreateButton.Name = "SpreadsheetCreateButton";
            this.SpreadsheetCreateButton.Size = new System.Drawing.Size(100, 40);
            this.SpreadsheetCreateButton.TabIndex = 20;
            this.SpreadsheetCreateButton.Text = "Create";
            this.SpreadsheetCreateButton.UseVisualStyleBackColor = true;
            this.SpreadsheetCreateButton.Click += new System.EventHandler(this.SpreadsheetCreateButton_Click);
            // 
            // SpreadsheetDeleteButton
            // 
            this.SpreadsheetDeleteButton.Location = new System.Drawing.Point(1295, 619);
            this.SpreadsheetDeleteButton.Name = "SpreadsheetDeleteButton";
            this.SpreadsheetDeleteButton.Size = new System.Drawing.Size(100, 40);
            this.SpreadsheetDeleteButton.TabIndex = 21;
            this.SpreadsheetDeleteButton.Text = "Delete";
            this.SpreadsheetDeleteButton.UseVisualStyleBackColor = true;
            this.SpreadsheetDeleteButton.Click += new System.EventHandler(this.SpreadsheetDeleteButton_Click);
            // 
            // RecentActionsBox
            // 
            this.RecentActionsBox.Location = new System.Drawing.Point(728, 695);
            this.RecentActionsBox.Name = "RecentActionsBox";
            this.RecentActionsBox.Size = new System.Drawing.Size(667, 128);
            this.RecentActionsBox.TabIndex = 22;
            this.RecentActionsBox.Text = "";
            // 
            // RecentActionsLabel
            // 
            this.RecentActionsLabel.AutoSize = true;
            this.RecentActionsLabel.Location = new System.Drawing.Point(723, 667);
            this.RecentActionsLabel.Name = "RecentActionsLabel";
            this.RecentActionsLabel.Size = new System.Drawing.Size(157, 25);
            this.RecentActionsLabel.TabIndex = 23;
            this.RecentActionsLabel.Text = "Recent Actions";
            // 
            // AdministrativeClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1450, 876);
            this.Controls.Add(this.RecentActionsLabel);
            this.Controls.Add(this.RecentActionsBox);
            this.Controls.Add(this.SpreadsheetDeleteButton);
            this.Controls.Add(this.SpreadsheetCreateButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SpreadsheetNameBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.ChangePasswordButton);
            this.Controls.Add(this.PasswordManagementLabel);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeleteClientButton);
            this.Controls.Add(this.CreateClientButton);
            this.Controls.Add(this.AccountManagementBox);
            this.Controls.Add(this.ShutdownButton);
            this.Controls.Add(this.UpdateLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ClientSideLabel);
            this.Controls.Add(this.AddressLabel);
            this.Controls.Add(this.AddressBox);
            this.Controls.Add(this.UpdatesBox);
            this.Controls.Add(this.ClientBox);
            this.Name = "AdministrativeClientGUI";
            this.Text = "Administrative Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ClientBox;
        private System.Windows.Forms.RichTextBox UpdatesBox;
        private System.Windows.Forms.TextBox AddressBox;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Label ClientSideLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label UpdateLabel;
        private System.Windows.Forms.Button ShutdownButton;
        private System.Windows.Forms.TextBox AccountManagementBox;
        private System.Windows.Forms.Button CreateClientButton;
        private System.Windows.Forms.Button DeleteClientButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label PasswordManagementLabel;
        private System.Windows.Forms.Button ChangePasswordButton;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox SpreadsheetNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SpreadsheetCreateButton;
        private System.Windows.Forms.Button SpreadsheetDeleteButton;
        private System.Windows.Forms.RichTextBox RecentActionsBox;
        private System.Windows.Forms.Label RecentActionsLabel;
    }
}

