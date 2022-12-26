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
        private int counter = 0, counterTrans = 0;
        Font font;
        List<PictureBox> pics = new List<PictureBox>();
        PictureBox pic = new PictureBox();
        List<string> txts = new List<string>();
        Bitmap bmp = new Bitmap(10, 10);
        private bool canPaint = false;

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
            font = new System.Drawing.Font("Arial", 6);

            pictureBox1.Size =
                new Size(pictureBox1.Size.Width / 2, pictureBox1.Size.Height / 2);
            pictureBox2.Size =
                new Size(pictureBox2.Size.Width / 6, pictureBox2.Size.Height);
            pictureBox1.Location =
                new Point(Size.Width - pictureBox1.Size.Width - 3,
                    Size.Height - pictureBox1.Size.Height - 3
                );

            previewMode = true;
        }

        private void frmScreenSaver_Load(object sender, EventArgs e)
        {
            txts.Add("People First");
            txts.Add("Stronger Together");
            txts.Add("Do what's right, not what's easy");
            txts.Add("Always Deliver");
            txts.Add("Be Authentic");

            LoadSettings();

            Cursor.Hide();
            TopMost = true;


            pics.Add(pic1);
            pics.Add(pic2);
            pics.Add(pic3);
            pics.Add(pic5);
            pics.Add(pic4);

            moveTimer.Interval = 500;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

        }
        private void LoadSettings()
        {
            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Statics.RegisteryPath);
            if (key != null)
            {
                try
                {
                    txts.Clear();
                    txts.Add((string)key.GetValue("text1"));
                    txts.Add((string)key.GetValue("text2"));
                    txts.Add((string)key.GetValue("text3"));
                    txts.Add((string)key.GetValue("text5"));
                    txts.Add((string)key.GetValue("text4"));

                    ForeColor = Color.FromArgb(int.Parse(key.GetValue("FontColor").ToString()));
                    BackColor = Color.FromArgb(int.Parse(key.GetValue("BackColor").ToString()));
                    string[] str = key.GetValue("fontsize").ToString().Split(Convert.ToChar(","));


                    float fontSize = (float)Convert.ToDouble(str[2]);
                    fontSize = (float)Convert.ToDouble(((0.02 * this.Width) + fontSize));
                    //fontSize = (Bounds.Width / 800) * fontSize; //12;
                    if (fontSize < 10) fontSize = 10;
                    if (fontSize > 63) fontSize = 63;

                    font = new Font(str[1], fontSize,
                        str[0] == "False" ? FontStyle.Regular : FontStyle.Bold);

                }
                catch (Exception)// ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            else
            {
                //Set default color
                ForeColor = Color.White;
                float fontSize = (float)Convert.ToDouble(((0.02 * this.Width) + 20));
                //fontSize = (Bounds.Width / 800) * fontSize;
                if (fontSize < 10) fontSize = 10;
                if (fontSize > 63) fontSize = 63;


                font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            }
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            if (counterTrans >= 4)
            {
                pic.Paint -= pic_Paint;
            }
            if (canPaint)
            {
                canPaint = false;
                if (counter <= 4)
                {
                    string txt = txts[counter];
                    float val = 1.9f;

                    if (counterTrans == 0)
                    {
                    }
                    else if (counterTrans == 1)
                        val = 1.6f;
                    else if (counterTrans == 2)
                        val = 1.4f;
                    else if (counterTrans == 3)
                        val = 1.2f;
                    else if (counterTrans == 4)
                        val = 1.1f;
                    else if (counterTrans == 5)
                        val = font.Size;

                    Font ft = new Font(font.Name, Convert.ToInt32(
                            Math.Round((font.Size / val), 2)), font.Style);
                    SizeF size = e.Graphics.MeasureString(txt, ft);

                    int pointX = 0;
                    if (counter == 1 || counter == 3)
                        pointX = pic.Width - ((int)(size.Width + 10));
                    else if (counter == 2)
                        pointX = (pic.Width / 2) - ((int)(size.Width / 2));


                    e.Graphics.DrawString(txt, ft, new SolidBrush(ForeColor),
                        pointX, 0);
                }
            }
        }

        void TransparetBackground(Control C)
        {
            if (previewMode) return;
            try
            {
                C.Visible = false;

                C.Refresh();
                Application.DoEvents();
                if (this.IsDisposed)
                {
                    return;
                }
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
            catch (Exception)
            {
            }
        }
        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            // Move text to new location
            if (counter <= 5 && timerTransparent.Enabled == false)
            {
                moveTimer.Interval = 3000;
                timerTransparent.Interval = 1;
                counterTrans = 0;
                pictureBox2.Visible = true;
                if (counter == 0)
                {
                    foreach (var item in pics)
                    {
                        TransparetBackground(item);
                    }
                }
                timerTransparent.Enabled = true;
                timerTransparent.Start();
            }
        }

        private void timerTransparent_Tick(object sender, EventArgs e)
        {
            timerTransparent.Interval = 200;
            canPaint = true;
            if (counter <= 4 && counterTrans <= 5)
            {
                if (counterTrans == 0)
                {
                    pic = pics[counter];
                }
                pic.Refresh();
                if (counterTrans == 0)
                {
                    pic.Paint += pic_Paint;
                }
                this.InvokePaint(pic, new PaintEventArgs(pic.CreateGraphics(), pic.DisplayRectangle));
                counterTrans++;
                if (counterTrans == 5)
                {
                    counter++;
                    counterTrans = 0;
                }
            }
            else if (counter < 10)
            {
                Graphics g1 = pic1.CreateGraphics();
                Graphics g2 = pic2.CreateGraphics();
                Graphics g3 = pic3.CreateGraphics();
                Graphics g4 = pic4.CreateGraphics();
                Graphics g5 = pic5.CreateGraphics();

                float val = 0f;
                if (counter == 5)
                    val = 1.1f;
                else if (counter == 6)
                    val = 1.2f;
                else if (counter == 7)
                    val = 1.4f;
                else if (counter == 8)
                    val = 1.6f;
                else if (counter == 9)
                    val = 1.9f;
                //val = 1.0f;
                Font ft = new Font(font.Name, Convert.ToInt32(
                        Math.Round((font.Size / val), 2)), font.Style);

                SizeF s1 = g1.MeasureString(txts[0], ft);
                SizeF s2 = g2.MeasureString(txts[1], ft);
                SizeF s3 = g3.MeasureString(txts[2], ft);
                SizeF s4 = g4.MeasureString(txts[4], ft);
                SizeF s5 = g5.MeasureString(txts[3], ft);


                int pointX2 = pics[1].Width - ((int)(s2.Width + 10));
                int pointX5 = pics[4].Width - ((int)(s5.Width + 10));
                int pointX3 = (pics[2].Width / 2) - ((int)(s3.Width / 2));


                pics[0].Refresh();
                pics[1].Refresh();
                pics[2].Refresh();
                pics[3].Refresh();
                pics[4].Refresh();

                g1.DrawString(txts[0], ft, new SolidBrush(ForeColor),
                    0, 0);
                g2.DrawString(txts[1], ft, new SolidBrush(ForeColor),
                    pointX2, 0);
                g3.DrawString(txts[2], ft, new SolidBrush(ForeColor),
                    pointX3, 0);
                g4.DrawString(txts[4], ft, new SolidBrush(ForeColor),
                    0, 0);
                g5.DrawString(txts[3], ft, new SolidBrush(ForeColor),
                    pointX5, 0);

                counter++;
            }
            else if (counter == 10)
            {
                counter = 0;
                pic1.Visible = false;
                pic2.Visible = false;
                pic3.Visible = false;
                pic4.Visible = false;
                pic5.Visible = false;
                pictureBox2.Visible = false;
                moveTimer.Interval = 500;
                timerTransparent.Stop();
                timerTransparent.Enabled = false;
                return;
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
