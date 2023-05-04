using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
            int temp = rd.Next(1, 4);
            if (temp == 2)
            {
                temp = rd.Next(1, 301);
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
        }





        public static void Zapisz(int i, DateTime data)
        {
            if (i == 0)
            {
            string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bydgoszcz");
            Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, $"{data.ToShortDateString()}.xml");

                if (data != DateTime.Today && !File.Exists(filePath))
                {
                    Bydgoszcz.tempMin = 0;
                    Bydgoszcz.temp = 0;
                    Bydgoszcz.tempMax = 0;
                    Bydgoszcz.tempOdczuwalna = 0;
                    Bydgoszcz.tempOdczuwalnaMin = 0;
                    Bydgoszcz.tempOdczuwalnaMax = 0;
                    Bydgoszcz.wilgotnosc = 0;
                    Bydgoszcz.szansaWystapieniaOpadow = 0;
                    Bydgoszcz.zachmurzenie = 0;
                    Bydgoszcz.predkoscWiatru = 0;
                    Bydgoszcz.kierunekWiatru = 0;
                    Bydgoszcz.cisnienie = 0;
                    Bydgoszcz.wschodSlonca = data;
                    Bydgoszcz.zachodSlonca = data;
                }
            using (var writer = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(DataTemplate));
                serializer.Serialize(writer, new DataTemplate
                {
                    Miejsce = "Bydgoszcz",
                    Data = data,
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

            Poprawka(filePath);
            }
            else if (i == 1)
            {
                string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Egipt");
                Directory.CreateDirectory(dirPath);

                string filePath = Path.Combine(dirPath, $"{data.ToShortDateString()}.xml");


                //
                if (data != DateTime.Today && !File.Exists(filePath))
                {
                    Egipt.tempMin = 0;
                    Egipt.temp = 0;
                    Egipt.tempMax = 0;
                    Egipt.tempOdczuwalna = 0;
                    Egipt.tempOdczuwalnaMin = 0;
                    Egipt.tempOdczuwalnaMax = 0;
                    Egipt.wilgotnosc = 0;
                    Egipt.szansaWystapieniaOpadow = 0;
                    Egipt.zachmurzenie = 0;
                    Egipt.predkoscWiatru = 0;
                    Egipt.kierunekWiatru = 0;
                    Egipt.cisnienie = 0;
                    Egipt.wschodSlonca = data;
                    Egipt.zachodSlonca = data;
                }


                using (var writer = new StreamWriter(filePath))
                {
                    var serializer = new XmlSerializer(typeof(DataTemplate));
                    serializer.Serialize(writer, new DataTemplate
                    {
                        Miejsce = "Egipt",
                        Data = data,
                        Wschod_Slonca = Egipt.wschodSlonca,
                        Zachod_Slonca = Egipt.zachodSlonca,
                        Temperatura_Minimalna = Egipt.tempMin,
                        Temperatura_Aktualna = Egipt.temp,
                        Temperatura_Maksymalna = Egipt.tempMax,
                        Temperatura_Odczuwalna = Egipt.tempOdczuwalna,
                        Temperatura_OdczuwalnaMin = Egipt.tempOdczuwalnaMin,
                        Temperatura_OdczuwalnaMax = Egipt.tempOdczuwalnaMax,
                        WilgotnoscProc = Egipt.wilgotnosc,
                        Szansa_Wystapienia_OpadowProc = Egipt.szansaWystapieniaOpadow,
                        ZachmurzenieProc = Egipt.zachmurzenie,
                        Predkosc_Wiatru_kmh = Egipt.predkoscWiatru,
                        Kierunek_Wiatru = Egipt.kierunekWiatru,
                        Cisnienie_Atmosferyczne = Egipt.cisnienie
                    });
                }

                Poprawka(filePath);
            }
            else
            {
                string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Grenlandia");
                Directory.CreateDirectory(dirPath);

                string filePath = Path.Combine(dirPath, $"{data.ToShortDateString()}.xml");

                //
                if (data != DateTime.Today && !File.Exists(filePath))
                {
                    Grenlandia.tempMin = 0;
                    Grenlandia.temp = 0;
                    Grenlandia.tempMax = 0;
                    Grenlandia.tempOdczuwalna = 0;
                    Grenlandia.tempOdczuwalnaMin = 0;
                    Grenlandia.tempOdczuwalnaMax = 0;
                    Grenlandia.wilgotnosc = 0;
                    Grenlandia.szansaWystapieniaOpadow = 0;
                    Grenlandia.zachmurzenie = 0;
                    Grenlandia.predkoscWiatru = 0;
                    Grenlandia.kierunekWiatru = 0;
                    Grenlandia.cisnienie = 0;
                    Grenlandia.wschodSlonca = data;
                    Grenlandia.zachodSlonca = data;
                }


                using (var writer = new StreamWriter(filePath))
                {
                    var serializer = new XmlSerializer(typeof(DataTemplate));
                    serializer.Serialize(writer, new DataTemplate
                    {
                        Miejsce = "Grenlandia",
                        Data = data,
                        Wschod_Slonca = Grenlandia.wschodSlonca,
                        Zachod_Slonca = Grenlandia.zachodSlonca,
                        Temperatura_Minimalna = Grenlandia.tempMin,
                        Temperatura_Aktualna = Grenlandia.temp,
                        Temperatura_Maksymalna = Grenlandia.tempMax,
                        Temperatura_Odczuwalna = Grenlandia.tempOdczuwalna,
                        Temperatura_OdczuwalnaMin = Grenlandia.tempOdczuwalnaMin,
                        Temperatura_OdczuwalnaMax = Grenlandia.tempOdczuwalnaMax,
                        WilgotnoscProc = Grenlandia.wilgotnosc,
                        Szansa_Wystapienia_OpadowProc = Grenlandia.szansaWystapieniaOpadow,
                        ZachmurzenieProc = Grenlandia.zachmurzenie,
                        Predkosc_Wiatru_kmh = Grenlandia.predkoscWiatru,
                        Kierunek_Wiatru = Grenlandia.kierunekWiatru,
                        Cisnienie_Atmosferyczne = Grenlandia.cisnienie
                    });
                }
                Poprawka(filePath);
            }
        }

        public static void Poprawka(string nazwa)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(nazwa);

            XmlElement root = doc.DocumentElement;

            foreach (XmlNode node in root.ChildNodes)
            {
                if (node is XmlElement)
                {
                    XmlElement element = (XmlElement)node;
                    element.InnerText = element.InnerText.Replace('.', ',');
                }
            }
            doc.Save(nazwa);
        }


    }
}
