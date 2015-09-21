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
                StreamReader stream = new StreamReader(client.GetStream(), Encoding.ASCII);
                string line = stream.ReadLine();
                Console.WriteLine("Received: {0}", line);
                var streamwriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
                done = line.Equals("bye");
                if (done)
                {
                    streamwriter.WriteLine("Bye");
                    streamwriter.Flush();
                }
                else
                {
                    streamwriter.WriteLine("ok");
                    streamwriter.Flush();
                }

            }
            client.Close();
            Console.WriteLine("Connection closed");

        }

    }
}
