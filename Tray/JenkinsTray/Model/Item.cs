using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsTray.Model
{
    public class Item : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged("Name"); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged("Status"); }
        }

        private DateTime timestamp;
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; RaisePropertyChanged("Timestamp"); }
        }

        private bool alertSuccess;
        public bool AlertSuccess
        {
            get { return alertSuccess; }
            set { alertSuccess = value; RaisePropertyChanged("AlertSuccess"); }
        }

        private bool alertFailure;
        public bool AlertFailure
        {
            get { return alertFailure; }
            set { alertFailure = value; RaisePropertyChanged("AlertFailure"); }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; RaisePropertyChanged("Url"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
