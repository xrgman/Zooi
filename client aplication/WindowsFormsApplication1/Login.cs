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
    public partial class Login : Form
    {
        private Network network;

        public Login()
        {
            InitializeComponent();
            network = new Network("127.0.0.1", 130);
            statusLabel.Text = network.status;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Sending to server here: 
            

            Form form = new FormClient(network);
            this.Hide();
            if(form.ShowDialog() == DialogResult.Cancel)
                this.Dispose();
        }
    }
}
