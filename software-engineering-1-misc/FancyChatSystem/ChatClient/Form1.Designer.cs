namespace ChatClient
{
  partial class Form1
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
      this.messages = new System.Windows.Forms.TextBox();
      this.messageToSendBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.serverAddress = new System.Windows.Forms.TextBox();
      this.connectButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // messages
      // 
      this.messages.Location = new System.Drawing.Point(13, 109);
      this.messages.Multiline = true;
      this.messages.Name = "messages";
      this.messages.ReadOnly = true;
      this.messages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.messages.Size = new System.Drawing.Size(1324, 608);
      this.messages.TabIndex = 0;
      // 
      // messageToSendBox
      // 
      this.messageToSendBox.Location = new System.Drawing.Point(13, 734);
      this.messageToSendBox.Name = "messageToSendBox";
      this.messageToSendBox.Size = new System.Drawing.Size(1324, 31);
      this.messageToSendBox.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 38);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(72, 25);
      this.label1.TabIndex = 2;
      this.label1.Text = "server";
      // 
      // serverAddress
      // 
      this.serverAddress.Location = new System.Drawing.Point(92, 38);
      this.serverAddress.Name = "serverAddress";
      this.serverAddress.Size = new System.Drawing.Size(222, 31);
      this.serverAddress.TabIndex = 3;
      this.serverAddress.Text = "localhost";
      // 
      // connectButton
      // 
      this.connectButton.Location = new System.Drawing.Point(359, 38);
      this.connectButton.Name = "connectButton";
      this.connectButton.Size = new System.Drawing.Size(125, 42);
      this.connectButton.TabIndex = 6;
      this.connectButton.Text = "connect";
      this.connectButton.UseVisualStyleBackColor = true;
      this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1349, 802);
      this.Controls.Add(this.connectButton);
      this.Controls.Add(this.serverAddress);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.messageToSendBox);
      this.Controls.Add(this.messages);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox messages;
    private System.Windows.Forms.TextBox messageToSendBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox serverAddress;
    private System.Windows.Forms.Button connectButton;
  }
}

