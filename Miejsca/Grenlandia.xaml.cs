﻿using System;
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
using System.Xml;

namespace miniStacjaPogody
{
    /// <summary>
    /// Logika interakcji dla klasy Grenlandia.xaml
    /// </summary>
    public partial class Grenlandia : Page
    {
        Random rd = new Random();
        private Timer _timer;

        public static double tempMin = 0;
        public static double temp = 0;
        public static double tempMax = 0;
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
        public static DateTime dzisiaj = DateTime.Today;
        static bool losuj = true;
        public Grenlandia()
        {
            InitializeComponent();
            try
            {
                LadujZPliku();

            }
            catch (Exception)
            {
                Laduj();
            }
            _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void Laduj()
        {
            if (losuj)
            {
                tempMin = rd.NextDouble() * rd.Next(-70, 10);
                temp = rd.NextDouble() * rd.Next(-70, 10);
                tempMax = rd.NextDouble() * rd.Next(-70, 10);

                while (tempMin > temp || temp > tempMax)
                {
                    tempMin = rd.NextDouble() * rd.Next(-70, 10);
                    temp = rd.NextDouble() * rd.Next(-70, 10);
                    tempMax = rd.NextDouble() * rd.Next(-70, 10);
                }
                tempOdczuwalnaMin = tempMin - DataTemplate.liczenieOdczuwalnejTemp(rd);
                tempOdczuwalnaMax = tempMax + DataTemplate.liczenieOdczuwalnejTemp(rd);
                tempOdczuwalna = temp + DataTemplate.liczenieOdczuwalnejTemp(rd);

                wilgotnosc = rd.Next(0, 101) + rd.NextDouble();
                szansaWystapieniaOpadow = rd.Next(0, 101) + rd.NextDouble();
                zachmurzenie = rd.Next(0, 101) + rd.NextDouble();
                cisnienie = rd.Next(980, 1031);

                predkoscWiatru = rd.Next(0, 216) + rd.NextDouble();
                kierunekWiatru = rd.Next(0, 361);
                wschodSlonca = dzisiaj.AddHours(rd.Next(4, 11)).AddMinutes(rd.Next(0, 60)).AddSeconds(rd.Next(0, 60));
                zachodSlonca = dzisiaj.AddHours(rd.Next(18, 23)).AddMinutes(rd.Next(0, 60)).AddSeconds(rd.Next(0, 60));
                losuj = false;
            }
            grData.Text = $"Bydgoszcz {dzisiaj.ToShortDateString()}\nWschod Slonca: {wschodSlonca.ToString("HH:mm:ss")}\nZachod Slonca: {zachodSlonca.ToString("HH:mm:ss")}";
            grOpady.Text = $"Wilgotnosc: {wilgotnosc:F1}%\nSzansa wystapienia opadow: {szansaWystapieniaOpadow:F1}%\nZachmurzenie: {zachmurzenie:F1}%\nCisnienie atmosferyzcne: {cisnienie}hpa";

        }

        private void LadujZPliku()
        {
            string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Grenlandia", $"{dzisiaj.ToShortDateString()}.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(dirPath);
            XmlNode root = doc.SelectSingleNode("/DataTemplate");

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

        private void Update(object State)
        {
            UpdateTemp();
            DataTemplate.modyfikacjaRandom(ref predkoscWiatru, rd);
            DataTemplate.modyfikacjaRandom(ref kierunekWiatru, rd);
            Dispatcher.Invoke(new Action(AktualizujTekst));
        }

        private void UpdateTemp()
        {
            int temp = rd.Next(1, 301);
            bool plus = false;
            if (temp < 101)
            {
                Grenlandia.temp -= 0.1;
                plus = false;
            }
            else if (temp < 201)
            {
                //
            }
            else
            {
                Grenlandia.temp += 0.1;
                plus = true;
            }
            Grenlandia.temp = DataTemplate.SprawdzTemp(Grenlandia.temp, tempMin, tempMax);

            temp = rd.Next(0, 3);
            if (temp == 2)
            {
                if (plus)
                {
                    tempOdczuwalna += 0.1;
                }
                else
                {
                    tempOdczuwalna -= 0.1;
                }
                tempOdczuwalna = DataTemplate.SprawdzTemp(Grenlandia.tempOdczuwalna, tempOdczuwalnaMin, tempOdczuwalnaMax);
            }
        }

        private void AktualizujTekst()
        {
            grTemp.Text = $"Temperatura minimalna: {tempMin:F1}°C\nTemperatura maksymalna: {tempMax:F1}°C\nTemperatura aktualna: {temp:F1}°C\n\nTemperatura odczuwalna minimalna: {tempOdczuwalnaMin:F1}°C\nTemperatura odczuwalna maksymalna: {tempOdczuwalnaMax:F1}°C\nTemperatura odczuwalna aktualna: {tempOdczuwalna:F1}°C";
            grWiatr.Text = $"Predkosc wiatru: {predkoscWiatru:F1}km/h\nKierunek wiatru: {kierunekWiatru}°";
        }





        private void powrot(object sender, RoutedEventArgs e)
        {
            DataTemplate.Zapisz(this);
            MainWindow.Powrot();
            _timer.Dispose();
        }

        private void admin(object sender, RoutedEventArgs e)
        {
            DataTemplate.Zapisz(this);
            MainWindow.navPanelAdministracyjny();
            _timer.Dispose();
        }
    }
}
