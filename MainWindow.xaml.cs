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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    /// Stacja pogody wymagania
    /// temperatura, wilgotnosc, sila wiatru, data aktualna i poprzednia (np z kazdym uruchomieniem zapisuje do pliku z dzisiejsza data, jak jest to dopisuje
    /// UI latwe do uzycia w kazdych warunkach, dotykowe i klawiszowe naraz
    /// wersja latwa: losowe dane i zmiana co x sekund (rand od 0 do 3, 0 do 1 zmiejsza o jeden, 1 do 2 takie samo, 2 do 3 zwieksza o jeden)






    public partial class MainWindow : Window
    {
        public static NavigationService NavS => ((MainWindow)Application.Current.MainWindow).MainFrame.NavigationService;
        public MainWindow()
        {
            InitializeComponent();
            NavS.Navigate(new Uri("GlowneOkno.xaml", UriKind.Relative));
        }

        public static void Powrot()
        {
            try
            {
                MainWindow.NavS.GoBack();
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }
        }

        public static void navPanelAdministracyjny()
        {
            MainWindow.NavS.Navigate(new Uri("PanelAdministracyjny.xaml", UriKind.Relative));
        }

        
    }
}
