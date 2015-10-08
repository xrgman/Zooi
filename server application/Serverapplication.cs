using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Net.Security;
using Network;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.Serialization;
using WindowsFormsApplication1;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;


namespace server_application
{
    [Serializable]
    public class Serverapplication
    {
        // Certificaat
        private X509Certificate2 cert = new X509Certificate2(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\Shared Server Client\cyclemaster.pfx",
                                                              "admin", X509KeyStorageFlags.MachineKeySet);

        private List<ServerClient> ConnectedClients { get; }

        public List<User> users = new List<User>();

        private   SslStream ssl;
        public    SslStream SSL { get { return ssl; }}

        public Serverapplication()
        {
           ConnectedClients = new List<ServerClient>();

            //add test users obviously for testing
            users.Add(new UserClient("Henk", PasswordHash.HashPassword("banaan")));
            users.Add(new Physician("Jaap", PasswordHash.HashPassword("appel")));
            //Test online users:
            ServerClient boefje = new ServerClient(null, this);
            boefje.user = new UserClient("Boef", PasswordHash.HashPassword("lol"));
            ConnectedClients.Add(boefje);
            ServerClient boefje2 = new ServerClient(null, this);
            boefje2.user = new UserClient("Boef2", PasswordHash.HashPassword("lol"));
            ConnectedClients.Add(boefje2);

            TcpListener listener = new TcpListener(IPAddress.Loopback, 130);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for Client Connections");
                TcpClient client = listener.AcceptTcpClient();
                // Authenticate cert
                ssl = new SslStream(client.GetStream());
                SSL.AuthenticateAsServer(cert, true, SslProtocols.Tls12, true);

                string ipAddress = "" + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                Console.WriteLine("Connected by {0} ({1}) on {2} ", ssl.CipherAlgorithm, ssl.CipherStrength, ipAddress.ToString());
                ConnectedClients.Add(new ServerClient(client, this));
            }
        }

        public List<ServerClient> getConnectedClients()
        {
            return ConnectedClients;
        }

        public List<User> GetConnectedUsers()
        {
            List<User> users = new List<User>();
            foreach(ServerClient client in ConnectedClients)
            {
                if(!(client.user is Physician))
                    users.Add(client.user);
            }
            return users;
        }

        public void SaveAllData()
        {
            //save clientfiles
            SslStream streamClient = new SslStream(File.Open("clients.a3", FileMode.Create));
            BinaryFormatter bformatter = new BinaryFormatter();

            Console.WriteLine("Writing clients Information");
            foreach (User u in users) 
                bformatter.Serialize(streamClient, u);

            streamClient.Close();
        }

        public void LoadAllData()
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            //Open the file and read values from client.
            SslStream streamClient = new SslStream(File.Open("clients.a3", FileMode.Open));
            bformatter = new BinaryFormatter();

            Console.WriteLine("Reading client Information");

            users = new List<User>();

            while (streamClient.Position < streamClient.Length - 1)
                try
                {
                    User u = (User)bformatter.Deserialize(streamClient);
                    if (u is UserClient)
                        users.Add((UserClient)u);
                    else if (u is Physician)
                        users.Add((Physician)u);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //TODO show error
                }
        }
    }
}