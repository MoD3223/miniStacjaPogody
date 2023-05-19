using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static void ZapiszKalibracje(string lokacja)
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
                DataTemplate.Poprawka(dirPath);
            }
                

        }


    }
}
