using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Network
{
    // SimData: Bevat de klassen 'simItem' waarin de ontvangen waarden opgeslagen worden
    public class Session : IComparable<Session>
    {
        // data
        private List<SimItem> simItems = new List<SimItem>();

        private TimeSpan sessionTime;

        public List<SimItem> SimItems { get { return simItems; } private set { } }

        // Enum for easy signal (Ex. rpm/pulse/etc..)
        public enum signalTypes
        {
            ACTUAL_POWER,
            DISTANCE,
            ENERGY,
            PULSE,
            REQUESTED_POWER,
            RPM,
            SPEED,
            TIME
        };

        public DateTime StartTime { get; set; }

        public Session()
        {
            // We beginnen nu
            StartTime = DateTime.Now;
            // Voor elk signaal type een sim item aanmaken
            foreach (var str in Enum.GetValues(typeof(signalTypes))){
                simItems.Add(new SimItem((signalTypes)(str)));      
            }
            
        }

        public SimItem getSimItem(signalTypes sType)
        {   // Bijbehorende simitem zoeken
            return simItems.Where(item => item.SignalType == sType).First();
        }

        public void addSignal(signalTypes sType, int value)
        {   // Toevoegen aan simItem
            getSimItem(sType).addData(value);
        }

        public void addMeasurement(Measurement m)
        {   // Fill SimItems with measurement data
            addSignal(signalTypes.ACTUAL_POWER, m.actual_power);
            addSignal(signalTypes.DISTANCE, m.distance);
            addSignal(signalTypes.ENERGY, m.energy);
            addSignal(signalTypes.PULSE, m.pulse);
            addSignal(signalTypes.RPM, m.rpm);
            addSignal(signalTypes.SPEED, m.speed);
            addSignal(signalTypes.REQUESTED_POWER, m.requested_power);
        }

        public Measurement getMeasurement(DateTime fromTime)
        {   // Measurement van bepaalde tijd opvragen
            return new Measurement((int)getSimItem(signalTypes.PULSE).getData(fromTime).Value,
                                    (int)getSimItem(signalTypes.RPM).getData(fromTime).Value,
                                    (int)getSimItem(signalTypes.SPEED).getData(fromTime).Value,
                                    (int)getSimItem(signalTypes.DISTANCE).getData(fromTime).Value,
                                    (int)getSimItem(signalTypes.REQUESTED_POWER).getData(fromTime).Value,
                                    (int)getSimItem(signalTypes.ENERGY).getData(fromTime).Value,
                                    "0",
                                    (int)getSimItem(signalTypes.ACTUAL_POWER).getData(fromTime).Value);
        }

        public Measurement lastMeasurement()
        {   // Elke methode 'lastMeasurement()' vanuit simItems aanroepen en in een nieuwe measurement gestopt
            return new Measurement((int)getSimItem(signalTypes.PULSE).lastMeasurement(),
                                    (int)getSimItem(signalTypes.RPM).lastMeasurement(),
                                    (int)getSimItem(signalTypes.SPEED).lastMeasurement(),
                                    (int)getSimItem(signalTypes.DISTANCE).lastMeasurement(),
                                    (int)getSimItem(signalTypes.REQUESTED_POWER).lastMeasurement(),
                                    (int)getSimItem(signalTypes.ENERGY).lastMeasurement(),
                                    "0",
                                    (int)getSimItem(signalTypes.ACTUAL_POWER).lastMeasurement());
        }

        // Random measurement maken/toevoegen
        public void addRandomMeasurement()
        {
            Random random = new Random();
            addMeasurement(new Measurement((int)(random.NextDouble() * 100),
                                            (int)(random.NextDouble() * 80),
                                            (int)(random.NextDouble() * 60),
                                            (int)(random.NextDouble() * 40),
                                            (int)(random.NextDouble() * 30),
                                            (int)(random.NextDouble() * 20),
                                            "hoi",
                                            (int)(random.NextDouble() * 10)));
        }

        public void start()
        {

        }

        public void pause()
        {

        }

        public void stop()
        {

        }

        //* Compare session by startTime
        public int CompareTo(Session s)
        {
            return this.StartTime.CompareTo(s.StartTime);
        }

    }
}