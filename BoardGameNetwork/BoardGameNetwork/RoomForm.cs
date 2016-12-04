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
    public partial class RoomForm : Form
    {
        Thread receiverThread;
        BGClientApp client;
        public RoomForm(BGClientApp client)
        {
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
            lock(chatBox)chatBox.Text += msg;
        }
    }
}
