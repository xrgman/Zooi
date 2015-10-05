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

        public Serverapplication()
        {
            ConnectedClients = new List<ServerClient>();

            //add test users obviously for testing
            users.Add(new UserClient("Henk", PasswordHash.HashPassword("banaan")));
            users.Add(new Physician("Jaap", PasswordHash.HashPassword("appel")));

            TcpListener listener = new TcpListener(IPAddress.Loopback, 130);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for Client Connections");
                TcpClient client = listener.AcceptTcpClient();
                // Authenticate cert
                SslStream ssl = new SslStream(client.GetStream());
                ssl.AuthenticateAsServer(cert, true, SslProtocols.Tls12, true);

                string ipAddress = "" + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                Console.WriteLine("Client connected from: {0}", ipAddress);
                ConnectedClients.Add(new ServerClient(client, this));
            }
        }

        public List<ServerClient> getConnectedClients()
        {
            return ConnectedClients;
        }

        public void SaveAllData()
        {
            //save clientfiles
            FileStream streamClient = File.Open("clients.a3", FileMode.Create);
            Console.WriteLine(streamClient.Name);
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
            Stream streamClient = File.Open("clients.a3", FileMode.Open);
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
        public void broadCast(Packet packet)
        {
            foreach (ServerClient serverClient in ConnectedClients)
                serverClient.sendPacket(packet);
        }
    }
}