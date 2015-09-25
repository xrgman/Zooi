using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class FormClient : Form
    {
        //statusLabel, modelLabel, versionLabel, timeLabel, actualPowerLabel, heartBeatLabel, rpmLabel, speedLabel, distanceLabel, energyLabel, requestedPowerLabel

        private Bike bike;
        private Network network;

        public FormClient(Network network)
        {
            InitializeComponent();
            this.network = network;
        }

        private void BComConnect_Click(object sender, EventArgs e)
        {
            CommForm commForm = new CommForm();
            DialogResult dialogResult = commForm.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                bike = new Bike(commForm.getCommport);
                modelLabel.Text = bike.GetModel();
                versionLabel.Text = bike.GetVersionNumber();
                statusLabel.Text = bike.GetStatus();
                Thread refreshThread = new Thread(new ThreadStart(RefreshThread));
                refreshThread.Start();
            }
            commForm.Dispose();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            bike.Reset();
        }


        //Safe setting of label methodes: 
        delegate void SetTextCallback(string text);

        private void SetActualPowerLabel(string actualPower)
        {
            if (this.actualPowerLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetActualPowerLabel);
                this.Invoke(d, new object[] { actualPower });
            }
            else
            {
                this.actualPowerLabel.Text = actualPower;
            }
        }

        private void SetTimeLabel(string time)
        {
            if (this.timeLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTimeLabel);
                this.Invoke(d, new object[] { time });
            }
            else
            {
                this.timeLabel.Text = time;
            }
        }

        private void SetHeartBeatLabel(string heartBeat)
        {
            if (this.heartBeatLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetHeartBeatLabel);
                this.Invoke(d, new object[] { heartBeat });
            }
            else
            {
                this.heartBeatLabel.Text = heartBeat;
            }
        }

        private void SetRpmLabel(string rpm)
        {
            if (this.rpmLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRpmLabel);
                this.Invoke(d, new object[] { rpm });
            }
            else
            {
                this.rpmLabel.Text = rpm;
            }
        }

        private void SetDistanceLabel(string distance)
        {
            if (this.distanceLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetDistanceLabel);
                this.Invoke(d, new object[] { distance });
            }
            else
            {
                this.distanceLabel.Text = distance;
            }
        }

        private void SetEnergyLabel(string energy)
        {
            if (this.energyLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetEnergyLabel);
                this.Invoke(d, new object[] { energy });
            }
            else
            {
                this.energyLabel.Text = energy;
            }
        }

        private void SetRequestedPowerLabel(string requestedPower)
        {
            if (this.requestedPowerLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetRequestedPowerLabel);
                this.Invoke(d, new object[] { requestedPower });
            }
            else
            {
                this.requestedPowerLabel.Text = requestedPower;
            }
        }

        private void SetSpeedLabel(string speed)
        {
            if (this.speedLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetSpeedLabel);
                this.Invoke(d, new object[] { speed });
            }
            else
            {
                this.speedLabel.Text = speed;
            }
        }

        private void RefreshThread()
        {
            while(statusLabel.Text != "Error: connection lost")
            {
                statusLabel.Text = bike.GetStatus();
                Measurement measurement = bike.getMeasurement();
                SetActualPowerLabel(measurement.actual_power.ToString());
                SetTimeLabel(measurement.time);
                SetHeartBeatLabel(measurement.pulse.ToString());
                SetRpmLabel(measurement.rpm.ToString());
                SetSpeedLabel(measurement.speed.ToString());
                SetDistanceLabel(measurement.distance.ToString());
                SetEnergyLabel(measurement.energy.ToString());
                SetRequestedPowerLabel(measurement.requested_power.ToString());
                Thread.Sleep(1000);
            }
        }

        private void FormClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            //terminating threads, not done yet!
            statusLabel.Text = "Error: connection lost";
        }
    }
}
