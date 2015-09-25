using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

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
            bool done = false;
            int totalReceived = 0;
            while (!done)
            {
                //data inlezen van client
                string received = ReadTextMessage(client);
                Console.WriteLine("recieved");
                ReadMessage(client);
            }
            client.Close();
            Console.WriteLine("Connection closed");

        }

        public static string ReadTextMessage(TcpClient client)
        {

            StreamReader stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            string line = stream.ReadLine();
            return line;
        }

        public static void WriteTextMessage(TcpClient client, string message)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            stream.WriteLine(message);
            stream.Flush();
        }
        public static string ReadMessage(TcpClient client)
        {

            byte[] buffer = new byte[256];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            do
            {
                int read = client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                Console.WriteLine("ReadMessage: " + read);
            } while (client.GetStream().DataAvailable);

            return Encoding.Unicode.GetString(buffer, 0, totalRead);
        }
    }
}
