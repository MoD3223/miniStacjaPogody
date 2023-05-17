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
    internal class Kalibracja
    {
        public static double KalibracjaTemperatura;
        public static double KalibracjaTemperaturaOdczuwalna;
        public static double KalibracjaWilgotnoscProc;
        public static double KalibracjaSzansaWystapieniaOpadowProc;
        public static double KalibracjaZachmurzenieProc;
        public static double KalibracjaPredkoscWiatru;
        public static int KalibracjaKierunekWiatru;
        public static int KalibracjaCisnienie;

        public static void CzytajKalibracje(string lokacja)
        {
            string dirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lokacja, "Kalibracja.xml");

            if (!File.Exists(dirPath))
            {
                using (var writer = new StreamWriter(dirPath))
                {
                    var serializer = new XmlSerializer(typeof(Kalibracja));
                    serializer.Serialize(writer, new Kalibracja
                    {
                        
                    });
                }
                DataTemplate.Poprawka(dirPath);
            }
                XmlDocument doc = new XmlDocument();
                doc.Load(dirPath);
                XmlNode root = doc.SelectSingleNode("/DataTemplate");
            //tempMin = Double.Parse(root.SelectSingleNode("Temperatura_Minimalna").InnerText);
            KalibracjaTemperatura = Double.Parse(root.SelectSingleNode("KalibracjaTemperatura").InnerText);

        }


    }
}
