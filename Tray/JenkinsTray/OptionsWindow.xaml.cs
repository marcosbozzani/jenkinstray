using JenkinsTray.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JenkinsTray
{
    public partial class OptionsWindow : Window
    {
        private Item item;

        public OptionsWindow(Item item)
        {
            InitializeComponent();
            this.item = item;
            DataContext = item;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RemoveJobClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
