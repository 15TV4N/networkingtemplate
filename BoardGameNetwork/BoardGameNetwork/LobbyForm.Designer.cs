namespace BoardGameNetwork
{
    partial class LobbyForm
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
            this.lobbyList = new System.Windows.Forms.ListBox();
            this.createButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lobbyList
            // 
            this.lobbyList.FormattingEnabled = true;
            this.lobbyList.Location = new System.Drawing.Point(12, 12);
            this.lobbyList.MultiColumn = true;
            this.lobbyList.Name = "lobbyList";
            this.lobbyList.Size = new System.Drawing.Size(175, 238);
            this.lobbyList.TabIndex = 0;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(195, 41);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 1;
            this.createButton.Text = "Create new";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(195, 12);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(75, 23);
            this.joinButton.TabIndex = 2;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(195, 227);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 3;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // LobbyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 262);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.lobbyList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LobbyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Lobbies";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lobbyList;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Button refreshButton;
    }
}