using JenkinsTray.Core;
using JenkinsTray.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JenkinsTray
{
    public partial class MainWindow : Window
    {
        private ITinyMessengerHub hub;
        private ItemContainer itemContainer;

        public class MainWindowModel
        {
            public bool RunOnStartup 
            {
                get { return RunOnWindowsStartup.Enabled; }
                set { RunOnWindowsStartup.Enabled = value; } 
            }
            public ObservableCollection<Item> Items { get; set; }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            NotificationIcon.Setup(this);

            var ioc = Nancy.TinyIoc.TinyIoCContainer.Current;

            var successSound = new SoundPlayer(Properties.Resources.sm64_mario_here_we_go);
            var failSound = new SoundPlayer(Properties.Resources.sm64_mario_falling);

            hub = ioc.Resolve<ITinyMessengerHub>();
            hub.Subscribe<Notification>(notification => 
            {
                var item = itemContainer.Update(notification);

                if (notification.Build.State == State.Finalized)
                {
                    if (notification.Build.Result == Result.Success)
                    {
                        if (item.AlertSuccess)
                        {
                            successSound.Play();
                        }
                    }
                    else
                    {
                        if (item.AlertFailure)
                        {
                            failSound.Play();
                        }
                    }
                }
            }, 
            new DispatcherTinyMessageProxy(Dispatcher));

            itemContainer = new ItemContainer();
            
            DataContext = new MainWindowModel() 
            { 
                Items = itemContainer.GetItems()
            };
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            itemContainer.Clear();
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            var item = (e.Source as Button).DataContext as Item;

            var optionsWindow = new OptionsWindow(item);
            optionsWindow.Owner = this;

            if (optionsWindow.ShowDialog() == true)
            {
                itemContainer.Remove(item);    
            }
        }
    }
}
