using JenkinsTray.Core;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace JenkinsTray
{
    public partial class App : Application
    {
        public const string Version = "1.0";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = Nancy.TinyIoc.TinyIoCContainer.Current;
            container.Register<ITinyMessengerHub, TinyMessengerHub>().AsSingleton();

            int port = GetPort();
            int result = 0;
            
            try
            {
                result = Server.Instance.Start(port, container);
            }
            catch (Exception)
            {
                result = -1;
            }

            if (result != 0)
            { 
                MessageBox.Show("Não foi possível iniciar o serviço na porta: " + port);
                Shutdown(-1);
            }
        }

        private int GetPort()
        {
            string value = ConfigurationManager.AppSettings["port"];

            int port = 0;

            if (int.TryParse(value, out port))
            {
                return port;
            }

            return 3333;
        }
    }
}
