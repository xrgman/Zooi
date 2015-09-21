using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    class Serverapplication
    {

        public Serverapplication()
        {
            IPAddress localhost;

            bool ipIsOk = IPAddress.TryParse("127.0.0.1", out localhost);
            if (!ipIsOk) { Console.WriteLine("ip adres kan niet geparsed worden."); Environment.Exit(1); }

            TcpListener listener = new System.Net.Sockets.TcpListener(localhost, 8600);
            listener.Start();

            while(true)
            {
                Console.WriteLine("Waiting for Client Connections");
                TcpClient client = listener.AcceptTcpClient();
                Thread thread = new Thread(HandleClientThread);
                thread.Start(client);
            }
        }

        static void HandleClientThread(object obj)
        {
            TcpClient client = obj as TcpClient;
            bool done = false;
            while (!done)
            {
                //data inlezen van client
                byte[] buffer = new byte[1024];
                int totalRead = 0;
                do
                {
                    int read = client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                    totalRead += read;
                } while (client.GetStream().DataAvailable);

                string received = Encoding.ASCII.GetString(buffer, 0, totalRead);
                Console.WriteLine("\nResponse from client: {0}", received);

                //iets doen met data
                byte[] bytes = Encoding.ASCII.GetBytes(received);
            }
            client.Close();
            Console.WriteLine("Connection closed");

        }

    }
}
