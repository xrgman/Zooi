using System;

namespace Network
{
    [Serializable]
    public class PacketMeasurement : Packet
    {
        public Measurement measurement { get; }
        public string physicianName { get; }
        public string sessionType { get; }
        public string username { get; }

        public PacketMeasurement(Measurement measurement, string physicianName, string sessionType, string username)
        {
            this.measurement = measurement;
            this.physicianName = physicianName;
            this.sessionType = sessionType;
            this.username = username;
        }

        public override void handleServerSide(ServerInterface serverInterface)
        {
            serverInterface.ReceiveMeasurement(measurement, physicianName, sessionType, username);
        }
    }
}
