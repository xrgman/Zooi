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
    public partial class NewClient : Form
    {
        Networkconnect network;

        public NewClient(Networkconnect network)
        {
            InitializeComponent();
            this.network = network;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(gebruikersNaam.Text!=""&&password.Text!="")
            {
               // network.GetAllUsers().Contains
                if (true)
                {

                }
                else
                {
                    MessageBox.Show("Username already in use, please choose another one.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields!");
            }
           // if (usernameTextBox.Text != "" && passwordTextBox.Text != "")
            //{
            //    Tuple<bool, bool> login = network.login(usernameTextBox.Text, passwordTextBox.Text);
            //    if (login.Item1)
            //    {
            //        Form form = new FormClient(network, login.Item2);
            //        this.Hide();
            //        if (form.ShowDialog() == DialogResult.Cancel)
            //            this.Dispose();
            //    }
            //    else
             //       MessageBox.Show("Wrong username or password");
            //}
            //else
            //    MessageBox.Show("Please enter a username and a password!");
        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void gebruikersNaam_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
