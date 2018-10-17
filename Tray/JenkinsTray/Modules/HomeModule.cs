using JenkinsTray.Core;
using JenkinsTray.Model;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsTray.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(ITinyMessengerHub hub)
        {
            Post["/"] = parameters =>
            {
                var notification = this.Bind<Notification>();
                hub.Publish(notification);
                return HttpStatusCode.OK;
            };
        }
    }
}
