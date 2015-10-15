using System;

namespace Network
{
    [Serializable]
    public class PacketMeasurement : Packet
    {
        public Measurement measurement { get; }
        public string physicianName { get; }

        public PacketMeasurement(Measurement measurement, string physicianName)
        {
            this.measurement = measurement;
            this.physicianName = physicianName;
        }

        public override void handleServerSide(ServerInterface serverInterface)
        {
            serverInterface.ReceiveMeasurement(measurement, physicianName);
        }
    }
}
