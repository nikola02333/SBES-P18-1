using System;
using Common;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using System.ServiceModel;
using System.Threading;
using System.Collections.Concurrent;
using WcfService3;
using SecurityManager;

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
            List<User> deserializedList = new List<User>();

            using (FileStream stream = File.OpenRead("Users.xml"))
            {
                deserializedList = (List<User>)serializer.Deserialize(stream);
            }
            foreach (var item in deserializedList)
            {
                if (item.Username == u.Username && item.Pass == u.Pass)
                {
                    item.IsAuthenticated = true;
                    authenticated = true;
                    Console.WriteLine("Uspeso Logovanje na nalog {0} sa pravima {1}.", u.Username, u.Type);
                }
            }
            using (FileStream stream = File.OpenWrite("Users.xml"))
            {
                serializer.Serialize(stream, deserializedList);
            }
            return authenticated;

        }
        public bool AddEntyty(Brojilo brojilo)
        {


            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(Permissions.addentity.ToString()))
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
                         new XElement("Prezime", brojilo.Prezime),
                           new XElement("Potrosnja", brojilo.Potrosnja)
                       ));
                    xDocument.Save("Baza.xml");
                    return true;
                }
            }
            else
            {
                //audit faild
            }
            Console.WriteLine("Zavrsio sam sa upisivanjem podataka u fajl.");
            return true;
        }


        public bool RemoveEntyty(Brojilo counter)
        {
            
            bool success_remove = false;
            List<Brojilo> counters = new List<Brojilo>();
            counters = ReadXMLCounters();
            var itemToRemove = counters.Single(r => r.Id == counter.Id);
            success_remove = counters.Remove(itemToRemove);
            SaveCountersToXml(counters);

            return success_remove; 
        }



        public bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld)
        {
            List<Brojilo> couters = ReadXMLCounters();
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

        public bool ChangeValueBrojila(string id, string potrosnja)
        {
            bool changed = false;

            Brojilo counter = new Brojilo();
            int id1 = Int32.Parse(id);

            counter = ReadCounterFromXml(id1);
            
            counter.Potrosnja = potrosnja;
            changed = true;

            List<Brojilo> listCounters = new List<Brojilo>();

            listCounters = ReadXMLCounters();
            listCounters.Add(counter);

            SaveCountersToXml(listCounters);

            return changed;

        }


        public bool ChangeIdBrojila(string newId, string oldId)
        {
            bool changed = false;

            Brojilo counter = new Brojilo();
            int stariId = Int32.Parse(oldId);

            counter = ReadCounterFromXml(stariId);

            counter.Id = newId;
            changed = true;

            List<Brojilo> listCounters = new List<Brojilo>();

            listCounters = ReadXMLCounters();
            listCounters.Add(counter);

            SaveCountersToXml(listCounters);

            return changed;

        }


        #region ReadXMl
        public List<Brojilo> ReadXMLCounters()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Brojila";
            XmlSerializer serializer = new XmlSerializer(typeof(List<Brojilo>), xRoot);
            List<Brojilo> dezerializedList = new List<Brojilo>();
            using (FileStream stream = File.OpenRead("Baza.xml"))
            {
                dezerializedList = (List<Brojilo>)serializer.Deserialize(stream);
            }
            return dezerializedList;
        }


        public List<User> ReadXMLUsers()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Users";
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>), xRoot);
            List<User> dezerializedList = new List<User>();

            using (FileStream stream = File.OpenRead("Users.xml"))
            {
                dezerializedList = (List<User>)serializer.Deserialize(stream);
            }

            return dezerializedList;
        }


        public Brojilo ReadCounterFromXml(int id)
        {
            Brojilo u = null;
            List<Brojilo> deserializedList = new List<Brojilo>();

            deserializedList = ReadXMLCounters();
            u = deserializedList.Single(r => r.Id == id.ToString());

            return u;
        }


        #endregion
        
        public void SaveCountersToXml(List<Brojilo> serializedList)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Brojila";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Brojilo>));
            using (StreamWriter streamWriter = System.IO.File.CreateText("Baza.xml"))
            {
                xmlSerializer.Serialize(streamWriter, serializedList);
            }
            /// ovo jedino valja
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

        /// <summary>
        /// //////////////////////////////
        /// </summary>
        /// <returns></returns>
        private int workerCounter = 0;

       

        public double GetPotrosnja(int id)
        {
            // sad ovde  LB izvuce ceo ENTITET iz XML-a i  posalje WORKERu !!!
            Brojilo counter = ReadCounterFromXml(id);

            do
            {
                foreach (var keyValuePair in WorkerLB.workers)
                {
                   
                    if (keyValuePair.Value.Free)
                    {
                        keyValuePair.Value.Free = false;
                        IWorkerService workerChannel  = getChannelToWorker(keyValuePair.Value);

                        double cena = workerChannel.GetPrice(counter.Potrosnja);    //Sva logika za getPrice
                        keyValuePair.Value.Free = true; // oznacimo ga kao slobodnog 
                        return cena;

                    }

                }
                Thread.Sleep(1000);

            } while (true);

            return -1;

        }


       


        private IWorkerService getChannelToWorker(WorkerInformations worker)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();

            EndpointAddress myEndpoint = new EndpointAddress(worker.URL);

            ChannelFactory<IWorkerService> myChannelFactory = new ChannelFactory<IWorkerService>(myBinding, myEndpoint);

            IWorkerService workerChannel = myChannelFactory.CreateChannel();

            return workerChannel;

        }


    }
}


