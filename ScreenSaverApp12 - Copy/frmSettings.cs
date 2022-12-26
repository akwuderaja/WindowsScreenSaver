using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;

namespace ScreenSaverApp
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load display text from the Registry
        /// </summary>
        private void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Statics.RegisteryPath);
            if (key != null)
            {
                try
                {
                    txtTextToDisplay1.Text = (string)key.GetValue("text1");
                    txtTextToDisplay2.Text = (string)key.GetValue("text2");
                    txtTextToDisplay3.Text = (string)key.GetValue("text3");
                    txtTextToDisplay4.Text = (string)key.GetValue("text4");
                    txtTextToDisplay5.Text = (string)key.GetValue("text5");

                    btnFont.ForeColor = Color.FromArgb(int.Parse(key.GetValue("FontColor").ToString()));
                    btnColor.BackColor = Color.FromArgb(int.Parse(key.GetValue("BackColor").ToString()));
                    string[] str = key.GetValue("fontsize").ToString().Split(Convert.ToChar(","));
                    btnFont.Font = new Font(str[1], Convert.ToInt32(str[2]),
                        str[0] == "False" ? FontStyle.Regular : FontStyle.Bold);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Statics.RegisteryPath);

            key.SetValue("text1", txtTextToDisplay1.Text);
            key.SetValue("text2", txtTextToDisplay2.Text);
            key.SetValue("text3", txtTextToDisplay3.Text);
            key.SetValue("text4", txtTextToDisplay4.Text);
            key.SetValue("text5", txtTextToDisplay5.Text);

            key.SetValue("FontColor", btnFont.ForeColor.ToArgb());
            key.SetValue("BackColor", btnColor.BackColor.ToArgb());
            key.SetValue("fontsize", string.Format("{0},{1},{2}",
                btnFont.Font.Bold, btnFont.Font.Name, btnFont.Font.Size));
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Color = btnFont.ForeColor;
            fontDialog1.Font = btnFont.Font;
            if (fontDialog1.ShowDialog()== DialogResult.OK)
            {
                btnFont.ForeColor = fontDialog1.Color;
                btnFont.Font = fontDialog1.Font;
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = btnColor.BackColor;
            if (colorDialog1.ShowDialog()== DialogResult.OK)
            {
                btnColor.BackColor = colorDialog1.Color;
            }
        }
    }
}
