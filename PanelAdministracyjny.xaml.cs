using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml;

namespace miniStacjaPogody
{
    /// <summary>
    /// Logika interakcji dla klasy PanelAdministracyjny.xaml
    /// </summary>
    public partial class PanelAdministracyjny : Page
    {
        static List<string> list = new List<string> {"Bydgoszcz","Egipt","Grenlandia" };
        string selected;
        DateTime selectedDate = DateTime.Today;
        string shortDate;
        int wybrano = 0;


        public static double tempMin;
        public static double temp;
        public static double tempMax;
        public static double tempOdczuwalna;
        public static double tempOdczuwalnaMin;
        public static double tempOdczuwalnaMax;
        public static double wilgotnosc;
        public static double szansaWystapieniaOpadow;
        public static double zachmurzenie;
        public static double predkoscWiatru;
        public static int kierunekWiatru;
        public static int cisnienie;
        public static DateTime wschodSlonca;
        public static DateTime zachodSlonca;
        public static string data;
        public Timer _timer;
        public static double KalibracjaDouble;
        public static string KalibracjaString; //Potrzebne??
        public static bool odkryty = false;






        public static double KalibracjaTemperatura;
        public static double KalibracjaTemperaturaOdczuwalna;
        public static double KalibracjaWilgotnoscProc;
        public static double KalibracjaSzansaWystapieniaOpadowProc;
        public static double KalibracjaZachmurzenieProc;
        public static double KalibracjaPredkoscWiatru;
        public static int KalibracjaKierunekWiatru;
        public static int KalibracjaCisnienie;






        //Ogolnie mamy zmieniac kalibracje o np +0.1 i ma to zapisywac ogolnie do pliku, ma tez pokazywac o ile skalibrowane na panelu sterowania, na glownym juz po poprawce
        //Moze radio buttony obok aby fajnie wskazywac ktore chcemy poprawic? A obok duze przyciski + - i cyfry, wywalic te comboboxy




        public PanelAdministracyjny()
        {
            InitializeComponent();
            wybor.ItemsSource = list;
            wybor.SelectedIndex = 0;
            _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            try
            {
                CzytajKalibracje(WyborNaStringa());
            }
            catch (Exception)
            {
                Kalibracja.ZapiszKalibracje(WyborNaStringa(),0,0);
                CzytajKalibracje(WyborNaStringa());
            }
            txtWartosc.Text = KalibracjaDouble.ToString();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = wybor.SelectedItem.ToString();
            Szukaj_Click(this, e);
            KalibracjaDouble = 0;
            txtWartosc.Text = KalibracjaDouble.ToString();
            try
            {
                CzytajKalibracje(WyborNaStringa());
            }
            catch (Exception)
            {
                Kalibracja.ZapiszKalibracje(WyborNaStringa(), 0, 0);
                CzytajKalibracje(WyborNaStringa());
            }
        }

        private void Szukaj_Click(object sender, RoutedEventArgs e)
        {
                shortDate = selectedDate.ToShortDateString();

                try
                {
                    string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WyborNaStringa(), $"{shortDate}.xml");
                    LadujZPliku(dirPath);
                }
                catch (Exception)
                {
                    DataTemplate.Zapisz(wybor.SelectedIndex,selectedDate);
                    Szukaj_Click(this, e);
                }
            


            LokiData.Text = "Lokalizacja i data";
            txtBlockData.Text = $"{wybor.SelectedValue.ToString()} {selectedDate.ToShortDateString()}\nWschod Slonca: {wschodSlonca.ToString("HH:mm:ss")}\nZachod Slonca: {zachodSlonca.ToString("HH:mm:ss")}";
            txtBlockOpady.Text = $"Wilgotnosc: {wilgotnosc:F1}%\nSzansa wystapienia opadow: {szansaWystapieniaOpadow:F1}%\nZachmurzenie: {zachmurzenie:F1}%\nCisnienie atmosferyzcne: {cisnienie}hpa";
            txtBlockTemp.Text = $"Temperatura minimalna: {tempMin:F1}°C\nTemperatura maksymalna: {tempMax:F1}°C\nTemperatura aktualna: {temp:F1}°C\nTemperatura odczuwalna minimalna: {tempOdczuwalnaMin:F1}°C\nTemperatura odczuwalna maksymalna: {tempOdczuwalnaMax:F1}°C\nTemperatura odczuwalna aktualna: {tempOdczuwalna:F1}°C";
            txtBlockWiatr.Text = $"Predkosc wiatru: {predkoscWiatru:F1}km/h\nKierunek wiatru: {kierunekWiatru}°";
            Edytuj.IsEnabled = true;

        }
        private static void LadujZPliku(string dirPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(dirPath);
            XmlNode root = doc.SelectSingleNode("/DataTemplate");

            data = DateTime.Parse(root.SelectSingleNode("Data").InnerText).ToShortDateString();
            tempMin = Double.Parse(root.SelectSingleNode("Temperatura_Minimalna").InnerText);
            temp = Double.Parse(root.SelectSingleNode("Temperatura_Aktualna").InnerText);
            tempMax = Double.Parse(root.SelectSingleNode("Temperatura_Maksymalna").InnerText);
            tempOdczuwalna = Double.Parse(root.SelectSingleNode("Temperatura_Odczuwalna").InnerText);
            tempOdczuwalnaMin = Double.Parse(root.SelectSingleNode("Temperatura_OdczuwalnaMin").InnerText);
            tempOdczuwalnaMax = Double.Parse(root.SelectSingleNode("Temperatura_OdczuwalnaMax").InnerText);
            wilgotnosc = Double.Parse(root.SelectSingleNode("WilgotnoscProc").InnerText);
            szansaWystapieniaOpadow = Double.Parse(root.SelectSingleNode("WilgotnoscProc").InnerText);
            zachmurzenie = Double.Parse(root.SelectSingleNode("ZachmurzenieProc").InnerText);
            predkoscWiatru = Double.Parse(root.SelectSingleNode("Predkosc_Wiatru_kmh").InnerText);
            kierunekWiatru = Int32.Parse(root.SelectSingleNode("Kierunek_Wiatru").InnerText);
            cisnienie = Int32.Parse(root.SelectSingleNode("Cisnienie_Atmosferyczne").InnerText);
            wschodSlonca = DateTime.Parse(root.SelectSingleNode("Wschod_Slonca").InnerText);
            zachodSlonca = DateTime.Parse(root.SelectSingleNode("Zachod_Slonca").InnerText);
        }

        private void Update(object state)
        {
            Dispatcher.Invoke(new Action(Clean));
        }
        

        private void Clean()
        {
            operacja.Text = "";
        }


        private void powrot(object sender, RoutedEventArgs e)
        {
            MainWindow.Powrot();
        }

        private void save(object sender, RoutedEventArgs e)
        {
            Kalibracja.ZapiszKalibracje(WyborNaStringa(),wybrano,Double.Parse(txtWartosc.Text));
            if (wybor.SelectedIndex == 0)
            {
                DataTemplate.Zapisz(10, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            else if (wybor.SelectedIndex == 1)
            {
                DataTemplate.Zapisz(11, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            else if (wybor.SelectedIndex == 2)
            {
                DataTemplate.Zapisz(12, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            CzytajKalibracje(WyborNaStringa());
        }

        private void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            savebtn.IsEnabled = true;
            odkryty = true;
            rdTemp.Visibility = Visibility.Visible;
            rdOpad.Visibility = Visibility.Visible;
            rdWiat.Visibility = Visibility.Visible;
        }
        
        private string WyborNaStringa()
        {
            if (wybor.SelectedIndex == 0)
            {
                return "Bydgoszcz";
            }
            else if (wybor.SelectedIndex == 1)
            {
                return "Egipt";
            }
            else
            {
                return "Grenlandia";
            }
        }

        private void btnPlusZeroOne_Click(object sender, RoutedEventArgs e)
        {
            KalibracjaDouble += 0.01;
            KalibracjaDouble = Math.Round(KalibracjaDouble, 2);
            txtWartosc.Text = KalibracjaDouble.ToString();

        }

        private void btnMinusOne_Click(object sender, RoutedEventArgs e)
        {
            KalibracjaDouble -= 0.1;
            KalibracjaDouble = Math.Round(KalibracjaDouble, 2);
            txtWartosc.Text = KalibracjaDouble.ToString();
        }

        private void btnPlusOne_Click(object sender, RoutedEventArgs e)
        {
            KalibracjaDouble += 0.1;
            KalibracjaDouble = Math.Round(KalibracjaDouble, 2); ;
            txtWartosc.Text = KalibracjaDouble.ToString();
        }

        private void btnMinusZeroOne_Click(object sender, RoutedEventArgs e)
        {
            KalibracjaDouble -= 0.01;
            KalibracjaDouble = Math.Round(KalibracjaDouble, 2);
            txtWartosc.Text = KalibracjaDouble.ToString();
        }




        private void CzytajKalibracje(string lokacja)
        {
            string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lokacja, "Kalibracja.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(dirPath);
            XmlNode root = doc.SelectSingleNode("/Kalibracja");
            //tempMin = Double.Parse(root.SelectSingleNode("Temperatura_Minimalna").InnerText);
            KalibracjaTemperatura = Double.Parse(root.SelectSingleNode("KalibracjaTemperatura").InnerText);
            KalibracjaTemperaturaOdczuwalna = Double.Parse(root.SelectSingleNode("KalibracjaTemperaturaOdczuwalna").InnerText);
            KalibracjaWilgotnoscProc = Double.Parse(root.SelectSingleNode("KalibracjaWilgotnoscProc").InnerText);
            KalibracjaSzansaWystapieniaOpadowProc = Double.Parse(root.SelectSingleNode("KalibracjaSzansaWystapieniaOpadowProc").InnerText);
            KalibracjaZachmurzenieProc = Double.Parse(root.SelectSingleNode("KalibracjaZachmurzenieProc").InnerText);
            KalibracjaPredkoscWiatru = Double.Parse(root.SelectSingleNode("KalibracjaPredkoscWiatru").InnerText);
            KalibracjaKierunekWiatru = Int32.Parse(root.SelectSingleNode("KalibracjaKierunekWiatru").InnerText);
            KalibracjaCisnienie = Int32.Parse(root.SelectSingleNode("KalibracjaCisnienie").InnerText);
        }

        private void rdTemp_Click(object sender, RoutedEventArgs e)
        {
            btnPlusZeroOne.Visibility = Visibility.Visible;
            btnMinusZeroOne.Visibility = Visibility.Visible;
            btnPlusOne.Visibility = Visibility.Visible;
            btnMinusOne.Visibility = Visibility.Visible;
            txtWartosc.Visibility = Visibility.Visible;

            KalibracjaDouble = KalibracjaTemperatura;
            txtWartosc.Text = KalibracjaDouble.ToString();


            wybrano = 1;
        }

        private void rdOpad_Click(object sender, RoutedEventArgs e)
        {
            btnPlusZeroOne.Visibility = Visibility.Visible;
            btnMinusZeroOne.Visibility = Visibility.Visible;
            btnPlusOne.Visibility = Visibility.Visible;
            btnMinusOne.Visibility = Visibility.Visible;
            txtWartosc.Visibility = Visibility.Visible;

            KalibracjaDouble = KalibracjaSzansaWystapieniaOpadowProc;
            txtWartosc.Text = KalibracjaDouble.ToString();
            wybrano = 2;
        }

        private void rdWiat_Click(object sender, RoutedEventArgs e)
        {
            btnPlusZeroOne.Visibility = Visibility.Visible;
            btnMinusZeroOne.Visibility = Visibility.Visible;
            btnPlusOne.Visibility = Visibility.Visible;
            btnMinusOne.Visibility = Visibility.Visible;
            txtWartosc.Visibility = Visibility.Visible;
            KalibracjaDouble = KalibracjaPredkoscWiatru;
            txtWartosc.Text = KalibracjaDouble.ToString();
            wybrano = 3;
        }
    }
}
