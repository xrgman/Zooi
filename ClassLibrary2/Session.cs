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

        // One for each signaltype
        private SimItem actual_power,
                        distance,
                        energy,
                        pulse,
                        requested_power,
                        rpm,
                        speed,
                        time;

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
            StartTime = DateTime.Now;

            // Add and instantiate all data (Every signalstype gets it's own simItem class)
            simItems.Add(pulse = new SimItem(signalTypes.PULSE));
            simItems.Add(rpm = new SimItem(signalTypes.RPM));
            simItems.Add(speed = new SimItem(signalTypes.SPEED));
            simItems.Add(time = new SimItem(signalTypes.TIME));
            simItems.Add(distance = new SimItem(signalTypes.DISTANCE));
            simItems.Add(requested_power = new SimItem(signalTypes.REQUESTED_POWER));
            simItems.Add(energy = new SimItem(signalTypes.ENERGY));
            simItems.Add(actual_power = new SimItem(signalTypes.ACTUAL_POWER));

            simItems.Sort();
        }

        // Random measurement maken/toevoegen
        public void addRandomItems()
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

        public List<SimItem> SimItems
        {
            get { return simItems; }
        }

        public SimItem getSimItem(signalTypes sType)
        {
            switch (sType)
            {
                case signalTypes.ACTUAL_POWER: return actual_power;
                case signalTypes.DISTANCE: return distance;
                case signalTypes.ENERGY: return energy;
                case signalTypes.PULSE: return actual_power;
                case signalTypes.REQUESTED_POWER: return requested_power;
                case signalTypes.RPM: return rpm;
                case signalTypes.SPEED: return speed;
                default: return null;
            }
        }

        public void addSignal(signalTypes sType, int value)
        {
            switch (sType)
            {
                case signalTypes.ACTUAL_POWER:
                    actual_power.addData(value); break;
                case signalTypes.DISTANCE:
                    distance.addData(value); break;
                case signalTypes.ENERGY:
                    energy.addData(value); break;
                case signalTypes.PULSE:
                    pulse.addData(value); break;
                case signalTypes.REQUESTED_POWER:
                    requested_power.addData(value); break;
                case signalTypes.RPM:
                    rpm.addData(value); break;
                case signalTypes.SPEED:
                    speed.addData(value); break;
            }
        }

        public void addMeasurement(Measurement m)
        {
            // Fill SimItems with measurement data
            actual_power.addData(m.actual_power);
            distance.addData(m.distance);
            energy.addData(m.energy);
            pulse.addData(m.pulse);
            requested_power.addData(m.requested_power);
            rpm.addData(m.rpm);
            speed.addData(m.speed);
        }

        public Measurement getMeasurement(DateTime fromTime)
        {
            return new Measurement((int)pulse.getData(fromTime).Value,
                                    (int)rpm.getData(fromTime).Value,
                                    (int)speed.getData(fromTime).Value,
                                    (int)distance.getData(fromTime).Value,
                                    (int)requested_power.getData(fromTime).Value,
                                    (int)energy.getData(fromTime).Value,
                                    "0",
                                    (int)actual_power.getData(fromTime).Value);
        }

        public Measurement lastMeasurement()
        {
            // Elke methode 'lastMeasurement()' vanuit simItems aanroepen 
            return new Measurement((int)pulse.lastMeasurement(),
                                   (int)rpm.lastMeasurement(),
                                   (int)speed.lastMeasurement(),
                                   (int)distance.lastMeasurement(),
                                   (int)requested_power.lastMeasurement(),
                                   (int)energy.lastMeasurement(),
                                   "0",
                                   (int)speed.lastMeasurement());
        }

        //* Compare session by startTime
        public int CompareTo(Session s)
        {
            return this.StartTime.CompareTo(s.StartTime);
        }

    }
}