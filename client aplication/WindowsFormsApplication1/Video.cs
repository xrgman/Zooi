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
    public partial class Video : Form
    {
        public Video()
        {
            InitializeComponent();
            axWindowsMediaPlayer1.URL = @"C:\Users\Davey\Videos\4K Video Downloader\15MinWorkout.mp4";
        }

    }
}
