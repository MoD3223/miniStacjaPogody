using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
        static List<int> godz = new List<int> { 0,1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        static List<int> min = Enumerable.Range(0, 60).ToList();
        static List<int> listTemperatura = Enumerable.Range(-100, 201).ToList();
        static List<int> listDecimal = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        static List<int> listProc = Enumerable.Range(0,100).ToList();
        static List<int> listHPA = Enumerable.Range(900, 301).ToList();
        static List<int> listKierunek = Enumerable.Range(0, 360).ToList();
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
            Widocznosc(System.Windows.Visibility.Hidden);
            wybor.ItemsSource = list;
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
            string temp;
            double temperatura,temperaturaMin,temperaturaMax;
            temperatura = PanelAdministracyjny.temp;
            temperaturaMin = PanelAdministracyjny.tempMin;
            temperaturaMax = PanelAdministracyjny.tempMax;

            try
            {
                temp = CtempMin1.SelectedItem.ToString() + "," + CtempMin2.SelectedItem.ToString();
                Double.TryParse(temp, out temperaturaMin);
                temp = String.Empty;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                temp = CtempMax1.SelectedItem.ToString() + "," + CtempMax2.SelectedItem.ToString();
                Double.TryParse(temp, out temperaturaMax);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }

            try
            {
                temp = CtempAktualna1.SelectedItem.ToString() + "," + CtempAktualna2.SelectedItem.ToString();
                Double.TryParse(temp, out temperatura);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }


            if (temperaturaMin > temperatura || temperatura > temperaturaMax)
            {
                temperaturaMin = PanelAdministracyjny.tempMin;
                temperatura = PanelAdministracyjny.temp;
                temperaturaMax = PanelAdministracyjny.tempMax;
                operacja.Text = "Podano zla temperature!";
                operacja.Foreground = Brushes.Red;
            }
            else
            {
                PanelAdministracyjny.temp = temperatura;
                PanelAdministracyjny.tempMin = temperaturaMin;
                PanelAdministracyjny.tempMax = temperaturaMax;
            }


            temperatura = PanelAdministracyjny.tempOdczuwalna;
            temperaturaMax = PanelAdministracyjny.tempOdczuwalnaMax;
            temperaturaMin = PanelAdministracyjny.tempOdczuwalnaMin;

            try
            {
                temp = CtempOAkt1.SelectedItem.ToString() + "," + CtempOAkt2.SelectedItem.ToString();
                Double.TryParse(temp, out temperatura);
                temp = String.Empty;
            }
            catch (Exception)
            {
            }

            try
            {
                temp = CtempOMin1.SelectedItem.ToString() + "," + CtempOMin2.SelectedItem.ToString();
                Double.TryParse(temp, out temperaturaMin);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }

            try
            {
                temp = CtempOMax1.SelectedValue.ToString() + "," + CtempOMax2.SelectedValue.ToString();
                Double.TryParse(temp, out temperaturaMax);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }
            
            


            if (temperaturaMin > temperatura || temperatura > temperaturaMax)
            {
                operacja.Text = "Podano zla temperature odczuwalna!";
                operacja.Foreground = Brushes.Red;
            }
            else
            {
                PanelAdministracyjny.tempOdczuwalna = temperatura;
                PanelAdministracyjny.tempOdczuwalnaMax = temperaturaMax;
                PanelAdministracyjny.tempOdczuwalnaMin = temperaturaMin;
            }

            try
            {
                temp = CWilg1.SelectedValue.ToString() + "," + CWilg2.SelectedValue.ToString();
                Double.TryParse(temp, out wilgotnosc);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }

            try
            {
                temp = COpad1.SelectedValue.ToString() + "," + COpad2.SelectedValue.ToString();
                Double.TryParse(temp, out szansaWystapieniaOpadow);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }

            try
            {
                temp = CZach1.SelectedValue.ToString() + "," + CZach2.SelectedValue.ToString();
                Double.TryParse(temp, out zachmurzenie);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }

            try
            {
                temp = CPre1.SelectedValue.ToString() + "," + CPre2.SelectedValue.ToString();
                Double.TryParse(temp, out predkoscWiatru);
                temp = String.Empty;
            }
            catch (Exception)
            {

            }

            try
            {
                Int32.TryParse(CCis.SelectedValue.ToString(), out cisnienie);
            }
            catch (Exception)
            {

            }

            try
            {
                Int32.TryParse(CKie.SelectedValue.ToString(), out kierunekWiatru);
            }
            catch (Exception)
            {

            }


            DateTime wschodSlonca = PanelAdministracyjny.wschodSlonca;
            DateTime zachodSlonca = PanelAdministracyjny.zachodSlonca;

            try
            {
                DateTime.TryParseExact(HourPick2.SelectedValue.ToString(), "H", CultureInfo.InvariantCulture, DateTimeStyles.None, out wschodSlonca);
            }
            catch (Exception)
            {

            }

            try
            {
                DateTime.TryParseExact(HourPick.SelectedValue.ToString(), "H", CultureInfo.InvariantCulture, DateTimeStyles.None, out zachodSlonca);
            }
            catch (Exception)
            {

            }

            try
            {
                DateTime.TryParseExact(MinPick.SelectedValue.ToString(), "m", CultureInfo.InvariantCulture, DateTimeStyles.None, out wschodSlonca);
            }
            catch (Exception)
            {

            }

            try
            {
                DateTime.TryParseExact(MinPick2.SelectedValue.ToString(), "m", CultureInfo.InvariantCulture, DateTimeStyles.None, out zachodSlonca);
            }
            catch (Exception)
            {

            }

            try
            {
                DateTime.TryParseExact(SecPick.SelectedValue.ToString(), "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out wschodSlonca);
            }
            catch (Exception)
            {

            }

            try
            {
                DateTime.TryParseExact(SecPick2.SelectedValue.ToString(), "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out zachodSlonca);
            }
            catch (Exception)
            {

            }
            
            



            TimeSpan ts = wschodSlonca - zachodSlonca;
            if (ts < TimeSpan.Zero)
            {
                operacja.Text = "Podano zla date wschodu/zachodu!";
                operacja.Foreground = Brushes.Red;
            }
            else
            {
                PanelAdministracyjny.wschodSlonca = wschodSlonca;
                PanelAdministracyjny.zachodSlonca = zachodSlonca;
            }

            if (odkryty)
            {
            selectedDate = (DateTime)DatePick.SelectedDate;
            if (wybrano == 0)
            {
                DataTemplate.Zapisz(10, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            else if (wybrano == 1)
            {
                DataTemplate.Zapisz(11, selectedDate);
                operacja.Text = "Zapisano plik!";
                operacja.Foreground = Brushes.Green;
            }
            else if (wybrano == 2)
            {
                DataTemplate.Zapisz(12, selectedDate);
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
            PopulateList();
            Widocznosc(Visibility.Visible);
        }

        private void PopulateList()
        {
            HourPick.ItemsSource = godz;
            HourPick2.ItemsSource = godz;
            MinPick.ItemsSource = min;
            MinPick2.ItemsSource = min;
            SecPick.ItemsSource = min;
            SecPick2.ItemsSource = min;
            CtempMin1.ItemsSource = listTemperatura;
            CtempMin2.ItemsSource = listDecimal;
            CtempMax1.ItemsSource = listTemperatura;
            CtempMax2.ItemsSource = listDecimal;
            CtempAktualna1.ItemsSource = listTemperatura;
            CtempAktualna2.ItemsSource = listDecimal;
            CtempOAkt1.ItemsSource = listTemperatura;
            CtempOAkt2.ItemsSource = listDecimal;
            CtempOMax1.ItemsSource = listTemperatura;
            CtempOMax2.ItemsSource = listDecimal;
            CtempOMin1.ItemsSource = listTemperatura;
            CtempOMin2.ItemsSource = listDecimal;
            CWilg1.ItemsSource = listProc;
            CWilg2.ItemsSource = listDecimal;
            COpad1.ItemsSource = listProc;
            COpad2.ItemsSource = listDecimal;
            CZach1.ItemsSource = listProc;
            CZach2.ItemsSource = listDecimal;
            CCis.ItemsSource = listHPA;
            CPre1.ItemsSource = listProc;
            CPre2.ItemsSource = listDecimal;
            CKie.ItemsSource = listKierunek;

        }

        private void Widocznosc(System.Windows.Visibility b)
        {
            HourPick.Visibility = b;
            HourPick2.Visibility = b;
            MinPick.Visibility = b;
            MinPick2.Visibility = b;
            SecPick.Visibility = b;
            SecPick2.Visibility = b;
            CtempMin1.Visibility = b;
            CtempMin2.Visibility = b;
            CtempMax1.Visibility = b;
            CtempMax2.Visibility = b;
            CtempAktualna1.Visibility = b;
            CtempAktualna2.Visibility = b;
            CtempOAkt1.Visibility = b;
            CtempOAkt2.Visibility = b;
            CtempOMax1.Visibility = b;
            CtempOMax2.Visibility = b;
            CtempOMin1.Visibility = b;
            CtempOMin2.Visibility = b;
            CWilg1.Visibility = b;
            CWilg2.Visibility = b;
            COpad1.Visibility = b;
            COpad2.Visibility = b;
            CZach1.Visibility = b;
            CZach2.Visibility = b;
            CCis.Visibility = b;
            CPre1.Visibility = b;
            CPre2.Visibility = b;
            CKie.Visibility = b;
            txtblock1.Visibility = b;
            txtblock2.Visibility = b;
            txtBlock3.Visibility = b;
            txtBlock4.Visibility = b;
            txtblock5.Visibility = b;
            txtblock6.Visibility = b;
            txtblock7.Visibility = b;
            txtblock8.Visibility = b;
            txtblock9.Visibility = b;
            txtblock10.Visibility = b;
            txtblock11.Visibility = b;
            txtblock12.Visibility = b;
            txtblock13.Visibility = b;
            txtblock14.Visibility = b;
        }
    }
}
