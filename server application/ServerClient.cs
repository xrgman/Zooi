using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Network;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Network.Packets;
using System.IO;
using System.Net;

namespace server_application
{
    [Serializable]
    public class ServerClient : ServerInterface
    {
        public TcpClient client { get; set; }
        private Serverapplication server;
        public User user { get; set; }

        // Certificaat
        private X509Certificate2 cert = new X509Certificate2(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\Shared Server Client\cyclemaster.pfx",
                                                              "admin", X509KeyStorageFlags.MachineKeySet);

        private SslStream ssl;
        public SslStream SSL { get { return ssl; } }

        public ServerClient(TcpClient client, Serverapplication server, SslStream ssl)
        {
            this.server = server;
            this.client = client;
            this.ssl = ssl;

            // Authenticate cert
           
            SSL.AuthenticateAsServer(cert, true, SslProtocols.Tls12, true);
            string ipAddress = "" + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            Console.WriteLine("Connected by {0} ({1}) on {2} ", ssl.CipherAlgorithm, ssl.CipherStrength, ipAddress.ToString());

            if (client != null)
            {
                new Thread(() =>
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    while (client.Connected)
                    {
                        Packet packet = NetworkFlow.ReadPacket(SSL);
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
                        NetworkFlow.SendPacket(new PacketLoginResponse(true, user is Physician), SSL);
                        Console.WriteLine("{0} succesfully logged in.",username);
                        this.user = user;
                        break;
                    }
                    else //wrong password
                    {
                        Console.WriteLine("wrong password");
                        NetworkFlow.SendPacket(new PacketLoginResponse(false, user is Physician), SSL);
                        break;
                    }
                }
            }
        }

        public void GiveUser(string username, bool allUsers, string physicianName)
        {
            Console.WriteLine("Someone is requesting the user: " + username);
            if(username.Equals("*"))
            {
                if (allUsers)
                    NetworkFlow.SendPacket(new PacketGiveUserResponse(server.users), SSL);
                else
                    NetworkFlow.SendPacket(new PacketGiveUserResponse(server.GetConnectedUsers(physicianName)),SSL);
                Console.WriteLine("Sending all users " + allUsers );
            }
            else
            {
                Console.WriteLine("Sending user: " + username);
            }
        }

        public void AddUser(User newUser, string physicianName)
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
                server.AddNewUser(newUser, physicianName);

            Console.WriteLine("Succesfully created new user {0} with password {1}.", newUser.username, newUser.password);

        }

        public void Broadcast(string sender, string message)
        {
            foreach(User user in server.users)
            {
                ServerClient client = server.getUser(user.username);
                if (client.SSL != null)
                {
                    NetworkFlow.SendPacket(new PacketBroadcastResponse(sender, message), client.SSL);
                }
            }
        }
        public void ChatMessage(string sender, string receiver, string message)
        {
            if (receiver == sender)
            {
                Console.WriteLine("Gebruiker STUURT!!!!");
                foreach (User u in server.users)
                {
                    try
                    {
                        Physician p = (Physician)u;
                        if ((p != null) && (receiver == sender))
                        {
                            if (p.hashClient(sender) && (receiver == sender))
                            {
                                receiver = p.username;
                            }
                        }
                    }
                    catch { }
                }
            }
            
            ServerClient client = server.getUser(receiver);
            NetworkFlow.SendPacket(new PacketChatMessage(message,sender, receiver),client.SSL);
                
        }


        public void BikeValues(string power, string time, string distance, string username)
        {
            ServerClient client = server.getUser(username);
            NetworkFlow.SendPacket(new PacketBikeValuesResponse(power,time,distance), client.SSL);
        }
    }
}
