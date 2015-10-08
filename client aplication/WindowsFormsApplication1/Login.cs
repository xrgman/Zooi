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
    public partial class Login : Form
    {
        private Networkconnect network;

        public Login()
        {
            InitializeComponent();
            network = new Networkconnect("127.0.0.1", 130);
            statusLabel.Text = network.status;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.Text != "" && passwordTextBox.Text != "")
            {
                Tuple<bool,bool> login =  network.login(usernameTextBox.Text, passwordTextBox.Text);
                if (login.Item1)
                {
                    Form form = new FormClient(network,login.Item2);
                    this.Hide();
                    if (form.ShowDialog() == DialogResult.Cancel)
                        this.Dispose();
                }
                else
                    MessageBox.Show("Wrong username or password");
            }
            else
                MessageBox.Show("Please enter a username and a password!");
        }
    }
}
