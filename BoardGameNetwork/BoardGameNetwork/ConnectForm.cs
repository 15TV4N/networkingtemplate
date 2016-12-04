using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace BoardGameNetwork
{
    public partial class ConnectForm : Form
    {
        public TcpClient client;
        public ConnectForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            ipBox.Text = "127.0.0.1";
            portBox.Text = "11000";
        }
        public ConnectForm(ref TcpClient client)
        {
            InitializeComponent();
            ipBox.Text = "127.0.0.1";
            portBox.Text = "11000";
            this.client = client;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                var ip = IPAddress.Parse(ipBox.Text);
                var port = Int32.Parse(portBox.Text);

                client = new TcpClient(ip.ToString(), port);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exc)
            {
                
                (new Thread(() => Application.Run(new ErrorForm("Could not connect", exc.ToString())))).Start();
            }
            finally
            {

            }
        }
    }
}
