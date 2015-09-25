using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using WindowsFormsApplication1;
using server_application;
using System.Net.Security;

namespace server_application
{
    class Serverapplication
    {
        List<Measurement> data = new List<Measurement>();
        public Serverapplication()
        {
            IPAddress ip = IPAddress.Parse("145.48.118.136");
            TcpListener listener = new System.Net.Sockets.TcpListener(ip, 8600);
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

     
    }
}
