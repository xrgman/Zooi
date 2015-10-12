using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class SimItem : IComparable<SimItem>
    {
        // Get int value on specific time
        private Dictionary<DateTime, double> dataSet = new Dictionary<DateTime, double>();

        public Session.signalTypes SignalType;

        // Create Data class without adding data first
        public SimItem(Session.signalTypes signalType)
        {
            this.SignalType = signalType;
        }

        public SimItem(Session.signalTypes signalType, DateTime[] dates, int[] values)
            : this(signalType)
        {
            addData(dates, values);
            dataSet.OrderBy(x => x.Key);
        }

        // Add single unit of data, date will be calculated on time of entry inside application NOT from the bicycle.
        public void addData(int value)
        {
            dataSet.Add(DateTime.Now, value);
        }

        // Combine 2 arrays of DateTime and int (must be synced, dates[0] should be time of value at values[0])
        public void addData(DateTime[] dates, int[] values)
        {
            if (dates.Length == values.Length)
                for (int x = 0; x < dates.Length; x++)
                    dataSet.Add(dates[x], values[x]);
            else
                Console.WriteLine(@"Data out of sync, it's not added");
        }

        public Dictionary<DateTime, double> getData()
        {
            return dataSet;
        }
        // Signaal op een specifiek punt terugkrijgen
        public KeyValuePair<DateTime, double> getData(DateTime min)
        {
            IEnumerator<KeyValuePair<DateTime, double>> tmpEnm = dataSet.Where(data => data.Key == min).GetEnumerator();
            tmpEnm.MoveNext();
            return tmpEnm.Current;
        }
        // Alle signalen tussen 2 datums teruggeven
        public Dictionary<DateTime, double> getDataBetween(DateTime min, DateTime max)
        {
            Dictionary<DateTime, double> tempDic = new Dictionary<DateTime, double>();

            foreach (KeyValuePair<DateTime, double> entry in dataSet)
                if (entry.Key >= min && entry.Key <= max)       // Get all records between min and max
                    tempDic.Add(entry.Key, entry.Value);

            return tempDic;
        }
        // Alle signalen tussen 2 waarden teruggeven
        public Dictionary<DateTime, double> getDataBetween(int min, int max)
        {
            Dictionary<DateTime, double> tempDic = new Dictionary<DateTime, double>();

            foreach (KeyValuePair<DateTime, double> entry in dataSet)
                if (entry.Value >= min && entry.Value <= max)       // Get all record between min and max
                    tempDic.Add(entry.Key, entry.Value);

            return tempDic;
        }

        public double lastMeasurement()
        {
            return dataSet.Values.Last();
        }

        // Sort simitems by name
        public int CompareTo(SimItem sim)
        {
            return this.SignalType.ToString().CompareTo(sim.SignalType.ToString());
        }
    }
}
