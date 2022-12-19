using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScreenSaverApp
{
    public static class Statics
    {
        public static string RegisteryPath = "SOFTWARE\\" + Application.ProductName;
        //public static RegistryKey RegisteryPath = Application.UserAppDataRegistry;
    }
}
