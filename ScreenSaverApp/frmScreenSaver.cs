using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ScreenSaverApp
{
    public partial class frmScreenSaver : Form
    {
        #region Win32 API functions

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion


        private Point mouseLocation;
        private bool previewMode = false;
        private Random rand = new Random();
        private int counter = 0;

        public frmScreenSaver()
        {
            InitializeComponent();
        }

        public frmScreenSaver(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
        }

        public frmScreenSaver(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            // Make text smaller
            Font ft = new System.Drawing.Font("Arial", 6);
            label1.Font = ft;
            label2.Font = ft;
            label3.Font = ft;
            label4.Font = ft;
            label5.Font = ft;

            label1.Location = new Point(this.Bounds.X + 2, this.Bounds.Y + 5);
            label2.Location = new Point(this.Bounds.X + 80, label1.Location.Y + 23);
            label3.Location = new Point(this.Bounds.X + 25, label2.Location.Y + 23);
            label4.Location = new Point(this.Bounds.X + 80, label3.Location.Y + 23);
            label5.Location = new Point(this.Bounds.X + 2, label4.Location.Y + 23);

            pictureBox1.Size =
                new Size(pictureBox1.Size.Width / 2, pictureBox1.Size.Height / 2);
            panel1.Size =
                new Size(panel1.Size.Width / 4, panel1.Size.Height);
            pictureBox1.Location =
                new Point(Size.Width - pictureBox1.Size.Width - 3,
                    Size.Height - pictureBox1.Size.Height - 3
                );

            previewMode = true;
        }

        private void frmScreenSaver_Load(object sender, EventArgs e)
        {            
            LoadSettings();

            Cursor.Hide();            
            TopMost = true;

            moveTimer.Interval = 1000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

            foreach (Control item in Controls)
            {
                if (typeof(Label)==item.GetType())
                {
                    try
                    {
                        TransparetBackground(item);
                    }
                    catch (Exception)
                    {
                    }
                }
            }            
        }
        void TransparetBackground(Control C)
        {
            C.Visible = false;

            C.Refresh();
            Application.DoEvents();

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;
            int Right = screenRectangle.Left - this.Left;

            Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
            Bitmap bmpImage = new Bitmap(bmp);
            bmp = bmpImage.Clone(new Rectangle(C.Location.X + Right, C.Location.Y + titleHeight, C.Width, C.Height), bmpImage.PixelFormat);
            C.BackgroundImage = bmp;

            C.Visible = true;
        }
        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            // Move text to new location
            if (counter >= 5)
            {
                counter = 0;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
            }
            else if (counter == 0)
            {
                counter++;
                label1.Visible = true;
            }
            else if (counter == 1)
            {
                counter++;
                label2.Visible = true;
            }
            else if (counter == 2)
            {
                counter++;
                label3.Visible = true;
            }
            else if (counter == 3)
            {
                counter++;
                label4.Visible = true;
            }
            else if (counter == 4)
            {
                counter++;
                label5.Visible = true;
            }
        }

        private void LoadSettings()
        {
            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Statics.RegisteryPath);
            if (key != null)
            {
                try
                {
                    label1.Text = (string)key.GetValue("text1");
                    label2.Text = (string)key.GetValue("text2");
                    label3.Text = (string)key.GetValue("text3");
                    label4.Text = (string)key.GetValue("text4");
                    label5.Text = (string)key.GetValue("text5");

                    ForeColor = Color.FromArgb(int.Parse(key.GetValue("FontColor").ToString()));
                    BackColor = Color.FromArgb(int.Parse(key.GetValue("BackColor").ToString()));
                    string[] str = key.GetValue("fontsize").ToString().Split(Convert.ToChar(","));
                    Font ft = new Font(str[1], Convert.ToInt32(str[2]),
                        str[0] == "False" ? FontStyle.Regular : FontStyle.Bold);

                    label1.ForeColor = ForeColor;
                    label2.ForeColor = ForeColor;
                    label3.ForeColor = ForeColor;
                    label4.ForeColor = ForeColor;
                    label5.ForeColor = ForeColor;

                    label1.Font = ft;
                    label2.Font = ft;
                    label3.Font = ft;
                    label4.Font = ft;
                    label5.Font = ft;
                }
                catch (Exception)// ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void frmScreenSaver_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                        Math.Abs(mouseLocation.Y - e.Y) > 5)
                        Application.Exit();
                }

                // Update current mouse location
                mouseLocation = e.Location;
            }
        }

        private void frmScreenSaver_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void frmScreenSaver_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }
    }
}
