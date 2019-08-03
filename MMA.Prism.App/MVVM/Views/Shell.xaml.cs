using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MMA.Prism.App.MVVM.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Really close?", "Warning", MessageBoxButton.YesNo);
            //if (result != MessageBoxResult.Yes)
            //{
            //    e.Cancel = true;
            //}
            e.Cancel = false;
        }
    }
}
