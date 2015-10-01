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
        List<ServerClient> clients = new List<ServerClient>();

        public List<UserClient> userClients = new List<UserClient>();
        public List<Physician> physicians = new List<Physician>();

        public Serverapplication()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new System.Net.Sockets.TcpListener(ip, 130);
            listener.Start();

            //make true but was necessary to test!
            while (false)
            {
                Console.WriteLine("Waiting for Client Connections");
                TcpClient client = listener.AcceptTcpClient();
                clients.Add(new ServerClient(client, this));
            }



            //        public void AddUser(String username, string password, bool isMedical)
            //       {
            //            UserClient user = new UserClient(username, isMedical, password);
            //        }
            //
            //        public string checkLogin(string username, string password)
            //        {
            //            foreach(UserClient user in clients){
            //                if (user.username.Equals(username))
            //                {
            //                    if (user.checkPassword(password))
            //                    {
            //                        //TODO: send user object to client
            //                        return "succesfull logged in";
            //                    }
            //                   return "wrong password!";
            //                }
            //            }
            //            return "wrong username!";
            //        }


        }
        
        public void SaveAllData()
        {
            //save files
            Stream stream = File.Open("clients.osl", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            Console.WriteLine("Writing clients Information");
            foreach(UserClient u in userClients)
                bformatter.Serialize(stream, u);
            stream.Close();

        }

        public void LoadAllData()
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            //Open the file and read values from it.
            Stream stream = File.Open("clients.osl", FileMode.Open);
            bformatter = new BinaryFormatter();

            Console.WriteLine("Reading Employee Information");

            bool printing = true;
            while(printing)
            {
                try
                {
                    UserClient u1 = (UserClient)bformatter.Deserialize(stream);
                    Console.WriteLine(u1.username);
                }
                catch (Exception)
                {
                    Console.WriteLine("finished");
                    stream.Close();
                    printing = false;
                }
                
            }
            
           // UserClient u2 = (UserClient)bformatter.Deserialize(stream);
            
            //Console.WriteLine(u2.username);
            
        }
    }
}
