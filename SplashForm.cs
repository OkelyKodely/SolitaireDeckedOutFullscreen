using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Solitaire
{
    public class SplashForm : Form
    {
        public SplashForm()
        {
            this.SetBounds(0, 0, 300, 220);
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;

            Panel splashPanel = new Panel();
            splashPanel.SetBounds(0, 0, 300, 220);
            Size size = new Size(300, 220);
            splashPanel.BackgroundImage = Image.FromFile("Content/frame_136_delay-s.gif");

            Bitmap bmp = new Bitmap(splashPanel.BackgroundImage, size);
            splashPanel.BackgroundImage = bmp;
            this.Controls.Add(splashPanel);
            this.Show();

            new System.Threading.ManualResetEvent(false).WaitOne(1000*3);

            this.Close();
        }
    }
}