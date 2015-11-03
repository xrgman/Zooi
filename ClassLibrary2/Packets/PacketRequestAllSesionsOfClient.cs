namespace Network.Packets
{
    class PacketRequestAllSesionsOfClient
    {
        public User user;
        public int ClientID;
        public PacketRequestAllSesionsOfClient(int ClientID)
        {
            this.ClientID = ClientID;
        }
        public PacketRequestAllSesionsOfClient(User user)
        {
            this.user = user;
        }
    }
}
