using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace ServerApplication
{
    class ServerApplication
    {
        static void Main(string[] args)
        {
            new ServerApplication();
        }

        private X509Certificate2 cert = new X509Certificate2(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\NetworkLibrary\Cert\cyclemaster.pfx", "admin", X509KeyStorageFlags.MachineKeySet);
        public List<ServerClient> ConnectedClients { get; }

        public ServerApplication()
        {
            ConnectedClients = new List<ServerClient>();
            //Starting listening to incomming clients: 
            TcpListener listener = new TcpListener(IPAddress.Any, 130);
            listener.Start();
            Console.WriteLine("Waiting for Client Connections");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                // Authenticate cert
                SslStream ssl = new SslStream(client.GetStream());
                ssl.AuthenticateAsServer(cert, true, SslProtocols.Tls12, true);
                string ipAddress = "" + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                Console.WriteLine("Client connected by {0} ({1}) with IP-adress: {2} ", ssl.CipherAlgorithm, ssl.CipherStrength, ipAddress.ToString());
                ConnectedClients.Add(new ServerClient(client, this, ssl));
            }
        }
    }
}
