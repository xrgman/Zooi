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

namespace server_application
{
    [Serializable]
    public class Serverapplication
    { 
        private List<ServerClient> ConnectedClients { get; }

        public List<User> users = new List<User>();

        public Serverapplication()
        {
            ConnectedClients = new List<ServerClient>();

            //add test users obviously for testing
            users.Add(new UserClient("Henk", PasswordHash.HashPassword("banaan")));
            users.Add(new Physician("Jaap", PasswordHash.HashPassword("appel")));

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ip, 130);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for Client Connections");
                TcpClient client = listener.AcceptTcpClient();
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
    }
}