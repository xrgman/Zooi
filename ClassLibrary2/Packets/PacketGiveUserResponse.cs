using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    [Serializable]
    public class PacketGiveUserResponse : Packet
    {
        private User user { get; }
        private List<User> users { get; }

        public PacketGiveUserResponse(User user)
        {
            this.user = user;
        }

        public PacketGiveUserResponse(List<User> users)
        {
            this.users = users;
        }

        public override void handleClientSide(ClientInterface clientInterface)
        {
            if (user != null)
                clientInterface.GiveUserResponse(user);
            else
                clientInterface.GiveUserResponse(users); 
        }
    }
}
