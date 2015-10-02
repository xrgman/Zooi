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
            }
            catch(Exception e)
            {
                return false;
            }
            Thread recieveThread = new Thread(new ThreadStart(RecieveThread));
            recieveThread.Start();
            return true;
        }

        public NetworkStream getStream()
        {
                return client.GetStream();
        }

        public void SendMessage(string message)
        {
            StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.Unicode);
            writer.WriteLine(message);
            writer.Flush();
        }

        private void RecieveThread()
        {
            StreamReader reader = new StreamReader(client.GetStream(), Encoding.Unicode);
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
