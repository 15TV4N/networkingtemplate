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
    public partial class LobbyForm : Form
    {
        BGClientApp client;
        List<LobbyCopy> lobbies;
        public LobbyForm(BGClientApp client)
        {
            lobbies = new List<LobbyCopy>();
            this.client = client;
            InitializeComponent();
            this.Text = "Lobbies - " + client.nick;
            this.DialogResult = DialogResult.Cancel;
        }
        private void createButton_Click(object sender, EventArgs e)
        {
            using (var form = new ChooseLobbyNameForm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    client.SendRequest("newlobby#" + form.lobbyName,true);
                    ShowRoom();
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            lobbies = client.GetLobbies();
            this.SuspendLayout();
            lobbyList.ResetText();
            lobbyList.DataSource = lobbies;
            lobbyList.DisplayMember = "name";
            this.ResumeLayout();
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            if (lobbyList.SelectedIndex < 0) return;
            if (client.RequestJoin(Int32.Parse(lobbies[lobbyList.SelectedIndex].id)))
                ShowRoom();
            else
            {
                new ErrorForm("Error", "Could not join lobby.").ShowDialog();
                lobbies = client.GetLobbies();
                lobbyList.DataSource = lobbies;
            }
        }
        private void ShowRoom()
        {
            using (var form = new RoomForm(client))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.Cancel) client.SendRequest("leavelobby",true);
            }
        }
    }
    
}
