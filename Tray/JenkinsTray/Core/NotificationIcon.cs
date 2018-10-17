using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JenkinsTray.Core
{
    public class NotificationIcon
    {
        private static System.Windows.Forms.NotifyIcon trayIcon;

        public static void Setup(Window window)
        {
            trayIcon = new System.Windows.Forms.NotifyIcon();
            var exePath = typeof(NotificationIcon).Assembly.Location;
            trayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(exePath);
            trayIcon.Visible = true;

            trayIcon.DoubleClick += (sender, args) =>
            {
                window.Show();
                window.Activate();
                window.WindowState = WindowState.Normal;
            };

            window.Closed += (sender, args) =>
            {
                try
                {
                    if (trayIcon != null)
                    {
                        trayIcon.Visible = false;
                        trayIcon.Icon = null; // required to make icon disappear
                        trayIcon.Dispose();
                        trayIcon = null;
                    }

                }
                catch (Exception) { }
            };

            window.StateChanged += (sender, args) =>
            {
                if (window.WindowState == WindowState.Minimized)
                {
                    window.Hide();
                }
            };

            window.Hide();
        }
    }
}
