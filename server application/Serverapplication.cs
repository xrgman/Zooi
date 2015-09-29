using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Net.Security;
using Network;

namespace server_application
{
    class Serverapplication
    {
        List<ServerClient> clients = new List<ServerClient>();
        List<Physician> physicians = new List<Physician>();

        public Serverapplication()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new System.Net.Sockets.TcpListener(ip, 130);
            listener.Start();

            while (true)
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
    }
}
