using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Packets
{
    [Serializable]
    class PacketRequestMeasurements:Packet
    {
        string client;
        DateTime date;

        public PacketRequestMeasurements(string client,DateTime date)
        {
            this.client = client;
            this.date = date;
        }
        
    }
}
