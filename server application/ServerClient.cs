using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Network;
using WindowsFormsApplication1;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

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
                Packet packet = NetworkFlow.ReadPacket(server.SSL);
                    if(packet != null)
                    {
                        Console.WriteLine("recieved packet");
                        packet.handleServerSide(this);
                    } else
                    {
                        //Console.WriteLine("Null PAcket ");
                    }
                }
                server.getConnectedClients().Remove(this);
                Console.WriteLine("Client disconnected");
            }).Start();
        }

        public void login(string username, string password)
        {
            Console.WriteLine("Iemand probeert in te loggen als " + username + ", wachtwoord: " + password);
            //Actual login checking:
            foreach (User user in server.users)
            {
                if (user.username.Equals(username))
                {
                    if (PasswordHash.ValidatePassword(password, user.password)) //succesfull login
                    { 
                        NetworkFlow.SendPacket(new PacketLoginResponse(true, user is Physician), server.SSL);
                        Console.WriteLine("{0} succesfully logged in.",username);
                        this.user = user;
                        break;
                    }
                    else //wrong password
                    {
                        Console.WriteLine("wrong password");
                        NetworkFlow.SendPacket(new PacketLoginResponse(false, user is Physician), server.SSL);
                        break;
                    }
                }
            }
        }
    }
}
