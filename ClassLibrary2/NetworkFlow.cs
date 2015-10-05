using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net.Security;

namespace Network
{
    public class NetworkFlow
    {
        public static bool SendPacket(Packet packet, SslStream ssl)
        {
            BinaryFormatter binairyFormatter = new BinaryFormatter();

            try
            {
                binairyFormatter.Serialize(ssl, packet);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public static Packet ReadPacket(SslStream ssl)
        {
            BinaryFormatter binairyFormatter = new BinaryFormatter();
            Packet packet;
            try
            {
                packet = (Packet) binairyFormatter.Deserialize(ssl);
            }
            catch (Exception e)
            {
                return null;
            }
            return packet;
        }
    }
}