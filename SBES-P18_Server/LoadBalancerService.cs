using System;
using Common;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SBES_P18_Server
{
    public class LoadBalancerService : ILoadBalancerService
    {
        public bool Login(User u)
        {
            bool authenticated = false;
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Users";
            // xRoot.Namespace = "http://www.cpandl.com";
            xRoot.IsNullable = true;

            XmlSerializer serializer = new XmlSerializer(typeof(List<User>), xRoot);
            List<User> dezerializedList = new List<User>();

            using (FileStream stream = File.OpenRead("Users.xml"))
            {
               dezerializedList = (List<User>)serializer.Deserialize(stream);
            }
         
            foreach (var item in dezerializedList)
            {
               if(item.Username == u.Username && item.Pass == u.Pass)
                {
                    item.IsAuthenticated = true;
                    authenticated = true;
                    Console.WriteLine("Uspeso Logovanje na nalog {0} sa pravima {1}.", u.Username, u.Type);
                }
               
            }
            using (FileStream stream = File.OpenWrite("Users.xml"))
            {
                
                serializer.Serialize(stream, dezerializedList);
            }

            return authenticated;
            
         }

        public List<Brojilo> ReadXML()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Brojila";
            // xRoot.Namespace = "http://www.cpandl.com";
          //  xRoot.IsNullable = true;
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<Brojilo>), xRoot);
            List<Brojilo> dezerializedList = new List<Brojilo>();

            using (FileStream stream = File.OpenRead("Baza.xml"))
            {
                dezerializedList = (List<Brojilo>)serializer.Deserialize(stream);
            }

            return dezerializedList;
         }

        public bool AddEntyty(Brojilo brojilo)
        {
            if (!File.Exists("Baza.xml"))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create("Baza.xml", xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Brojila");
                    
                    xmlWriter.WriteStartElement("Brojilo");
                    xmlWriter.WriteElementString("Id", brojilo.Id);
                    xmlWriter.WriteElementString("Ime", brojilo.Ime);
                    xmlWriter.WriteElementString("Prezime", brojilo.Prezime);
                    xmlWriter.WriteElementString("Potrosnja", brojilo.Potrosnja);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load("Baza.xml");
                XElement root = xDocument.Element("Brojila");
                IEnumerable<XElement> rows = root.Descendants("Brojilo");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                   new XElement("Brojilo",
                   new XElement("Id", brojilo.Id),
                   new XElement("Ime", brojilo.Ime),
                     new XElement("Prezime",brojilo.Prezime),
                       new XElement("Potrosnja", brojilo.Potrosnja)
                   ));
                xDocument.Save("Baza.xml");
            }
            Console.WriteLine("Zavrsio sam sa upisivanjem podataka u fajl.");
            return true;
        }
        public bool RemoveEntyty(Brojilo counter)
        {
            bool success_remove = false;
            List<Brojilo> counters = new List<Brojilo>();
            counters = ReadXML();
            var itemToRemove = counters.Single(r => r.Id == counter.Id);
            counters.Remove(itemToRemove);
            SaveCountersToXml(counters); 
            return success_remove;
        }
        public bool Work()
        {
            throw new NotImplementedException();
        }
        public bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld)
        {
            List<Brojilo> couters = ReadXML();
            foreach (var item in couters)
            {
                if (item.Id == counterOld.Id)
                {
                    item.Id = counterNew.Id;
                    item.Potrosnja = counterNew.Potrosnja;
                }
            }
            return true;
        }
        #region SaveToXML
        public void SaveCountersToXml(List<Brojilo> dezerializedList)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Brojila";
            xRoot.IsNullable = true;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Brojilo>), xRoot);
            using (FileStream stream = File.OpenWrite("Baza.xml"))
            {
                serializer.Serialize(stream, dezerializedList);
            }
        }
        public void SaveEntityToXml(List<User> dezerializedList)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Users";
            xRoot.IsNullable = true;
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>), xRoot);
            using (FileStream stream = File.OpenWrite("Users.xml"))
            {
                serializer.Serialize(stream, dezerializedList);
            }
        }
        #endregion

    }
}

