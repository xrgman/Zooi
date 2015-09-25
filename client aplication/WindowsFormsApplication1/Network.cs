using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Runtime;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public class Network
    {
        private NetworkCommunication networkCommunication;
        public string status { get; set; }

        public Network(string ipAdress, int port)
        {
            networkCommunication = new NetworkCommunication(ipAdress, port, this);
            if (networkCommunication.ConnectToServer())
                status = "Connected";
            else
                status = "Can't connect to: " + ipAdress + ":" + port;
        }

        public void login(string username, string password)
        {
            //send login
        }

        public static string ReadTextMessage(SslStream sslStream)
        {

            StreamReader stream = new StreamReader(sslStream, Encoding.ASCII);
            string line = stream.ReadLine();
            return line;
        }

        public static void WriteTextMessage(SslStream sslStream, string message)
        {
            var stream = new StreamWriter(sslStream, Encoding.ASCII);
            stream.WriteLine(message);
            stream.Flush();
        }
        public static string ReadMessage(SslStream sslStream, TcpClient client)
        {

            byte[] buffer = new byte[256];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            do
            {
                int read = sslStream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                Console.WriteLine("ReadMessage: " + read);
            } while (client.GetStream().DataAvailable);

            return Encoding.Unicode.GetString(buffer, 0, totalRead);
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

    }
}
