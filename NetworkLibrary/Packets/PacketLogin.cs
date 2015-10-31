using System;

namespace NetworkLibrary.Packets
{
    [Serializable]
    public class PacketLogin : Packet
    {
        private String username;
        private String password;
        private bool loginOk;

        public PacketLogin(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public PacketLogin(bool loginOk)
        {
            this.loginOk = loginOk;
        }

        public override void handleClientSide(ClientInterface clientInterface)
        {
            clientInterface.LoginResponse(loginOk);
        }

        public override void handleServerSide(ServerInterface serverInterface)
        {
            serverInterface.Login(username, password);
        }
    }
}
