using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using NetworkLibrary;
using NetworkLibrary.Packets;

namespace ServerApplication
{
    class ServerClient : ServerInterface
    {
        private ServerApplication server;
        private SslStream stream;

        public ServerClient(TcpClient client, ServerApplication server, SslStream stream)
        {
            this.server = server;
            this.stream = stream;
            if (client != null)
            {
                new Thread(() =>
                {
                    while (client.Connected)
                    {
                        Packet packet = NetworkCommunication.ReadPacket(stream);
                        if (packet != null)
                        {
                            Console.WriteLine("recieved packet");
                            packet.handleServerSide(this);
                        }
                    }
                    //When disconnected:
                    server.ConnectedClients.Remove(this);
                    Console.WriteLine("Client disconnected");
                }).Start();
            }
        }

        public void Login(string username, string password)
        {
            Console.WriteLine("login packet, username: " + username + ", wachtwoord: " + password);
            //Test sending back:
            NetworkCommunication.SendPacket(new PacketLogin(true),stream);
            //Moet je maar ff in project pakke en kopiere en aanpasse op structuur;
        }
    }
}
