using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net.Security;

namespace WindowsFormsApplication1
{
    class NetworkCommunication
    {
        private string ipAdress;
        private int port;
        private TcpClient client;
        private Networkconnect parent;
        private SslStream stream;

        public NetworkCommunication(string ipAdress, int port, Networkconnect parent)
        {
            this.ipAdress = ipAdress;
            this.port = port;
            this.parent = parent;
        }

        public bool ConnectToServer()
        {
            try
            {
                client = new TcpClient(ipAdress, port);
                stream = new SslStream(client.GetStream());
            }
            catch(Exception e)
            {
                return false;
            }
            Thread recieveThread = new Thread(new ThreadStart(RecieveThread));
            recieveThread.Start();
            return true;
        }

        public SslStream getStream()
        {
            return stream;
        }

        public void SendMessage(string message)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
            writer.WriteLine(message);
            writer.Flush();
        }

        private void RecieveThread()
        {
            StreamReader reader = new StreamReader(stream, Encoding.Unicode);
            string message;
            while(parent.status != "Error, lost connection to the server")
            {
                try
                {
                     message = reader.ReadLine();
                    //Do stuff:
                }
                catch(Exception)
                {
                    parent.status = "Error, lost connection to the server";
                }
            }
        }
    }
}
