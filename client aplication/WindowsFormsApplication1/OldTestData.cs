using System.Windows.Forms;
using Network;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    public partial class OldTestData : Form
    {
        private UserClient user;
        private Session currentTest;

        public OldTestData(UserClient user)
        {
            InitializeComponent();
            this.user = user;
            oldTestsList.DataSource = user.GetTests();
        }

        private void oldTestsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            currentTest = user.getSessions()[oldTestsList.SelectedIndex];
            setLabels();
        }

        private void setLabels()
        {
            rpmChart.Series["rpm"].Points.Clear();
            rpmChart.Series["Heart Rate (bpm)"].Points.Clear();

            testDateLabel.Text = "" + currentTest.startedDate;
            durationLabel.Text = "" + currentTest.GetLastMeasurement().time;
            omslagPowerLabel.Text = "" + currentTest.GetLastMeasurement().requested_power;
            omslagTimeLabel.Text = "" + currentTest.GetLastMeasurement().time;

            List<int> rpmList = new List<int>();
            int x = 1;
            foreach(Measurement m in currentTest.measurements)
            {
                rpmChart.Series["rpm"].Points.AddXY(x, m.rpm);
                rpmChart.Series["Heart Rate (bpm)"].Points.AddXY(x, m.pulse);
                x++;
            }
        }
    }
}
