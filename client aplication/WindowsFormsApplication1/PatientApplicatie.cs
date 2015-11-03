using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Network;

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
            network.SetParent(this);
            this.isPhysician = isPhysician;
            this.network = network;
            this.username = username;
            if(!isPhysician) //Client:
            {
                currentUser = network.getUser(username);
                pwrTxtBox.Hide();
                distanceTxtBox.Hide();
                timeTxtBox.Hide();
                label5.Hide();
                label6.Hide();
                label7.Hide();
                sendButton.Hide();
                broadCastButton.Hide();
                connectedUsers.Hide();
                newClient.Hide();
                dropDownTest.HideDropDown();
                bikeImprovementToolStripMenuItem.HideDropDown();
            }
            else //Specialist:
            {
                BComConnect.Hide();
                resetButton.Hide();
                //Getting all connected users:
                users = network.GetAllConnectedUsers(username);
                if (users != null)
                {
                    if (users.Count > 0)
                            currentUser = users.First();
                    FillUserComboBox();
                }
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

        private void broadCastButton_Click(object sender, EventArgs e) {
            if (isPhysician)
            {
                foreach(User u in users)
                    network.sendChatMessage(TChatSend.Text, username, u.username);
                RTBChatText.Text += "BROADCAST: " + username + ": " + TChatSend.Text + System.Environment.NewLine;


            } else
                MessageBox.Show("cliënten kunnen geen broadcastbericht versturen!");

            
            RTBChatText.Text += username + ": " + TChatSend.Text + System.Environment.NewLine;
        }
        delegate void SetUserComboBox();

        public void FillUserComboBox()
        {
            if (this.connectedUsers.InvokeRequired)
            {
                SetUserComboBox d = new SetUserComboBox(FillUserComboBox);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.connectedUsers.Items.Clear();
                foreach (User user in users)
                {
                    //if(user.isOnline)
                        this.connectedUsers.Items.Add(user);
                }
                if (currentUser != null)
                {
                    this.connectedUsers.SelectedIndex = this.connectedUsers.Items.IndexOf(currentUser);
                }
                else if (connectedUsers.Items.Count > 0)
                    this.connectedUsers.SelectedIndex = connectedUsers.Items.IndexOf(0);
            }
        }


        //Safe setting of label methodes: 
        delegate void SetTextCallback(Label label, string text);

        private void SetLabelText(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                try
                {
                    SetTextCallback d = new SetTextCallback(SetLabelText);
                    this.Invoke(d, new object[] { label, text });
                }catch(Exception e)
                {
                    //TODO catch die shit
                }
               
            }
            else
            {
                label.Text = text;
            }
        }

        private void RefreshThread()
        {
            network.sendMeasurement(null, ((UserClient)currentUser).physician, "Create", username);
            do
            {
                //Set the status of connection:
                SetLabelText(statusLabel,bike.GetStatus());
                //Get latest measurement: 
                Measurement measurement = bike.GetMeasurement();
                if (measurement != null)
                {
                    SetLabelText(actualPowerLabel,measurement.actual_power.ToString() + " Watt");
                    SetLabelText(timeLabel, measurement.time);
                    SetLabelText(heartBeatLabel,measurement.pulse.ToString()+" bpm");
                    SetLabelText(rpmLabel,measurement.rpm.ToString());
                    SetLabelText(speedLabel,measurement.speed.ToString() + " km/h");
                    SetLabelText(distanceLabel,measurement.distance.ToString()+ " km");
                    SetLabelText(energyLabel,measurement.energy.ToString()+" kJ");
                    SetLabelText(requestedPowerLabel,measurement.requested_power.ToString()+" Watt");
                }
                //Send measurement to the server
                network.sendMeasurement(measurement,((UserClient)currentUser).physician,"Last", username);
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

        /// <summary>
        /// Send bike data to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            //stuur naar server en die stuurt naar client;
            if(pwrTxtBox.BackColor != Color.Red && timeTxtBox.BackColor != Color.Red && distanceTxtBox.BackColor != Color.Red)
                network.sendBikeValues(pwrTxtBox.Text, timeTxtBox.Text, distanceTxtBox.Text,currentUser.username); 
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

        /// <summary>
        /// get the chat message from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void getChatMessage(string sender, string message)
        {

            // Invoke nodig?
            if (RTBChatText.InvokeRequired)
            {
                RTBChatText.Invoke((MethodInvoker)delegate
                {
                    // BSend_Click(sender + ": " + message, null);
                    RTBChatText.Text += sender + ": " + message + System.Environment.NewLine;
                });
            }else
                RTBChatText.Text += sender + ": " + message + System.Environment.NewLine;
        }

        /// <summary>
        /// sends chat message to server 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BSend_Click(object sender, EventArgs e)
        {
            SendChatMessage(TChatSend.Text);
            TChatSend.Text = "";
        }

        private void BSend_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("enter");
            if (e.KeyData == Keys.Enter)
            {
                BSend_Click(sender, e);
            }   
        }


        private void connectedUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //currentUser = (User) connectedUsers.SelectedItem;
            SetCurrentUser();
            RefreshFields();
        }

        delegate void SetCurrentUserCallBack();

        private void SetCurrentUser()
        {
            if (connectedUsers.InvokeRequired)
            {
                try
                {
                    SetCurrentUserCallBack d = new SetCurrentUserCallBack(SetCurrentUser);
                    this.Invoke(d, new object[] {});
                }
                catch (Exception e) { }
            }
            else
            {
                currentUser = (User)connectedUsers.SelectedItem;
            }
        }


        private void RefreshFields()
        {
            SetLabelText(actualPowerLabel, "");
            SetLabelText(timeLabel, "");
            SetLabelText(heartBeatLabel, "");
            SetLabelText(rpmLabel, "");
            SetLabelText(speedLabel, "");
            SetLabelText(distanceLabel, "");
            SetLabelText(energyLabel, "");
            SetLabelText(requestedPowerLabel, "");
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
                        measurement = ((UserClient)currentUser).lastSession().GetLastMeasurement();
                    }
                    catch(NullReferenceException e)
                    {
                    }
                    if (measurement != null)
                    {
                        SetLabelText(actualPowerLabel, measurement.actual_power.ToString() +" Watt");
                        SetLabelText(timeLabel, measurement.time);
                        SetLabelText(heartBeatLabel, measurement.pulse.ToString()+" bpm");
                        SetLabelText(rpmLabel, measurement.rpm.ToString());
                        SetLabelText(speedLabel, measurement.speed.ToString()+" km/h");
                        SetLabelText(distanceLabel, measurement.distance.ToString()+" km");
                        SetLabelText(energyLabel, measurement.energy.ToString()+" kJ");
                        SetLabelText(requestedPowerLabel, measurement.requested_power.ToString() +" Watt");
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("no measurement");
                    }
                    //network.GetAllConnectedUsers(username);
                    users = network.users;
                    if (currentUser != null)
                    {
                        System.Diagnostics.Debug.WriteLine(currentUser.username);
                        User user = network.getUser(currentUser.username);
                        
                        currentUser = user;
                    }
                    //FillUserComboBox();
                    //System.Diagnostics.Debug.WriteLine("current user: " + currentUser);
                }
                Thread.Sleep(1000);
            }
        }

        private void newClient_Click(object sender, EventArgs e)
        {
            new NewClient(network,username).ShowDialog();
        }

        private void viewOldDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new OldSesionData(network,username);
            f.Show();

        }

        private void startVideoTrainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendChatMessage("Starting video training session 15 min workout");

            Thread t = new Thread(delegate () { new Video.VideoPlayer("15MinWorkout.mp4").ShowDialog(); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void minVideoWorkoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendChatMessage("Starting video training session 45 min workout");

            Thread t = new Thread(delegate () { new Video.VideoPlayer("45MinWorkout.mp4").ShowDialog(); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public void SetBikeValues(string power, string time, string distance)
        {
            if (bike != null)
            {
                if (power != "")
                    bike.SetPower(Int32.Parse(power));
                if (time != "")
                    bike.setTime(Int32.Parse(time));
                if (distance != "")
                    bike.SetDistance(Int32.Parse(distance));
            }
        }

        public void SendChatMessage(string text)
        {
            string receiver;
            if (isPhysician)
                receiver = currentUser.username;
            else
                receiver = username;

            network.sendChatMessage(text, username, receiver);
            RTBChatText.Text += username + ": " + text + System.Environment.NewLine;
        }

        public void SendChatMessage(string text, User user)
        {
            string receiver = user.username;
            network.sendChatMessage(text, username, receiver);
            //if(currentUser == user)
                SetChatMessage(username + ": " + text + System.Environment.NewLine);
        }

        delegate void SetChatWindow(string text);

        private void SetChatMessage(string text)
        {
            if (RTBChatText.InvokeRequired)
            {
                try
                {
                    SetChatWindow d = new SetChatWindow(SetChatMessage);
                    this.Invoke(d, new object[] { text });
                }
                catch (Exception e) { }
            }
            else
                RTBChatText.Text += text; 
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            network.sendSaveData();     
        }

        public void DataSaved(bool succesfull)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        
        private void bikeImprovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        //Inspannings test IP2: 
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            if (isPhysician)
            {
                Thread thread = new Thread(new ThreadStart(BikeImprovementThread));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void BikeImprovementThread()
        {
            bool test = true;
            UserClient user = (UserClient) currentUser;
            //The test:
            SendChatMessage("Bike Imporvement test started!",user);
            SendChatMessage("Let's start with a warming up for 10-15 minutes before we start the real excesise",user);
            network.sendBikeValues("100", "0000", "", user.username);
            //Warming up:
            for(int minutes = 15; minutes > 0; minutes--)
            {
                SendChatMessage("Just " + minutes + " minutes left",user);
                //Thread.Sleep(60000);
                Thread.Sleep(10);
            }
            //Making new test on server side:
            network.sendMeasurement(null, "", "CreateTest", user.username);
            //First part of the test:
            SendChatMessage("You are all warmed up now", user);
            SendChatMessage("Now lets start with 90 rpm for the next 120 seconds",user);
            network.sendBikeValues("100", "", "", user.username);
            for(int seconds = 120; seconds > 0; seconds --)
            {
                user = (UserClient) network.getUser(user.username);
                network.sendMeasurement(user.lastSession().GetLastMeasurement(),"", "AddToTest",user.username);
                if (seconds % 30 == 0 || seconds < 10)
                    SendChatMessage("Just " + seconds + " seconds left", user);
                Thread.Sleep(1000);
            }
            network.sendSaveData();
            //Second part of the test:
            SendChatMessage("First part of the test finished!", user);
            SendChatMessage("Now we will keep repeating this pattern until you can't handle it no more", user);
            int power = 100;
            int time = 100;
            while (test && time > 0)
            {
                power += 20;
                time -= 20;
                SendChatMessage("Now cycle at 90 rpm for the next " + time + " seconds", user);
                network.sendBikeValues("" + power, "", "", user.username);
                for (int seconds = time; seconds > 0; seconds--)
                {
                    user = (UserClient)network.getUser(user.username);
                    Measurement m = (user.lastSession().GetLastMeasurement());
                    if (m.rpm == 0)
                        test = false;
                    network.sendMeasurement(m, "", "AddToTest", user.username);
                    if (seconds % 25 == 0 || seconds < 10)
                        SendChatMessage("Just " + seconds + " seconds left", user);
                    Thread.Sleep(1000);
                }
                network.sendSaveData();
            }
            SendChatMessage("End of the test!", user);
            network.sendSaveData();
        }

        private void olddataToolStripMenuItem_Click(object sender, EventArgs e)
        {
              OldTestData old = new OldTestData((UserClient) currentUser);
              DialogResult dialogResult = old.ShowDialog();
              if (dialogResult == DialogResult.OK) { }
              old.Dispose();      
        }
    } 
}
