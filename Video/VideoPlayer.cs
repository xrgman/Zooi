using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Video
{
    public partial class VideoPlayer : Form
    {
        public VideoPlayer(String video)
        {          
            InitializeComponent();
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.URL = @"C:\Users\Davey\Videos\4K Video Downloader\"+video;
            
        }

        private void VideoPlayer_FormClosing(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.Hide();
            axWindowsMediaPlayer1.close();
            axWindowsMediaPlayer1.Dispose();
        }

    }
}
