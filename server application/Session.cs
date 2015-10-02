using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace server_application
{
    [Serializable]
    public class Session 
    {
        public DateTime startedDate { get; private set; }

        public List<Measurement> measurements = new List<Measurement>();


        public Session(DateTime startedDate, UserClient userClient)
        {
            this.startedDate = startedDate;
        }

        //deserialize method
        public Session(SerializationInfo info, StreamingContext ctxt)
        {
            measurements = (List<Measurement>)info.GetValue("measurement", typeof(List<Measurement>));
            startedDate = (DateTime)info.GetValue("startedDate", typeof(DateTime));
            //sessions = (List<Session>)info.GetValue("sessions", typeof(List<>));

        }

        public void AddMeasurement(Measurement measurement)
        {
            measurements.Add(measurement);
        }


        public List<Measurement> getMeasurement()
        {
            return measurements;
        }

        //serialize method
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("measurements", measurements);
            info.AddValue("startedDate", startedDate);
        }


    }
}
