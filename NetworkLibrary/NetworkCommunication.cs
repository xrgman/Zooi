using System;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetworkLibrary
{
    public class NetworkCommunication
    {
        //Send packet:
        public static bool SendPacket(Packet packet, SslStream ssl)
        {
            BinaryFormatter binairyFormatter = new BinaryFormatter();
            try
            {
                binairyFormatter.Serialize(ssl, packet);
                ssl.Flush();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        //Recieve packet:
        public static Packet ReadPacket(SslStream ssl)
        {
            BinaryFormatter binairyFormatter = new BinaryFormatter();
            Packet packet;
            try
            {
                packet = (Packet)binairyFormatter.Deserialize(ssl);
                ssl.Flush();
            }
            catch (Exception e)
            {
                return null;
            }
            return packet;
        }
    }
}
