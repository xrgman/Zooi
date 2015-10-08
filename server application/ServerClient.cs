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
        public User user { get; set; }

        public ServerClient(TcpClient client, Serverapplication server)
        {
            this.server = server;
            this.client = client;
            if (client != null)
            {
                new Thread(() =>
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    while (client.Connected)
                    {
                        Packet packet = NetworkFlow.ReadPacket(server.SSL);
                        if (packet != null)
                        {
                            Console.WriteLine("recieved packet");
                            packet.handleServerSide(this);
                        }
                        else
                        {
                            //Console.WriteLine("Null Packet ");
                        }
                    }
                    server.getConnectedClients().Remove(this);
                    Console.WriteLine("Client disconnected");
                }).Start();
            }
        }

        public void Login(string username, string password)
        {
            Console.WriteLine("Iemand probeert in te loggen als " + username + ", wachtwoord: " + password);
            //Actual login checking:
            foreach (User user in server.users)
            {
                if (user.username.ToLower().Equals(username.ToLower()))
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

        public void GiveUser(string username, bool allUsers)
        {
            Console.WriteLine("Someone is requesting the user: " + username);
            if(username.Equals("*"))
            {
                if (allUsers)
                    NetworkFlow.SendPacket(new PacketGiveUserResponse(server.users), server.SSL);
                else
                    NetworkFlow.SendPacket(new PacketGiveUserResponse(server.GetConnectedUsers()),server.SSL);
                Console.WriteLine("Sending all users " + allUsers );
            }
            else
            {
                Console.WriteLine("Sending user: " + username);
            }
        }

        public void AddUser(User newUser)
        {
            Boolean addUser = true;

            //Last check to prevent creating an user that already excists
            foreach (User user in server.users)
            {
                if (user.username.ToLower().Equals(newUser.username.ToLower()))
                {
                    addUser = false;
                    break;
                }
            }
            //if no duplicate has been found, newUser can be added to the userlist
            if (addUser)
                server.AddNewUser(newUser);

            Console.WriteLine("Succesfully created new user {0} with password {1}.", newUser.username, newUser.password);

        }
    }
}
