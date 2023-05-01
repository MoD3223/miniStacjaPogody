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

namespace miniStacjaPogody
{
    /// <summary>
    /// Logika interakcji dla klasy GlowneOkno.xaml
    /// </summary>
    public partial class GlowneOkno : Page
    {
        public GlowneOkno()
        {
            InitializeComponent();
        }

        private void btnByd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NavS.Navigate(new Uri("Bydgoszcz.xaml", UriKind.Relative));
        }

        private void btnGre_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NavS.Navigate(new Uri("Grenlandia.xaml", UriKind.Relative));
        }

        private void btnEgi_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NavS.Navigate(new Uri("Egipt.xaml", UriKind.Relative));
        }




    }
}
