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
using Network;

namespace WindowsFormsApplication1
{
    class NetworkCommunication
    {
        private string ipAdress;
        private int port;
        private TcpClient server;
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
                server = new TcpClient(ipAdress, port);
            }
            catch(Exception e)
            {
                return false; //failed to connect.
            }
            Thread recieveThread = new Thread(new ThreadStart(RecieveThread));
            recieveThread.Start();
            return true; //succesfully connected to server.
        }

        public void sendPacket(Packet packet)
        {
            NetworkFlow.SendPacket(packet, server);
        }

        private void RecieveThread()
        {
           
        }
    }
}
