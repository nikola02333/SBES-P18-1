using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Xml;
using Common;
using System.Xml.Serialization;
using System.IO;

namespace Worker
{
    public class WorkerService : IWorkerService
    {

        public List<Tarife> ReadXMLTarife()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Configuration";

            XmlSerializer serializer = new XmlSerializer(typeof(List<Tarife>), xRoot);
            List<Tarife> dezerializedList = new List<Tarife>();

            using (FileStream stream = File.OpenRead("Tarifa.xml"))
            {
                dezerializedList = (List<Tarife>)serializer.Deserialize(stream);
            }

            return dezerializedList;
        }

        public double GetPrice(string pot)
        {
            //string to int
            int potrosnja = Int32.Parse(pot);
            List<Tarife> lt = ReadXMLTarife();
            Console.WriteLine("Obradjujem proracun...");
            double cena=0;
            for(int i=0; i<lt.Count(); i++)
            {

                if (potrosnja > lt[i].GG)
                {
                    cena += (lt[i].GG - lt[i].DG) * lt[i].Cena;
                }
                else if(potrosnja > lt[i].DG)
                {
                    cena += (potrosnja - lt[i].DG) * lt[i].Cena;
                }               
            }
          
            return cena;
        }

    }
}
