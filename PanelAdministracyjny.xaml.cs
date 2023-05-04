using System;
using System.Collections.Generic;
using System.Linq;
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
        static List<int> godz = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        static List<int> min = Enumerable.Range(1, 59).ToList();
        static List<int> listTemperatura = Enumerable.Range(-100, 200).ToList();
        static List<int> listTempDecimal = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        string selected;
        DateTime selectedDate;
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
        static bool odkryty = false;
        public Timer _timer;







        public PanelAdministracyjny()
        {
            InitializeComponent();
            wybor.ItemsSource = list;
            HourPick.ItemsSource = godz;
            wybor.SelectedIndex = 0;
            DatePick.SelectedDate = DateTime.Today;
            _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = wybor.SelectedItem.ToString();
            odkryty = false;
        }

        private void Szukaj_Click(object sender, RoutedEventArgs e)
        {
                selectedDate = (DateTime)DatePick.SelectedDate;
                shortDate = selectedDate.ToShortDateString();

            if (wybor.SelectedIndex == 0)
            {
                try
                {
                    string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bydgoszcz", $"{shortDate}.xml");
                    LadujZPliku(dirPath);
                }
                catch (Exception)
                {
                    DataTemplate.Zapisz(0,selectedDate);
                    Szukaj_Click(this, e);
                }
            }
            else if (wybor.SelectedIndex == 1)
            {
                try
                {
                    string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Egipt", $"{shortDate}.xml");
                    LadujZPliku(dirPath);
                }
                catch (Exception)
                {
                    DataTemplate.Zapisz(1,selectedDate);
                    Szukaj_Click(this, e);
                }
            }
            else
            {
                try
                {
                    string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Grenlandia", $"{shortDate}.xml");
                    LadujZPliku(dirPath);
                }
                catch (Exception)
                {
                    DataTemplate.Zapisz(2,selectedDate);
                    Szukaj_Click(this, e);
                }
            }
            odkryty = true;


            LokiData.Text = "Lokalizacja i data";
            Temperatur.Text = "Temperatura";
            Opad.Text = "Opady";
            Wiat.Text = "Wiatr";
            txtBlockData.Text = $"Bydgoszcz {selectedDate.ToShortDateString()}\nWschod Slonca: {wschodSlonca.ToString("HH:mm:ss")}\nZachod Slonca: {zachodSlonca.ToString("HH:mm:ss")}";
            txtBlockOpady.Text = $"Wilgotnosc: {wilgotnosc:F1}%\nSzansa wystapienia opadow: {szansaWystapieniaOpadow:F1}%\nZachmurzenie: {zachmurzenie:F1}%\nCisnienie atmosferyzcne: {cisnienie}hpa";
            txtBlockTemp.Text = $"Temperatura minimalna: {tempMin:F1}°C\nTemperatura maksymalna: {tempMax:F1}°C\nTemperatura aktualna: {temp:F1}°C\n\nTemperatura odczuwalna minimalna: {tempOdczuwalnaMin:F1}°C\nTemperatura odczuwalna maksymalna: {tempOdczuwalnaMax:F1}°C\nTemperatura odczuwalna aktualna: {tempOdczuwalna:F1}°C";
            txtBlockWiatr.Text = $"Predkosc wiatru: {predkoscWiatru:F1}km/h\nKierunek wiatru: {kierunekWiatru}°";


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
            if (odkryty)
            {
            selectedDate = (DateTime)DatePick.SelectedDate;
            if (wybrano == 0)
            {
                DataTemplate.Zapisz(0, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            else if (wybrano == 1)
            {
                DataTemplate.Zapisz(1, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            else if (wybrano == 2)
            {
                DataTemplate.Zapisz(1, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            }
            else
            {
                operacja.Text = "Zmieniono parametry, prosze wyszukac ponownie przed proba zapisania!";
                operacja.Foreground = Brushes.Red;
            }
        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            odkryty = false;
        }

        private void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            savebtn.IsEnabled = true;
        }

        private void HourPick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int hour = (int)HourPick.SelectedValue;
            //
            
        }

        private void HourPick2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int hour = (int)HourPick2.SelectedValue;
        }
    }
}
