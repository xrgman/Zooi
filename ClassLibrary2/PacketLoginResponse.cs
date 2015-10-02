using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    [Serializable]
    public class PacketLoginResponse : Packet
    {
        public bool loginOk { get; set; }

        public override void handleClientSide(ClientInterface clientInterface)
        {
            clientInterface.loginResponse(loginOk);
        }

        public override string checkContent()
        {
            return "loginOk: " + loginOk;
        }
    }
}
