using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace BoardGameNetwork
{
    /// <summary>
    /// implementation in progress...
    /// </summary>
    public partial class RoomForm : Form
    {
        Thread receiverThread;
        BGClientApp client;
        List<string>chat;
        public RoomForm(BGClientApp client)
        {
            chat = new List<string>();
            this.client = client;
            InitializeComponent();
            receiverThread = new Thread(() => client.RoomReceiver(this));
            receiverThread.Start();
        }
        public void SendMessage(string msg)
        {
            string req = "say#" + client.nick + ": " + msg;
            client.SendRequest(req,true);
        }
        public void ReceiveMessage(string msg)
        {
            (new Thread(() => Application.Run(new ErrorForm("er", "msg rcvd")))).Start();
            chat.Add(msg);
            chatBox.Text += (msg+ "\n");
        }

        private void sayBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '#')
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == (char)Keys.Return)
            {
                if (sayBox.Text == String.Empty)
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    client.SendRequest("say#" + client.nick + ": " + sayBox.Text);
                    sayBox.Text = String.Empty;
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}
