namespace BoardGameNetwork
{
    partial class RoomForm
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
            this.chatBox = new System.Windows.Forms.TextBox();
            this.sayBox = new System.Windows.Forms.TextBox();
            this.playerlistBox = new System.Windows.Forms.TextBox();
            this.readyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatBox
            // 
            this.chatBox.Location = new System.Drawing.Point(12, 12);
            this.chatBox.Multiline = true;
            this.chatBox.Name = "chatBox";
            this.chatBox.ReadOnly = true;
            this.chatBox.Size = new System.Drawing.Size(167, 149);
            this.chatBox.TabIndex = 1;
            // 
            // sayBox
            // 
            this.sayBox.Location = new System.Drawing.Point(12, 167);
            this.sayBox.Name = "sayBox";
            this.sayBox.Size = new System.Drawing.Size(167, 20);
            this.sayBox.TabIndex = 0;
            this.sayBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sayBox_KeyPress);
            // 
            // playerlistBox
            // 
            this.playerlistBox.Location = new System.Drawing.Point(185, 12);
            this.playerlistBox.Multiline = true;
            this.playerlistBox.Name = "playerlistBox";
            this.playerlistBox.ReadOnly = true;
            this.playerlistBox.Size = new System.Drawing.Size(87, 149);
            this.playerlistBox.TabIndex = 2;
            // 
            // readyButton
            // 
            this.readyButton.Location = new System.Drawing.Point(185, 167);
            this.readyButton.Name = "readyButton";
            this.readyButton.Size = new System.Drawing.Size(87, 20);
            this.readyButton.TabIndex = 3;
            this.readyButton.Text = "Ready";
            this.readyButton.UseVisualStyleBackColor = true;
            // 
            // RoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 193);
            this.Controls.Add(this.readyButton);
            this.Controls.Add(this.playerlistBox);
            this.Controls.Add(this.sayBox);
            this.Controls.Add(this.chatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RoomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RoomForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox chatBox;
        private System.Windows.Forms.TextBox sayBox;
        private System.Windows.Forms.TextBox playerlistBox;
        private System.Windows.Forms.Button readyButton;
    }
}