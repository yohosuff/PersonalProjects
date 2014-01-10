namespace Server
{
    partial class ServerForm
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
            this.startServerButton = new System.Windows.Forms.Button();
            this.serverStatusLabel = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.sendAllButton = new System.Windows.Forms.Button();
            this.connectedClientsLabel = new System.Windows.Forms.Label();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.portNumberTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startServerButton
            // 
            this.startServerButton.Location = new System.Drawing.Point(9, 13);
            this.startServerButton.Name = "startServerButton";
            this.startServerButton.Size = new System.Drawing.Size(75, 36);
            this.startServerButton.TabIndex = 0;
            this.startServerButton.Text = "Start Server";
            this.startServerButton.UseVisualStyleBackColor = true;
            this.startServerButton.Click += new System.EventHandler(this.startServerButton_Click);
            // 
            // serverStatusLabel
            // 
            this.serverStatusLabel.AutoSize = true;
            this.serverStatusLabel.Location = new System.Drawing.Point(10, 55);
            this.serverStatusLabel.Name = "serverStatusLabel";
            this.serverStatusLabel.Size = new System.Drawing.Size(77, 13);
            this.serverStatusLabel.TabIndex = 1;
            this.serverStatusLabel.Text = "Server is down";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(10, 71);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(572, 132);
            this.logTextBox.TabIndex = 2;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(11, 209);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(501, 20);
            this.messageTextBox.TabIndex = 3;
            this.messageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageTextBox_KeyDown);
            // 
            // sendAllButton
            // 
            this.sendAllButton.Enabled = false;
            this.sendAllButton.Location = new System.Drawing.Point(518, 209);
            this.sendAllButton.Name = "sendAllButton";
            this.sendAllButton.Size = new System.Drawing.Size(64, 20);
            this.sendAllButton.TabIndex = 4;
            this.sendAllButton.Text = "Send All";
            this.sendAllButton.UseVisualStyleBackColor = true;
            this.sendAllButton.Click += new System.EventHandler(this.sendAllButton_Click);
            // 
            // connectedClientsLabel
            // 
            this.connectedClientsLabel.AutoSize = true;
            this.connectedClientsLabel.Location = new System.Drawing.Point(13, 236);
            this.connectedClientsLabel.Name = "connectedClientsLabel";
            this.connectedClientsLabel.Size = new System.Drawing.Size(128, 13);
            this.connectedClientsLabel.TabIndex = 5;
            this.connectedClientsLabel.Text = "Connected Clients (None)";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(93, 29);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(100, 20);
            this.ipAddressTextBox.TabIndex = 6;
            this.ipAddressTextBox.Text = "127.0.0.1";
            // 
            // portNumberTextBox
            // 
            this.portNumberTextBox.Location = new System.Drawing.Point(200, 29);
            this.portNumberTextBox.Name = "portNumberTextBox";
            this.portNumberTextBox.Size = new System.Drawing.Size(67, 20);
            this.portNumberTextBox.TabIndex = 7;
            this.portNumberTextBox.Text = "1666";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Port Number";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 407);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portNumberTextBox);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.connectedClientsLabel);
            this.Controls.Add(this.sendAllButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.serverStatusLabel);
            this.Controls.Add(this.startServerButton);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startServerButton;
        private System.Windows.Forms.Label serverStatusLabel;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Button sendAllButton;
        private System.Windows.Forms.Label connectedClientsLabel;
        private System.Windows.Forms.TextBox ipAddressTextBox;
        private System.Windows.Forms.TextBox portNumberTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

