using System;

namespace NetworkLibrary
{
    [Serializable]
    public class Packet
    {
        public virtual void handleClientSide(ClientInterface clientInterface)
        { }
        public virtual void handleServerSide(ServerInterface serverInterface)
        { }
    }
}
