using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Network;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class PatientPage : UserControl
    {

        public UserClient user { get; private set; }

        // Puur om mensen te irriteren met nog meer traagheid.
        private Thread thread;

        private SearchTypes CurrentSearchType { get; set; }

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

            thread = new Thread(new ThreadStart(doStuffsThread));
            thread.Start();
        }

        public PatientPage() : this(new UserClient("John", PasswordHash.HashPassword("banaan")))
        {
        
        }

        public void plot()
        {
            if (chart1.InvokeRequired)
            {
                // Invoke for data integrity, multiple threads use this method
                chart1.Invoke((MethodInvoker)delegate
                {

                    // Series gaan vullen met onze items en waarden
                    IEnumerator<SimItem> enu = user.lastSession().SimItems.GetEnumerator();
                    while (enu.MoveNext())
                    {
                        SimItem item = (SimItem)enu.Current;

                        if (chart1.Series.IsUniqueName(item.Name))
                            chart1.Series.Add(item.Name);

                        // Standard view, No filtering
                        if (CurrentSearchType == SearchTypes.DEFAULT)
                        {
                            chart1.Series[item.Name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line; // set chart Type

                            foreach (KeyValuePair<DateTime, double> entry in item.getData())     // Plot all values the the graph
                                chart1.Series[item.Name].Points.AddXY(entry.Key.ToString("hh:mm:ss"), entry.Value);

                        }
                        // Get data between minValBox value and max box value
                        else if (CurrentSearchType.Equals(SearchTypes.BETWEEN_VALUES))
                        {
                            foreach (KeyValuePair<DateTime, double> entry in item.getDataBetween(Int32.Parse(valueMinBox.Text),
                                                                                                 Int32.Parse(valueMaxBox.Text)))     // Plot all values the the graph
                            {
                                chart1.Series[item.Name].Points.AddXY(entry.Key.ToString("hh:mm:ss"), entry.Value);
                            }
                        }
                        // Between dates
                        else if (CurrentSearchType == SearchTypes.BETWEEN_DATES)
                        {
                            foreach (KeyValuePair<DateTime, double> entry in item.getDataBetween(DateTime.Parse(dateTimePicker1.Text),
                                                                                                 DateTime.Parse(dateTimePicker1.Text)))
                                chart1.Series[item.Name].Points.AddXY(entry.Key.ToString("hh:mm:ss"), entry.Value);
                        }

                        createSerieCheckboxarea();

                    }
                });
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void PatientPage_Load(object sender, EventArgs e)
        {

        }

        private void doStuffsThread()
        {
            while (true)
            {
                user.lastSession().addRandomItems();
                plot();

                Thread.Sleep(1000);
            }
        }
    }
}