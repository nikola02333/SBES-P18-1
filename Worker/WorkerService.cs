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
    public class WorkerService : IWorkerRoleService
    {
        public static List<Tarife> tarife = new List<Tarife>();

        public WorkerService()
        {
            tarife= ReadXMLTarife();
        }

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

        public double Price(int pot, List<Tarife> lt)
        {
            double cena=0;
            for(int i=0; i<lt.Count(); i++)
            {

                if (pot > lt[i].GG)
                {
                    cena += (lt[i].GG - lt[i].DG) * lt[i].Cena;
                }
                else if(pot > lt[i].DG)
                {
                    cena += (pot - lt[i].DG) * lt[i].Cena;
                }               
            }

            return cena;
        }

        public int Work(int id)
        {
            throw new NotImplementedException();
        }

        public void StartId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
