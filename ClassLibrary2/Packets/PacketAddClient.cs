using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Network
{
    [Serializable]
    public class PacketAddClient : Packet
    {
        public User user { get; set; }

        public PacketAddClient(User user)
        {
            this.user = user;
        }

        public override void handleServerSide(ServerInterface serverInterface)
        {
            serverInterface.AddUser(user);
        }
    }
}
