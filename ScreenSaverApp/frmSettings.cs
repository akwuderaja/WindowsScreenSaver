using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            if (key != null){
                txtTextToDisplay1.Text = (string)key.GetValue("text1");
                txtTextToDisplay2.Text = (string)key.GetValue("text2");
                txtTextToDisplay3.Text = (string)key.GetValue("text3");
                txtTextToDisplay4.Text = (string)key.GetValue("text4");
                txtTextToDisplay5.Text = (string)key.GetValue("text5");
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
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }
    }
}
