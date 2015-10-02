using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    [Serializable]
    public class PacketLogin : Packet
    {
        public String username { get; set; }
        public String password { get; set; }

        public override void handleServerSide(ServerInterface serverInterface)
        {
            serverInterface.login(username, password);
            Console.WriteLine(username+password);
        }
        public override string checkContent()
        {
            return "username: " + username + " password: " + password;
        }
    }
}
