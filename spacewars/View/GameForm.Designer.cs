namespace View
{
    partial class GameForm
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
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.CustomShipButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(10, 12);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(41, 13);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server:";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(53, 9);
            this.serverTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(96, 20);
            this.serverTextBox.TabIndex = 1;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(151, 12);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(189, 9);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(96, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(287, 6);
            this.connectButton.Margin = new System.Windows.Forms.Padding(1);
            this.connectButton.MinimumSize = new System.Drawing.Size(58, 18);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(58, 25);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButtonPushed_Handler);
            // 
            // CustomShipButton
            // 
            this.CustomShipButton.Location = new System.Drawing.Point(349, 6);
            this.CustomShipButton.Name = "CustomShipButton";
            this.CustomShipButton.Size = new System.Drawing.Size(75, 25);
            this.CustomShipButton.TabIndex = 6;
            this.CustomShipButton.Text = "Upload Skin";
            this.CustomShipButton.UseVisualStyleBackColor = true;
            this.CustomShipButton.Click += new System.EventHandler(this.CustomShipButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 411);
            this.Controls.Add(this.CustomShipButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.serverLabel);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "GameForm";
            this.Text = "SpaceWars!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button CustomShipButton;
    }
}

