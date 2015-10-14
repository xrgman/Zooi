using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Network;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class PatientPage : UserControl
    {
        // Bijbehorende gebruiker
        public UserClient user { get; private set; }

        // Thread
        private Thread thread;
        // Wat laat de grafiek zien
        private SearchTypes CurrentSearchType { get; set; }

        private bool hasBeenSetup;

        public enum SearchTypes
        {
            DEFAULT,
            BETWEEN_VALUES,
            BETWEEN_DATES
        }

        public PatientPage(UserClient user)
        {
            this.user = user;
            InitializeComponent();
            CurrentSearchType = SearchTypes.DEFAULT;       // Standaard plotten (Wachten op signalen)
            setupChart();

            thread = new Thread(new ThreadStart(doStuffsThread));
            thread.Start();
        }

        public PatientPage() : this(new UserClient("John", PasswordHash.HashPassword("banaan")))
        {

        }

        // Initiele opzet van de chart
        public void setupChart()
        {
            chart1.Series.Clear();

            // Series gaan vullen met onze items en waarden
            IEnumerator<SimItem> enu = user.lastSession().SimItems.GetEnumerator();
            while (enu.MoveNext())
            {
                SimItem item = (SimItem)enu.Current;

                if (chart1.Series.IsUniqueName(item.SignalType.ToString()))
                    chart1.Series.Add(item.SignalType.ToString());

                // Standard view, No filtering
                if (CurrentSearchType == SearchTypes.DEFAULT)
                {
                    chart1.Series[item.SignalType.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line; // set chart Type

                    foreach (KeyValuePair<DateTime, double> entry in item.getData())     // Plot all values the the graph
                        chart1.Series[item.SignalType.ToString()].Points.AddXY(entry.Key.ToString("hh:mm:ss"), entry.Value);

                }
                // Get data between minValBox value and max box value
                else if (CurrentSearchType.Equals(SearchTypes.BETWEEN_VALUES))
                {
                    chart1.Series[item.SignalType.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    foreach (KeyValuePair<DateTime, double> entry in item.getDataBetween(Int32.Parse(valueMinBox.Text), Int32.Parse(valueMaxBox.Text)))
                    {    // Plot all values the the graph
                        chart1.Series[item.SignalType.ToString()].Points.AddXY(entry.Key.ToString("hh:mm:ss"), entry.Value);
                    }
                }
                // Between dates
                else if (CurrentSearchType == SearchTypes.BETWEEN_DATES)
                {
                    chart1.Series[item.SignalType.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Stock;
                    foreach (KeyValuePair<DateTime, double> entry in item.getDataBetween(DateTime.Parse(startTimePicker.Text),
                                                                                            DateTime.Parse(startTimePicker.Text)))
                        chart1.Series[item.SignalType.ToString()].Points.AddXY(entry.Key.ToString("hh:mm:ss"), entry.Value);
                }

                // Checkboxes bijvoegen
                createSerieCheckboxarea();
                hasBeenSetup = true;
            }
        }

        // Per measurement een plot uitvoeren, in plaats van steeds weer rijen opnieuw erdoorheen jassen
        public void plotMeasurement(Measurement m)
        {
            DateTime t = DateTime.Now;
            
            chart1.Series[Session.signalTypes.PULSE.ToString()].Points.AddXY(t.ToString("hh:mm:ss"), m.pulse);
            chart1.Series[Session.signalTypes.RPM.ToString()].Points.AddXY(t.ToString("hh:mm:ss"), m.rpm);
            chart1.Series[Session.signalTypes.ACTUAL_POWER.ToString()].Points.AddXY(t.ToString("hh:mm:ss"), m.actual_power);
            chart1.Series[Session.signalTypes.REQUESTED_POWER.ToString()].Points.AddXY(t.ToString("hh:mm:ss"), m.requested_power);
            chart1.Series[Session.signalTypes.SPEED.ToString()].Points.AddXY(t.ToString("hh:mm:ss"), m.speed);
            chart1.Series[Session.signalTypes.ENERGY.ToString()].Points.AddXY(t.ToString("hh:mm:ss"), m.energy);
        }

        private void doStuffsThread()
        {
            while (true)
            {
                // Elke seconde nieuwe measurement genereren en invoegen
                Thread.Sleep(1000);
                user.lastSession().addRandomMeasurement();

                // Waarden aan grafiek toevoegen
                if (chart1.InvokeRequired)
                {
                    chart1.Invoke((MethodInvoker)delegate
                    {
                        plotMeasurement(user.lastSession().lastMeasurement());
                    });
                } else
                    plotMeasurement(user.lastSession().lastMeasurement());
            }
        }
    }
}