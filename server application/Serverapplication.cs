using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Net.Security;

namespace server_application
{
    class Serverapplication
    {
        List<UserClient> clients = new List<UserClient>();
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
                Thread thread = new Thread(HandleClientThread);
                thread.Start(client);
            }
        }

        public static void HandleClientThread(object obj)
        {
            TcpClient client = obj as TcpClient;
            SslStream sslStream;
            sslStream = new SslStream(client.GetStream());
            bool done = false;
            while (!done)
            {
                //data inlezen van client
                    string received = Network.ReadTextMessage(sslStream);
                    Console.WriteLine("recieved");
                    Network.ReadMessage(sslStream, client);
            }
            client.Close();
            Console.WriteLine("Connection closed");

        }

        public void AddUser(String username, string password, bool isMedical)
        {
            UserClient user = new UserClient(username, isMedical, password);
        }

        public string checkLogin(string username, string password)
        {
            foreach(UserClient user in clients){
                if (user.username.Equals(username))
                {
                    if (user.checkPassword(password))
                    {
                        //TODO: send user object to client
                        return "succesfull logged in";
                    }
                    return "wrong password!";
                }
            }
            return "wrong username!";
        }

     
    }
}
