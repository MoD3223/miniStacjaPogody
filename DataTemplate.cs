using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace miniStacjaPogody
{
    public class DataTemplate
    {
        public string Miejsce { get; set; }
        public double Temperatura_Minimalna { get; set; }
        public double Temperatura_Aktualna { get; set; }
        public double Temperatura_Maksymalna { get; set; }
        public double Temperatura_Odczuwalna { get; set; }
        public double Temperatura_OdczuwalnaMin { get; set; }
        public double Temperatura_OdczuwalnaMax { get; set; }
        public double WilgotnoscProc { get; set; }
        public double Szansa_Wystapienia_OpadowProc { get; set; }
        public double ZachmurzenieProc { get; set; }
        public double Predkosc_Wiatru_kmh { get; set; }
        public int Kierunek_Wiatru { get; set; }
        public int Cisnienie_Atmosferyczne { get; set; }
        public DateTime Wschod_Slonca { get; set; }
        public DateTime Zachod_Slonca { get; set; }
        public DateTime Data { get; set; }


        public static double SprawdzTemp(double tempPrzekazany, double tempMin, double tempMax)
        {
            if (tempPrzekazany < tempMin)
            {
                tempPrzekazany = tempMin;
            }
            else if (tempPrzekazany > tempMax)
            {
                tempPrzekazany = tempMax;
            }
            return tempPrzekazany;
        }

        public static int liczenieOdczuwalnejTemp(Random rd)
        {
            int x = rd.Next(-13, 14);
            if (x > -10 && x < 10)
            {
                x = 0;
            }
            if (x < 0)
            {
                x += 10;
            }
            else if (x > 0)
            {
                x -= 10;
            }
            return x;
        }

        public static void modyfikacjaRandom(ref double x, Random rd)
        {
            int temp = rd.Next(1, 301);
            if (temp < 101)
            {
                x -= 0.1;
            }
            else if (temp < 201)
            {
                //
            }
            else
            {
                x += 0.1;
            }
        }

        public static void modyfikacjaRandom(ref int x, Random rd)
        {
            int temp = rd.Next(1, 301);
            if (temp < 101)
            {
                x -= 1;
            }
            else if (temp < 201)
            {
                //
            }
            else
            {
                x += 1;
            }
        }

        public static void Zapisz(Bydgoszcz bd)
        {
            string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bydgoszcz");
            Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, $"{Bydgoszcz.dzisiaj.ToShortDateString()}.xml");

            using (var writer = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(DataTemplate));
                serializer.Serialize(writer, new DataTemplate
                {
                    Miejsce = "Bydgoszcz",
                    Data = Bydgoszcz.dzisiaj,
                    Wschod_Slonca = Bydgoszcz.wschodSlonca,
                    Zachod_Slonca = Bydgoszcz.zachodSlonca,
                    Temperatura_Minimalna = Bydgoszcz.tempMin,
                    Temperatura_Aktualna = Bydgoszcz.temp,
                    Temperatura_Maksymalna = Bydgoszcz.tempMax,
                    Temperatura_Odczuwalna = Bydgoszcz.tempOdczuwalna,
                    Temperatura_OdczuwalnaMin = Bydgoszcz.tempOdczuwalnaMin,
                    Temperatura_OdczuwalnaMax = Bydgoszcz.tempOdczuwalnaMax,
                    WilgotnoscProc = Bydgoszcz.wilgotnosc,
                    Szansa_Wystapienia_OpadowProc = Bydgoszcz.szansaWystapieniaOpadow,
                    ZachmurzenieProc = Bydgoszcz.zachmurzenie,
                    Predkosc_Wiatru_kmh = Bydgoszcz.predkoscWiatru,
                    Kierunek_Wiatru = Bydgoszcz.kierunekWiatru,
                    Cisnienie_Atmosferyczne = Bydgoszcz.cisnienie
                });
            }
        }







    }
}
