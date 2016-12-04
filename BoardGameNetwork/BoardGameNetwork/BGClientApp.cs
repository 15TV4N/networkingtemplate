using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;


namespace BoardGameNetwork
{
    public class BGClientApp
    {
        private const String ip = "127.0.0.1";
        private const Int32 port = 11000;
        private TcpClient client;
        private NetworkStream stream;

        public string nick;
        public RoomForm currRoom;

        public BGClientApp(Point loc)
        {
            using (var form = new ConnectForm())
            {
                form.Location = loc;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    client = form.client;
                    stream = client.GetStream();
                }
                else Thread.CurrentThread.Abort();
            }
            using (var form = new ChooseNickForm())
            {
                form.Location = loc;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.nick = form.nick;
                    SendRequest("changenick#" + nick,true);
                }
                else
                {
                    SendRequest("disconnect",true);
                    Thread.CurrentThread.Abort();
                }
            }
            using (var form = new LobbyForm(this))
            {
                form.Location = loc;
                var result = form.ShowDialog();
                if (result == DialogResult.Cancel) SendRequest("disconnect",true);
            }
        }
        public void SendRequest(string request, bool lockStream = false)
        {
            if (lockStream)
            {
                lock(stream)
                {
                    var data = Encoding.ASCII.GetBytes(request);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
            else
            {
                var data = Encoding.ASCII.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }

        }
        public List<LobbyCopy> GetLobbies()
        {
            var receivedBytes = new byte[512];
            string lobbyData = "";
            lock (stream)
            {
                SendRequest("getlobbies");
                while (!stream.DataAvailable) Thread.Sleep(100);

                int i;
                while ((i = stream.Read(receivedBytes, 0, receivedBytes.Length)) != 0)
                {
                    lobbyData = System.Text.Encoding.ASCII.GetString(receivedBytes, 0, i);
                    if (!stream.DataAvailable) break;
                }
            }
            if (lobbyData == "none") return new List<LobbyCopy>();
            List<LobbyCopy> list = new List<LobbyCopy>();
            string[] lobbies = lobbyData.Split('$');
            for (int i = 0; i < lobbies.Length-1; i++)
            {
                if (lobbies[i] == "") continue;
                string[] lobbyInfo = lobbies[i].Split('#');
                LobbyCopy l = new LobbyCopy(lobbyInfo);
                list.Add(l);
            }
            return list;
        }
        public bool RequestJoin(int lobbyId)
        {
            var receivedBytes = new byte[512];
            var boolMsg = "false";
            lock (stream)
            {
                SendRequest("joinlobby#" + lobbyId);
                while (!stream.DataAvailable) Thread.Sleep(100);

                int i;
                while ((i = stream.Read(receivedBytes, 0, receivedBytes.Length)) != 0)
                {
                    boolMsg = System.Text.Encoding.ASCII.GetString(receivedBytes, 0, i);
                    if (!stream.DataAvailable) break;
                }
            }
            return Boolean.Parse(boolMsg);
        }
        public void RoomReceiver(RoomForm room)
        {
            currRoom = room;
            while (true)
            {
                var receivedBytes = new byte[512];
                while (this.client.Connected)
                {
                    Thread.Sleep(1000);
                    if (stream.DataAvailable)
                    {
                        lock (stream)
                        {
                            int i;
                            while ((i = stream.Read(receivedBytes, 0, receivedBytes.Length)) != 0)
                            {
                                var data = System.Text.Encoding.ASCII.GetString(receivedBytes, 0, i);
                                if (!stream.DataAvailable)
                                {
                                    RoomUpdate(data);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void RoomUpdate(string data)
        {
            var args = data.Split('#');
            switch (args[0])
            {
                case "say":
                    currRoom.ReceiveMessage(args[1]);
                    break;
            }
        }
    }
    public struct LobbyCopy
    {
        public string createTime { get; set; }
        public string id;
        public string name { get; set; }
        public string[] members;
        public int memberCount { get { return members.Length; } }
        public LobbyCopy(string [] args)
        {
            this.id = args[0];
            this.name = args[1];
            this.createTime = args[2];
            members = new string[args.Length - 3];
            for (int i=3; i<args.Length; i++)
            {
                members[i - 3] = args[i];
            }
        }
        public override string ToString()
        {
            return "[" + createTime + "]\t" + name + "\t(" + members.Length + ")";
        }
    }
}
