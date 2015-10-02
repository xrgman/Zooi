using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Network;


namespace server_application
{
    [Serializable]
    public class ServerClient : ServerInterface
    {
        public TcpClient client { get; set; }
        private Serverapplication server;
        private User user;


        public ServerClient(TcpClient client, Serverapplication server)
        {
            this.server = server;
            this.client = client;
            new Thread(() =>
            { 
                BinaryFormatter formatter = new BinaryFormatter();
                while (client.Connected)
                {
                    Packet packet = NetworkFlow.ReadPacket(client);
                    if(packet != null)
                    {
                        Console.WriteLine("recieved packet");
                        packet.handleServerSide(this);
                    }
                }
                server.getConnectedClients().Remove(this);
                Console.WriteLine("Client disconnected");
            }).Start();
        }

        public void login(string username, string password)
        {
            Console.WriteLine("Iemand probeert in te loggen met " + username + ", " + password);
            //Combining lists: 
            List<User> users = new List<User>();
            users.AddRange(server.userClients);
            users.AddRange(server.physicians);
            //Actual login checking:
            foreach (User user in users)
            {
                if (user.username.Equals(username))
                {
                    if (user.password.Equals(password)) //succesfull login
                    { 
                        NetworkFlow.SendPacket(new PacketLoginResponse(true, user is Physician), client);
                        Console.WriteLine("{0} succesfully logged in.",username);
                        this.user = user;
                        break;
                    }
                    else //wrong password
                    {
                        Console.WriteLine("wrong passadbjlas;kfh");
                        NetworkFlow.SendPacket(new PacketLoginResponse(false, false), client);
                        break;
                    }
                }
            }
        }
    }
}
