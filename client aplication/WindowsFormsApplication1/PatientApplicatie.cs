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
using Network;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1
{
    public partial class FormClient : Form
    {
        //statusLabel, modelLabel, versionLabel, timeLabel, actualPowerLabel, heartBeatLabel, rpmLabel, speedLabel, distanceLabel, energyLabel, requestedPowerLabel

        private Bike bike;
        private Networkconnect network;
        private bool isPhysician = false;
        private string username;
        private List<User> users;
        private User currentUser;

        public FormClient(Networkconnect network, bool isPhysician, string username)
        {
            InitializeComponent();
            this.isPhysician = isPhysician;
            this.network = network;
            this.username = username;
            if(!isPhysician) //Client:
            {
                pwrTxtBox.Hide();
                distanceTxtBox.Hide();
                timeTxtBox.Hide();
                label5.Hide();
                label6.Hide();
                label7.Hide();
                sendButton.Hide();
                broadCastButton.Hide();
                connectedUsers.Hide();
            }
            else //Specialist:
            {
                BComConnect.Hide();
                resetButton.Hide();
                //Getting all connected users:
                users = network.GetAllConnectedUsers();
                if(users.Count > 0)
                    currentUser = users.First();
                FillUserComboBox();
                Thread physicianThread = new Thread(new ThreadStart(ResfreshThreadPhysician));
                physicianThread.IsBackground = true;
                physicianThread.Start();
            }
        }

        private void BComConnect_Click(object sender, EventArgs e)
        {
            CommForm commForm = new CommForm();
            DialogResult dialogResult = commForm.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                bike = new Bike(commForm.getCommport);
               // modelLabel.Text = bike.GetModel();
                while(modelLabel.Text.Equals("ERROR") || modelLabel.Text.Equals(""))
                    modelLabel.Text = bike.GetModel();
                //versionLabel.Text = bike.GetVersionNumber();
                while(versionLabel.Text.Equals("ERROR") || versionLabel.Text.Equals(""))
                    versionLabel.Text = bike.GetVersionNumber();
                statusLabel.Text = bike.GetStatus();
                Thread refreshThread = new Thread(new ThreadStart(RefreshThread));
                refreshThread.IsBackground = true;
                refreshThread.Start();
            }
            commForm.Dispose();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            bike.Reset();
        }

        private void  broadCastButton_Click(object sender, EventArgs e)
        {
            
        }

        private void FillUserComboBox()
        {
            foreach(User user in users)
            {
                connectedUsers.Items.Add(user);
            }
            connectedUsers.SelectedIndex = connectedUsers.Items.IndexOf(currentUser);
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

        private void SetStatusLabel(string status)
        {
            if (this.statusLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusLabel);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                this.statusLabel.Text = status;
            }
        }

        private void RefreshThread()
        {
            do
            {
                SetStatusLabel(bike.GetStatus());
                Measurement measurement = bike.GetMeasurement();
                if (measurement != null)
                {
                    SetActualPowerLabel(measurement.actual_power.ToString());
                    SetTimeLabel(measurement.time);
                    SetHeartBeatLabel(measurement.pulse.ToString());
                    SetRpmLabel(measurement.rpm.ToString());
                    SetSpeedLabel(measurement.speed.ToString());
                    SetDistanceLabel(measurement.distance.ToString());
                    SetEnergyLabel(measurement.energy.ToString());
                    SetRequestedPowerLabel(measurement.requested_power.ToString());
                }
                //Set values from doctor:


                //Send measurement to the server

                
                Thread.Sleep(1000);
            }
            while (statusLabel.Text != "Error: connection lost");
        }

        private void FormClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            //terminating threads, not done yet!
            
        }

        /// <summary>
        /// For client only! this gets send to rhbike;
        /// </summary>
        /// <param name="power"></param>
        /// <param name="time"></param>
        /// <param name="distance"></param>
        public void setBikeValues(string power, string time, string distance)
        {
            int powerNumber;
            int distanceNumber;
            if (pwrTxtBox.Text != "")
                if (Int32.TryParse(pwrTxtBox.Text, out powerNumber))
                    bike.SetPower(powerNumber);
            if (timeTxtBox.Text != "")
                bike.setTime(Int32.Parse(timeTxtBox.Text));
            if (distanceTxtBox.Text != "")
                if (Int32.TryParse(distanceTxtBox.Text, out distanceNumber))
                    bike.SetDistance(distanceNumber);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            //stuur naar server en die stuurt naar client;
            if(pwrTxtBox.BackColor != Color.Red && timeTxtBox.BackColor != Color.Red && distanceTxtBox.BackColor != Color.Red)
                network.sendBikeValues(pwrTxtBox.Text, timeTxtBox.Text, distanceTxtBox.Text); 
        }

        /// <summary>
        /// Checks if entered value is correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pwrTxtBox_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (!Int32.TryParse(pwrTxtBox.Text, out value) && pwrTxtBox.Text != "")
                pwrTxtBox.BackColor = Color.Red;
            else
            {
                if(value >= 0 && value <= 400)
                    pwrTxtBox.BackColor = Color.White;
                else
                    pwrTxtBox.BackColor = Color.Red;
            }     
        }

        private void timeTxtBox_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (!Int32.TryParse(timeTxtBox.Text, out value) && timeTxtBox.Text != "")
                timeTxtBox.BackColor = Color.Red;
            else
                timeTxtBox.BackColor = Color.White;
        }

        private void distanceTxtBox_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (!Int32.TryParse(distanceTxtBox.Text, out value) && distanceTxtBox.Text != "")
                distanceTxtBox.BackColor = Color.Red;
            else
                distanceTxtBox.BackColor = Color.White;
        }

        private void BSend_Click(object sender, EventArgs e)
        {
            network.sendChatMessage(TChatSend.Text, username, "Jaap");
            RTBChatText.Text += username + ": " +  TChatSend.Text + System.Environment.NewLine;
        }
    

        private void connectedUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentUser = (User) connectedUsers.SelectedItem;
            RefreshFields();
            TChatView.Text = "current user: " + currentUser;
        }

        private void RefreshFields()
        {
            //Set model en version fields
        }

        private void ResfreshThreadPhysician()
        {
            while(true)
            {
                if(currentUser != null)
                {
                    Measurement measurement = null;
                    try
                    {
                        measurement = ((UserClient)currentUser).getLastSession().GetLastMeasurement();
                    }
                    catch(NullReferenceException e)
                    {

                    }
                    if (measurement != null)
                    {
                        SetActualPowerLabel(measurement.actual_power.ToString());
                        SetTimeLabel(measurement.time);
                        SetHeartBeatLabel(measurement.pulse.ToString());
                        SetRpmLabel(measurement.rpm.ToString());
                        SetSpeedLabel(measurement.speed.ToString());
                        SetDistanceLabel(measurement.distance.ToString());
                        SetEnergyLabel(measurement.energy.ToString());
                        SetRequestedPowerLabel(measurement.requested_power.ToString());
                    }
                    //get new user data;
                }
                Thread.Sleep(1000);
            }
        }


    } 
}
