using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JenkinsTray.Core
{
    public class RunOnWindowsStartup
    {
        private static string name = "JenkinsTray";

        // The path to the key where Windows looks for startup applications
        private static RegistryKey rk = 
            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static bool Enabled 
        {
            get
            {
                return rk.GetValue(name) != null;
            }

            set
            {
                if (value)
                {
                    // Add the value in the registry so that the application runs at startup
                    var exePath = typeof(RunOnWindowsStartup).Assembly.Location;
                    rk.SetValue(name, exePath);
                }
                else
                {
                    // Remove the value from the registry so that the application doesn't start
                    rk.DeleteValue(name, false);
                }
            }
        }
    }
}
