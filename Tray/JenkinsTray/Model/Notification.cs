using JenkinsTray.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsTray.Model
{
    public class Notification : ITinyMessage
    {
        public Job Job { get; set; }
        public Build Build { get; set; }

        public object Sender
        {
            get { return null; }
        }
    }
}
