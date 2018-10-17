using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsTray.Model
{
    public class Build
    {
        public string Name { get; set; }
        public long Duration { get; set; }
        public string Url { get; set; }
        public State State { get; set; }
        public Result Result { get; set; }
    }
}
