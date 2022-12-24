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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using ScreenSaverApp.Properties;
using System.Resources;
using System.Threading;

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
        private int counter = 0, counterTrans = 0;
        int A = 0, R = 0, G = 0, B = 0;
        Font font;
        List<Label> lbls = new List<Label>();

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
            //TopMost = true;

            moveTimer.Interval = 500;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

            lbls.Add(label1);
            lbls.Add(label2);
            lbls.Add(label3);
            lbls.Add(label4);
            lbls.Add(label5);
        
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

            //C.Visible = true;
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
                panel1.Visible = false;
                moveTimer.Interval = 500;
                //panel1.BackgroundImage= null;
                timerTransparent.Stop(); 
            }
            else
            {
                moveTimer.Interval = 3000;
                counterTrans = 0;
                panel1.Visible = true;
                if (counter==0)
                {
                    foreach (Control item in Controls)
                    {
                        if (typeof(Label) == item.GetType())
                        {
                            try
                            {
                                TransparetBackground(item);
                                //item.Visible = false;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
                Thread.Sleep(100);
                timerTransparent.Start(); 
            }
        }

        private void timerTransparent_Tick(object sender, EventArgs e)
        {
            if (counter >= 5)
                return;
            var c = lbls[counter].ForeColor;
            if (counterTrans == 0)
            {
                A = c.A;
                R = c.R;
                G = c.G;
                B = c.B;

                Color c2 = Color.FromArgb(A,
                    (int)(R/4), (int)(G/4), (int)(B /4));
                    //(int)(c.R * 0.8), (int)(c.G * 0.8), (int)(c.B * 0.8));
                //lbls[counter].ForeColor = c2;
                lbls[counter].Font = new Font(font.Name, font.Size/1.5f, font.Style);
                lbls[counter].Visible = true;
                counterTrans++;
            }
            else if (counterTrans == 1)
            {
                Color c2 = Color.FromArgb(A,
                    (int)(R / 3), (int)(G / 3), (int)(B / 3));
                //(int)(c.R * 0.8), (int)(c.G * 0.8), (int)(c.B * 0.8));
                //lbls[counter].ForeColor = c2;
                lbls[counter].Font = new Font(font.Name, font.Size / 1.3f, font.Style);
                counterTrans++;
            }
            else if (counterTrans == 2)
            {
                Color c2 = Color.FromArgb(A,
                    (int)(R/2), (int)(G/2), (int)(B /2));
                //(int)(c.R * 0.8), (int)(c.G * 0.8), (int)(c.B * 0.8));
                //lbls[counter].ForeColor = c2;
                lbls[counter].Font = new Font(font.Name, font.Size / 1.1f, font.Style);
                counterTrans++;
            }
            else if (counterTrans == 3)
            {
                lbls[counter].ForeColor = ForeColor;
                lbls[counter].Font = font;
                counterTrans = 0;
                counter++; timerTransparent.Stop();
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


                    float fontSize = (float)Convert.ToDouble(str[2]);
                    fontSize = (Bounds.Width / 800) * fontSize; //12;
                    if (fontSize < 10) fontSize = 10;

                    font = new Font(str[1], fontSize,
                        str[0] == "False" ? FontStyle.Regular : FontStyle.Bold);

                    label1.ForeColor = ForeColor;
                    label2.ForeColor = ForeColor;
                    label3.ForeColor = ForeColor;
                    label4.ForeColor = ForeColor;
                    label5.ForeColor = ForeColor;

                    label1.Font = font;
                    label2.Font = font;
                    label3.Font = font;
                    label4.Font = font;
                    label5.Font = font;
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

        //private int _lastFormSize;
        private void frmScreenSaver_SizeChanged(object sender, EventArgs e)
        {
            //var bigger = GetArea(this.Size) > _lastFormSize;
            //float scaleFactor = bigger ? 1.1f : 0.9f;
            //label1.Font = new Font(label1.Font.FontFamily.Name, label1.Font.Size * scaleFactor);

            //_lastFormSize = GetArea(this.Size);
        }


        //private int GetArea(Size size)
        //{
        //    return size.Height * size.Width;
        //}
    }
}
