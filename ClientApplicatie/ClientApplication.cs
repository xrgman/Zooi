using System.Windows.Forms;

namespace ClientApplicatie
{
    public partial class ClientApplication : Form
    {
        private Network network;
        private string username;

        public ClientApplication(Network network, string username)
        {
            InitializeComponent();
            this.network = network;
            this.username = username;
        }



    }
}
