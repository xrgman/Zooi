using System;

namespace Network
{
    [Serializable]
   public class PacketGiveUser : Packet
    {
        private string username { get; }
        private bool allUsers { get; }

        //Use * to get all users;
        public PacketGiveUser(string username, bool allUsers)
        {
            this.username = username;
            this.allUsers = allUsers;
        }

        public override void handleServerSide(ServerInterface serverInterface)
        {
            serverInterface.GiveUser(username, allUsers);
        }
    }
}
