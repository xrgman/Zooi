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
        public static bool SendPacket(Packet packet, TcpClient destination)
        {
            BinaryFormatter binairyFormatter = new BinaryFormatter();
            NetworkStream stream = destination.GetStream();
            try
            {
                binairyFormatter.Serialize(stream, packet);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public static Packet ReadPacket(TcpClient source)
        {
            BinaryFormatter binairyFormatter = new BinaryFormatter();
            NetworkStream stream = source.GetStream();
            Packet packet;
            try
            { 
                packet = (Packet) binairyFormatter.Deserialize(stream);

            }
            catch (Exception e)
            {
                return null;
            }
            return packet;
        }
    }
}