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

namespace server_application
{
    [Serializable]
    public class Serverapplication
    { 
        private List<ServerClient> ConnectedClients { get; }

        public List<UserClient> userClients { get; set; }
        public List<Physician> physicians { get; set; }

        public Serverapplication()
        {
            ConnectedClients = new List<ServerClient>();
            userClients = new List<UserClient>();
            physicians = new List<Physician>();
            //test user:
            userClients.Add(new UserClient("Henk", "banaan"));
            //test Physician:
            physicians.Add(new Physician("Jaap", "appel"));
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ip, 130);
            listener.Start();

            //make true but was necessary to test!
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
            foreach (UserClient u in userClients)
                bformatter.Serialize(streamClient, u);
            streamClient.Close();

            //save physicianfiles
            Stream streamPhysician = File.Open("physicians.a3", FileMode.Create);
            BinaryFormatter bformatter1 = new BinaryFormatter();
            Console.WriteLine("Writing physicians Information");
            foreach (Physician p in physicians)
                bformatter1.Serialize(streamPhysician, p);
            streamPhysician.Close();
        }

        public void LoadAllData()
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            //Open the file and read values from client.
            Stream streamClient = File.Open("clients.a3", FileMode.Open);
            bformatter = new BinaryFormatter();

            Console.WriteLine("Reading client Information");

            userClients = new List<UserClient>();

            while (streamClient.Position < streamClient.Length - 1)
                try
                {
                    userClients.Add((UserClient)bformatter.Deserialize(streamClient));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //TODO show error
                }

            //Open the file and read values from physician.
            Stream streamPhysician = File.Open("physicians.a3", FileMode.Open);
            bformatter = new BinaryFormatter();

            physicians = new List<Physician>();
            Console.WriteLine("Reading physicians Information");
            while (streamPhysician.Position < streamPhysician.Length - 1)
            {
                try
                {
                    physicians.Add((Physician)bformatter.Deserialize(streamPhysician));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //TODO show error
                }
            }
        }
    }
}