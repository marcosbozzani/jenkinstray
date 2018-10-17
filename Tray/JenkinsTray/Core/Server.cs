using Nancy;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsTray.Core
{
    public class Server
    {
        private Server() { }

        public int Port { get; private set; }

        public bool Running { get; private set; }

        public static Server Instance { get { return instance.Value; } }

        public int Start(int port, TinyIoCContainer container = null)
        {
            if (Running)
            {
                return 0;
            }

            InitHost(port, container);

            if (!Running)
            {
                var cmd1 = AddUrlAcl(string.Format("http://+:{0}/", port));
                var cmd2 = OpenFirewallPort("JenkinsTray", port);

                int result = RunElevated(cmd1, cmd2);

                if (result != 0)
                {
                    return result;
                }

                InitHost(port, container);
            }
            
            if (!Running)
            {
                return -1;
            }

            Port = port;
            return 0;
        }

        public int Stop()
        {
            if (Running)
            {
                host.Stop();
            }

            return 0;
        }

        private void InitHost(int port, TinyIoCContainer container)
        {
            try
            {
                var bootstrapper = new Bootstrapper(container);
                var uri = new Uri(string.Format("http://localhost:{0}/", port));
                host = new NancyHost(bootstrapper, uri);
                host.Start();
                Running = true;
            }
            catch (AutomaticUrlReservationCreationFailureException)
            {
                Running = false;
            }
        }

        private string AddUrlAcl(string address)
        {
            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var account = (NTAccount)sid.Translate(typeof(NTAccount));
            return AddUrlAcl(address, account.Value);
        }

        private string AddUrlAcl(string address, string user)
        {
            var args = @"netsh http add urlacl url={0} user={1}";
            return string.Format(args, address, user);
        }

        private string OpenFirewallPort(string ruleName, int port)
        {
            var args = "netsh advfirewall firewall add rule name=\"{0}\" dir=in protocol=TCP localport=\"{1}\" action=allow";
            return string.Format(args, ruleName, port);
        }

        private int RunElevated(params string[] commands)
        {
            var args = new StringBuilder("/C \"");

            foreach (var command in commands)
            {
                args.Append(command.Replace("\"", "\\\""));

                if (command != commands.Last())
                {
                    args.Append(" & ");
                }
            }

            args.Append("\"");

            var startInfo = new ProcessStartInfo("cmd.exe", args.ToString());
            startInfo.Verb = "runas";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.UseShellExecute = true;

            var process = Process.Start(startInfo);
            process.WaitForExit();

            return process.ExitCode;
        }

        private NancyHost host;

        private static Lazy<Server> instance = new Lazy<Server>(() => new Server());

        private class Bootstrapper : DefaultNancyBootstrapper
        {
            private readonly TinyIoCContainer container;

            public Bootstrapper(TinyIoCContainer container)
            {
                this.container = container;
            }

            protected override TinyIoCContainer GetApplicationContainer()
            {
                return container ?? base.GetApplicationContainer();
            }
        }
    }
}
