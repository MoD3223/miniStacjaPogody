using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace miniStacjaPogody
{
    public class Kalibracja
    {
        public double KalibracjaTemperatura { get; set; }
        public double KalibracjaTemperaturaOdczuwalna { get; set; }
        public double KalibracjaWilgotnoscProc { get; set; }
        public double KalibracjaSzansaWystapieniaOpadowProc { get; set; }
        public double KalibracjaZachmurzenieProc { get; set; }
        public double KalibracjaPredkoscWiatru { get; set; }
        public int KalibracjaKierunekWiatru { get; set; }
        public int KalibracjaCisnienie { get; set; }

        public static void ZapiszKalibracje(string lokacja, int ID, double wartosc)
        {
            string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lokacja, "Kalibracja.xml");

            if (!File.Exists(dirPath))
            {
                using (var writer = new StreamWriter(dirPath))
                {

                    var serializer = new XmlSerializer(typeof(Kalibracja));
                    serializer.Serialize(writer, new Kalibracja
                    {
                        KalibracjaTemperatura = 0,
                        KalibracjaTemperaturaOdczuwalna = 0,
                        KalibracjaWilgotnoscProc = 0,
                        KalibracjaSzansaWystapieniaOpadowProc = 0,
                        KalibracjaZachmurzenieProc = 0,
                        KalibracjaPredkoscWiatru = 0,
                        KalibracjaKierunekWiatru = 0,
                        KalibracjaCisnienie = 0
                    });
                }
            }


            if (ID == 1)
            {
                using (var writer = new StreamWriter(dirPath))
                {

                    var serializer = new XmlSerializer(typeof(Kalibracja));
                    serializer.Serialize(writer, new Kalibracja
                    {
                        KalibracjaTemperatura = wartosc,
                        KalibracjaTemperaturaOdczuwalna = wartosc
                    });
                }

            }
            else if (ID == 2)
            {
                using (var writer = new StreamWriter(dirPath))
                {

                    var serializer = new XmlSerializer(typeof(Kalibracja));
                    serializer.Serialize(writer, new Kalibracja
                    {
                        KalibracjaWilgotnoscProc = wartosc,
                        KalibracjaSzansaWystapieniaOpadowProc = wartosc,
                        KalibracjaZachmurzenieProc = wartosc
                    });
                }
            }
            else if (ID == 3)
            {
                using (var writer = new StreamWriter(dirPath))
                {

                    var serializer = new XmlSerializer(typeof(Kalibracja));
                    serializer.Serialize(writer, new Kalibracja
                    {
                        KalibracjaPredkoscWiatru = wartosc,
                        KalibracjaKierunekWiatru = Int32.Parse(wartosc.ToString()),
                        KalibracjaCisnienie = Int32.Parse(wartosc.ToString())
                    });
                }
            }
            DataTemplate.Poprawka(dirPath);


        }

    }
}
