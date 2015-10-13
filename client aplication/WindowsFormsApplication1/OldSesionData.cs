using Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class OldSesionData : Form
    {
        Networkconnect network;
        Network.User user;
        public OldSesionData(Networkconnect network,string username)
        {
            this.network = network;
            InitializeComponent();
            user = network.getUser(username);
            if(user is Physician)
            {
                Physician p = (Physician)user;
                listBox_Data.DataSource = p.clients;
            }
            else
            {
                UserClient a = (UserClient)user;
                listBox_Data.DataSource = a.sessions;
            }
            
        }

        private void LEnergy_Click(object sender, EventArgs e)
        {

        }

        private void LDistance_Click(object sender, EventArgs e)
        {

        }

        private void OldSesionData_Load(object sender, EventArgs e)
        {

        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        private void listBox_Data_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
