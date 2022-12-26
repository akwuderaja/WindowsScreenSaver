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
        private Random rand = new Random();
        private int counter = 0, counterTrans = 0;
        int A = 0, R = 0, G = 0, B = 0;
        Font font;
        List<Label> lbls = new List<Label>();
        List<PictureBox> pics = new List<PictureBox>();
        Label lbl = new Label();
        PictureBox pic1 = new PictureBox();
        List<string> txts = new List<string>();
        Bitmap bmp = new Bitmap(10, 10);
        private bool canPaint;

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
            pictureBox2.Size =
                new Size(pictureBox2.Size.Width / 4, pictureBox2.Size.Height);
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

            label1.Paint += lbl_Paint;
            label2.Paint += lbl_Paint;
            label3.Paint += lbl_Paint;
            label4.Paint += lbl_Paint;
            label5.Paint += lbl_Paint;

            txts.Add(label1.Text);
            txts.Add(label2.Text);
            txts.Add(label3.Text);
            txts.Add(label4.Text);
            txts.Add(label5.Text);

            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
        }

        private void lbl_Paint(object sender, PaintEventArgs e)
        {
            timerTransparent.Stop();
            lbl = (Label)sender;
            this.InvokePaint(pic, new PaintEventArgs(pic.CreateGraphics(), pic.DisplayRectangle));
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (canPaint)
            {
                if (counter <= 4)
                {
                    var c = lbls[counter].ForeColor;
                    string txt = txts[counter];
                    Font ft = new Font(font.Name, Convert.ToInt32(
                            Math.Round((font.Size / 1.5f), 2)), font.Style);
                    SizeF size = e.Graphics.MeasureString(txt, font);
                    if (counterTrans == 0)
                    {
                        if (counter == 0)
                            bmp = new Bitmap(pic.Width, pic.Height);
                        else
                        {
                            Bitmap newBmp = new Bitmap(pic.Width, pic.Height, e.Graphics);
                            bmp = newBmp;
                            pic.Image = bmp;
                            e.Graphics.Save();
                            e.Dispose();
                            timerTransparent.Start();
                            return;
                        }
                        Thread.Sleep(10);
                    }
                    else if (counterTrans == 1)
                    {
                        ft = new Font(font.Name, Convert.ToInt32(
                            Math.Round((font.Size / 1.4f), 2)), font.Style);
                        size = e.Graphics.MeasureString(txt, font);
                    }
                    else if (counterTrans == 2)
                    {
                        ft = new Font(font.Name, Convert.ToInt32(
                            Math.Round((font.Size / 1.3f), 2)), font.Style);
                        size = e.Graphics.MeasureString(txt, font);
                    }
                    else if (counterTrans == 3)
                    {
                        ft = new Font(font.Name, Convert.ToInt32(
                            Math.Round((font.Size / 1.2f), 2)), font.Style);
                        size = e.Graphics.MeasureString(txt, font);
                    }
                    else if (counterTrans == 4)
                    {
                        ft = new Font(font.Name, Convert.ToInt32(
                            Math.Round((font.Size / 1.5f), 2)), font.Style);
                        size = e.Graphics.MeasureString(txt, font);
                    }

                    pic.Image = bmp;
                    Thread.Sleep(10);
                    e.Graphics.DrawString(txt, ft, new SolidBrush(ForeColor), lbl.Location.X, lbl.Location.Y);
                }
                else
                {
                    float val = 0f;
                    if (counter == 5)
                        val = 1.1f;
                    else if (counter == 6)
                        val = 1.2f;
                    else if (counter == 7)
                        val = 1.3f;
                    else if (counter == 8)
                        val = 1.4f;
                    else if (counter == 9)
                        val = 1.5f;
                    else if (counter == 10)
                    {
                        counter = 0;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        label5.Visible = false;
                        pictureBox2.Visible = false;
                        moveTimer.Interval = 500;
                        timerTransparent.Stop();
                        timerTransparent.Enabled = false;
                        return;
                    }

                    foreach (Control item in Controls)
                        if (typeof(Label) == item.GetType())
                            item.Font = new Font(font.Name, font.Size / val, font.Style);

                    counter++;
                }
                canPaint = false;
                timerTransparent.Start();
            }
        }

        void TransparetBackground(Control C)
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
                    //pic.Image = null;
                }
                timerTransparent.Enabled = true;
                timerTransparent.Start();
            }
        }

        private void DrawLetter(string letter, Control lbl, Font font, Color color)
        {
            Graphics g = pic.CreateGraphics();
            font = FindBestFitFont(g, letter.ToString(), font, pic.ClientRectangle.Size);
            SizeF size = g.MeasureString(letter.ToString(), font);
            g.DrawString(letter, font, new SolidBrush(color), lbl.Location.X, lbl.Location.Y);
        }

        private Font FindBestFitFont(Graphics g, String text, Font font, Size proposedSize)
        {
            // Compute actual size, shrink if needed
            while (true)
            {
                SizeF size = g.MeasureString(text, font);

                // It fits, back out
                if (size.Height <= proposedSize.Height &&
                     size.Width <= proposedSize.Width) { return font; }

                // Try a smaller font (90% of old size)
                Font oldFont = font;
                font = new Font(font.Name, (float)(font.Size * .9), font.Style);
                oldFont.Dispose();
            }
        }

        private void timerTransparent_Tick(object sender, EventArgs e)
        {
            timerTransparent.Interval = 200;
            if (counter <= 4 && counterTrans < 5)
            {
                canPaint = true;
                var c = lbls[counter].ForeColor;
                Label lbl = lbls[counter];
                string txt = txts[counter];
                this.InvokePaint(lbl, new PaintEventArgs(lbl.CreateGraphics(), lbl.DisplayRectangle));
                counterTrans++;
            }
            else if (counterTrans == 5)
            {
                lbls[counter].Font = font;
                counter++;
                counterTrans = 0;
            }
            else
            {
                float val = 0f;
                if (counter == 5)
                    val = 1.1f;
                else if (counter == 6)
                    val = 1.2f;
                else if (counter == 7)
                    val = 1.3f;
                else if (counter == 8)
                    val = 1.4f;
                else if (counter == 9)
                    val = 1.5f;
                else if (counter == 10)
                {
                    counter = 0;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    pictureBox2.Visible = false;
                    moveTimer.Interval = 500;
                    timerTransparent.Stop();
                    timerTransparent.Enabled = false;
                    return;
                }

                foreach (Control item in Controls)
                    if (typeof(Label) == item.GetType())
                        item.Font = new Font(font.Name, font.Size / val, font.Style);

                counter++;
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
                    if (fontSize > 38) fontSize = 38;

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
            else
            {
                ForeColor = label1.ForeColor;
                //float fontSize = (float)Convert.ToDouble(label1.Font.Size);
                float fontSize = (float)Convert.ToDouble(((0.01 * this.Width) + 12));
                fontSize = (Bounds.Width / 800) * fontSize;
                if (fontSize < 10) fontSize = 10;
                if (fontSize > 38) fontSize = 38;


                font = new Font(label1.Font.Name, fontSize, label1.Font.Style);
            }
        }
        //public void DrawStringFloatFormat(PaintEventArgs e, Color color, Font drawFont)
        public void DrawStringFloatFormat(System.Drawing.Graphics e, Color color, Font drawFont)
        {

            // Create string to draw.
            String drawString = "Sample Text";

            // Create font and brush.
            SolidBrush drawBrush = new SolidBrush(color);

            // Create point for upper-left corner of drawing.
            float x = 150.0F;
            float y = 50.0F;

            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            // Draw string to screen.
            //e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            e.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
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
