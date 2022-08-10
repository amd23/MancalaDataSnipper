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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MancalaDataSnipper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (sender, e) =>
            {
                var vm = this.DataContext as MainWindowViewModel;
                try
                {
                    {
                        e.Cancel = true;
                        Application.Current.Shutdown();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Application.Current.Shutdown();
                }
            };
            


            
        }
    }
}
