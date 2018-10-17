using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsTray.Model
{
    public class ItemContainer
    {
        private Dictionary<string, Item> lookup;
        private ObservableCollection<Item> items;

        public ItemContainer()
        {
            Load();
            items.CollectionChanged += (sender, e) => Save();
        }

        public ObservableCollection<Item> GetItems()
        {
            return items;
        }

        public Item Update(Notification notification)
        {
            Item item = null;
            bool newItem = false;

            if (!lookup.TryGetValue(notification.Job.Url, out item))
            {
                item = new Item();
                item.AlertSuccess = true;
                item.AlertFailure = true;
                item.Url = notification.Job.Url;
                item.Name = notification.Job.Name;
                newItem = true;
            }

            item.Timestamp = DateTime.Now;

            if (notification.Build.State == State.Started)
            {
                item.Status = "Running";
            }
            else if (notification.Build.State == State.Finalized)
            {
                item.Status = notification.Build.Result.ToString();
            }

            if (newItem)
            {
                Add(item);
            }

            return item;
        }

        public void Add(Item item)
        {
            items.Add(item);
            lookup.Add(item.Url, item);

            var inpc = item as INotifyPropertyChanged;

            if (inpc != null)
            {
                inpc.PropertyChanged += (sender, args) =>
                {
                    Save();
                };
            }
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(items);
            File.WriteAllText(SettingsFile(), json, Encoding.UTF8);
        }

        private void Load()
        {
            items = new ObservableCollection<Item>();
            lookup = new Dictionary<string, Item>();
            
            var json = File.ReadAllText(SettingsFile(), Encoding.UTF8);

            foreach (var item in JsonConvert.DeserializeObject<IEnumerable<Item>>(json))
            {
                Add(item);
            }
        }

        public void Clear()
        {
            items.Clear();
            lookup.Clear();
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            lookup.Remove(item.Url);
        }

        private string SettingsFile()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var jenkinsTrayPath = System.IO.Path.Combine(appDataPath, "JenkinsTray");

            if (!Directory.Exists(jenkinsTrayPath))
            {
                Directory.CreateDirectory(jenkinsTrayPath);
            }

            var settingsFile = System.IO.Path.Combine(jenkinsTrayPath, "settings.json");

            if (!File.Exists(settingsFile))
            {
                File.WriteAllText(settingsFile, "[]", Encoding.UTF8);
            }

            return settingsFile;
        }
    }
}
