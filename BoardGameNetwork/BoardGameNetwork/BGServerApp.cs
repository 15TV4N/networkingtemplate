using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BoardGameNetwork
{
    class BGServerApp
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        private const String ip = "127.0.0.1";
        private const Int32 port = 11000;
        private Thread acceptClientThread;
        public static List<BGClient> clients;
        public static List<Lobby> lobbies;

        public BGServerApp()
        {
            AllocConsole();
            clients = new List<BGClient>();
            lobbies = new List<Lobby>();
            this.acceptClientThread = new Thread(() => AcceptClients());
            this.acceptClientThread.Priority = ThreadPriority.AboveNormal;
            this.acceptClientThread.Start();
        }
        public void AcceptClients()
        {
            TcpListener server = null;
            try
            {
                Console.WriteLine("[{0}]\tInitializing server...",
                    DateTime.Now.ToShortTimeString());
                server = new TcpListener(IPAddress.Parse(ip), port);
                server.Start();
                Console.WriteLine("[{0}]\tServer started. - {1}",
                    DateTime.Now.ToShortTimeString(), server.LocalEndpoint);
                Console.WriteLine("[{0}]\tWaiting for clients...",
                    DateTime.Now.ToShortTimeString());
                while (true)
                {
                    Thread.Sleep(1000);
                    while (server.Pending())
                    {
                        lock (clients) clients.Add(new BGClient(server.AcceptTcpClient()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[{0}]\tAn error has occured:\n{1}\n",
                    DateTime.Now, e.ToString());
            }
            finally
            {
                server.Stop(); 
                Console.WriteLine("[{0}]\tServer offline.",
                    DateTime.Now.ToShortTimeString());
                Console.ReadKey();
            }
        }
    }
    public class BGClient
    {
        private TcpClient client;
        public NetworkStream stream;
        private Thread receiverThread;

        public string nick;
        public Lobby currentLobby;

        public BGClient(TcpClient client)
        {
            this.nick = "unavailable";
            currentLobby = null;
            this.client = client;
            this.receiverThread = new Thread(() => Receiver());
            receiverThread.Priority = ThreadPriority.Normal;
            receiverThread.Start();
        }
        public void Receiver()
        {
            try
            {
                Console.WriteLine("[{0}]\tListening for client {1}.",
                    DateTime.Now.ToShortTimeString(), this.client.Client.RemoteEndPoint.ToString().Split(':')[1]);
                stream = this.client.GetStream();
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
                                var handleThread = new Thread(() => HandleRequest(data));
                                handleThread.Priority = ThreadPriority.AboveNormal;
                                handleThread.Start();
                                if (!stream.DataAvailable) break;
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException tae) { }
            catch (Exception e)
            {
                Console.WriteLine("[{0}]\tUnexpected error:\n\n{1}\n",
                    DateTime.Now.ToShortTimeString(), e.ToString());
            }
            finally
            {
                lock (BGServerApp.clients) BGServerApp.clients.Remove(this);
                stream.Close();
                client.Close();
            }
        }
        private void HandleRequest(string data)
        {
            var args = data.Split('#');
            switch (args[0])
            {
                case "changenick":
                    this.nick = args[1];
                    Console.WriteLine("[{0}]\tClient {1} has set their nick to {2}.",
                        DateTime.Now.ToShortTimeString(), this.client.Client.RemoteEndPoint.ToString().Split(':')[1], nick);
                    break;
                case "disconnect":
                    receiverThread.Abort();
                    Console.WriteLine("[{0}]\tClient {1} has disconnected",
                        DateTime.Now.ToShortTimeString(), (nick == "unavailable") ? this.client.Client.RemoteEndPoint.ToString().Split(':')[1] : nick);
                    lock (BGServerApp.clients) BGServerApp.clients.Remove(this);
                    if (currentLobby!=null)lock (this.currentLobby.members) this.currentLobby.members.Remove(this);
                    break;
                case "getlobbies":

                    Console.WriteLine("[{0}]\tClient {1} has requested the lobbylist.",
                        DateTime.Now.ToShortTimeString(), nick);
                    var list = String.Empty;
                    lock (BGServerApp.lobbies)
                    {
                        foreach (var l in BGServerApp.lobbies)
                        {
                            list += l.ToString();
                            list += "$";
                        }
                    }
                    if (list == String.Empty) list = "none";
                    var listData = Encoding.ASCII.GetBytes(list);
                    lock (stream)
                    {
                        stream.Write(listData, 0, listData.Length);
                        stream.Flush();
                    }
                    break;
                case "newlobby":
                    Console.WriteLine("[{0}]\tClient {1} has requested to create new lobby.",
                        DateTime.Now.ToShortTimeString(), nick);
                    new Lobby(this, args[1]);
                    break;
                case "joinlobby":
                    Console.WriteLine("[{0}]\tClient {1} has requested to join lobby {2}.",
                        DateTime.Now.ToShortTimeString(), nick, args[1]);
                    try
                    {
                        var response = "false";
                        lock (BGServerApp.lobbies) foreach (var l in BGServerApp.lobbies) if (l.id == Int32.Parse(args[1]))
                                {
                                    try { currentLobby.RemoveMember(this); }
                                    catch { }
                                    l.AddMember(this);
                                    response = "true";
                                    break;
                                }
                        
                        var msg = Encoding.ASCII.GetBytes(response);
                        lock (stream)
                        {
                            stream.Write(msg, 0, msg.Length);
                            stream.Flush();
                        }
                    }
                    catch
                    {
                        var msg = Encoding.ASCII.GetBytes("false");
                        lock (stream)
                        {
                            stream.Write(msg, 0, msg.Length);
                            stream.Flush();
                        }
                    }
                    break;
                case "leavelobby":
                    Console.WriteLine("[{0}]\tClient {1} has requested to leave lobby {2}",
                        DateTime.Now.ToShortTimeString(), nick, currentLobby.id);
                    lock (BGServerApp.lobbies)
                    {
                        foreach (var l in BGServerApp.lobbies)
                        {
                            if (l.id == this.currentLobby.id)
                            {
                                l.RemoveMember(this);
                                break;
                            }
                        }
                    }
                    break;
                case "say":
                    Console.WriteLine("wanna say sth");
                    lock (BGServerApp.lobbies)
                    {
                        foreach (var l in BGServerApp.lobbies)
                        {
                            if (l.id == this.currentLobby.id)
                            {
                                l.Say(args[1]);
                                break;
                            }
                        }
                    }
                    Console.WriteLine("said sth");
                    break;
            }
        }
    }
    public class Lobby
    {
        public int id;
        public string name;
        public string chat;
        public DateTime created;
        public List<BGClient> members;
        public Lobby(BGClient creator, string name)
        {
            this.id = UniqueId();
            lock (BGServerApp.lobbies) BGServerApp.lobbies.Add(this);
            chat = String.Empty;
            members = new List<BGClient>();
            this.name = name;
            this.chat = "[" + DateTime.Now.ToShortTimeString() + "] Lobby created.";
            AddMember(creator);
            created = DateTime.Now;
        }
        private int UniqueId()
        {
            var uid = new Random().Next(100, 999);
            lock (BGServerApp.lobbies) foreach (var l in BGServerApp.lobbies) { if (uid == l.id) UniqueId(); }
            return uid;
        }
        public void AddMember(BGClient client)
        {
            try { client.currentLobby.RemoveMember(client); }
            catch { }
            lock (members) members.Add(client);
            client.currentLobby = this;
        }
        public void RemoveMember(BGClient client)
        {
            lock (members) members.Remove(client);
            client.currentLobby = null;
            if (members.Count == 0)
            {
                lock (BGServerApp.lobbies) BGServerApp.lobbies.Remove(this);
                Console.WriteLine("[{0}]\tLobby removed: {1}",
                        DateTime.Now.ToShortTimeString(), id);
            }
        }
        public override string ToString()
        {
            if (this.members.Count == 0) return string.Empty;
            string data = string.Empty;
            data += id;
            data += "#";
            data += name;
            data += "#";
            data += created.ToShortTimeString();
            lock (members)
            {
                foreach (var member in members)
                {
                    data += "#";
                    data += member.nick;
                }
            }
            return data;
        }
        public void Say(string msg)
        {
            lock (members)
            {
                foreach (var m in members)
                {
                    var data = Encoding.ASCII.GetBytes(msg);
                    m.stream.Write(data, 0, data.Length);
                    m.stream.Flush();
                }
            }
        }
    }
}
