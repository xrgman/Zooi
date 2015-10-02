using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Network
{
    [Serializable]
    class PacketChatMessage:Packet
    {
        public string message { get; }
        //public Bitmap picture { get; }

        public PacketChatMessage(string Message)
        {
            message = Message;
        }
    }
}
