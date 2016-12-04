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
    public partial class ChooseLobbyNameForm : Form
    {
        public string lobbyName;

        public ChooseLobbyNameForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Abort;
            this.lobbyNameBox.Text = "Lobby" + new Random().Next(10, 99);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            lobbyName = lobbyNameBox.Text;
            if (lobbyName.Length < 3) (new Thread(() => Application.Run(new ErrorForm("Error", "Lobby name too short")))).Start();
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
