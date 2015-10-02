using Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Login : Form, ClientInterface
    {
        private SslStream stream;
        private Networkconnect network;

        public Login()
        {
            InitializeComponent();
            network = new Networkconnect("127.0.0.1", 130);

            stream = network.getStream();
            statusLabel.Text = network.status;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Sending to server here: 
            Packet loginPacket = new PacketLogin() { username = usernameTextBox.Text, password = passwordTextBox.Text };
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, loginPacket);

            Form form = new FormClient(network);
            this.Hide();
            if(form.ShowDialog() == DialogResult.Cancel)
                this.Dispose();
        }
        public void loginResponse(bool loginOk)
        {
            if (loginOk)
                MessageBox.Show("Login is ok");
            else
                MessageBox.Show("Login is niet ok");
        }
    }
}
