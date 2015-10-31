using NetworkLibrary;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ClientApplicatie
{
    public class NetworkConnect
    {
        private X509Certificate2Collection certs = new X509Certificate2Collection();
        private SslStream ssl;
        private Network parent;

        public NetworkConnect(Network parent)
        {
            this.parent = parent;
            certs.Add(new X509Certificate2(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\NetworkLibrary\Cert\cyclemaster.pfx", "admin", X509KeyStorageFlags.MachineKeySet));
        }

        public bool ConnectToServer(string ipAdress, int port)
        {
            try
            {
                TcpClient server = new TcpClient(ipAdress, port);
                ssl = new SslStream(server.GetStream(), true, new RemoteCertificateValidationCallback(ValidateCertificate));
                ssl.AuthenticateAsClient("cyclemaster", certs, SslProtocols.Tls12, true);
            }
            catch (Exception e)
            {
                return false; //failed to connect.
            }
            Thread receiveThread = new Thread(new ThreadStart(ReceiveThread));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            return true; //succesfully connected to server.
        }

        public void sendPacket(Packet packet)
        {
            NetworkCommunication.SendPacket(packet, ssl);
        }

        private void ReceiveThread()
        {
            while (true)
            {
                Packet packet = NetworkCommunication.ReadPacket(ssl);
                if (packet != null)
                    packet.handleClientSide(parent);
            }
        }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain certChain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}
