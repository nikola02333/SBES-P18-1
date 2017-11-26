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
using SecurityManager;

namespace SBES_P18_Server
{
    public class LoadBalancerService : ILoadBalancerService
    {
        static string[] _listID = new string[15];
        static string[] Block_listID = new string[15];

        static int id_counter = 0;
        static int IDS2( string[] elements)
        {
            int occurances = 0;
            int count = 1;

            for (int index = 1; index < elements.Length; index++)
            {
                if (elements[index] == elements[index - 1] && (elements[index] != null))
                {
                    count++;
                    occurances = Math.Max(occurances, count);
                }
                else
                {
                    count = 1;

                }
            }
            return occurances;
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
                    try
                    { 
                        XDocument xDocument = XDocument.Load("Baza.xml");
                        XElement root = xDocument.Element("ArrayOfBrojilo");
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

                        Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.addentity.ToString());  // zeraja  je stavio za servis
                        return true;
                    }
                   catch(Exception e)
                  {
                        //  treba prepraviti da Dekripcija vrati List-u brojila , da se doda i onda  opet enkriptuje

                        //SymmetricAlgorithms.Program.Start();
                           
                  }
                   



                }
            }
            else
            {
                Audit.AuthorizationFailed(principal.Identity.Name, Environment.MachineName, "nema pravo pristupa");
            }
            Console.WriteLine("Zavrsio sam sa upisivanjem podataka u fajl.");
            return true;
        }
        public bool RemoveEntyty(int id_counter)
        {
            bool success_remove = false;
            List<Brojilo> counters = new List<Brojilo>();
            counters = ReadXMLCounters();
            try
            {
                var itemToRemove = counters.Single(r => r.Id == id_counter.ToString());
                success_remove = counters.Remove(itemToRemove);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            SaveCountersToXml(counters);
            return success_remove; 
        }

        public bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            
            if (principal.IsInRole(Permissions.addentity.ToString()))
            {
                List<Brojilo> couters = ReadXMLCounters();
                foreach (var item in couters)
                {
                    if (item.Id == counterOld.Id)
                    {
                        item.Id = counterNew.Id;
                        item.Potrosnja = counterNew.Potrosnja;
                        item.Ime = counterNew.Ime;
                    }
                }
                SaveCountersToXml(couters);
                Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.modify.ToString()); 
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
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (principal.IsInRole(Permissions.addentity.ToString()))
            {
                Brojilo counter = new Brojilo();
                int stariId = Int32.Parse(oldId);
                counter = ReadCounterFromXml(stariId);
                counter.Id = newId;
                changed = true;

                List<Brojilo> listCounters = new List<Brojilo>();

                listCounters = ReadXMLCounters();
                listCounters.Add(counter);

                SaveCountersToXml(listCounters);
                Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.modify.ToString());
            }
            else
            {
                Audit.AuthorizationFailed(principal.Identity.Name, Environment.MachineName, "nema pravo pristupa");
            }
            return changed;
        }
        #region ReadXMl
        public List<Brojilo> ReadXMLCounters()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ArrayOfBrojilo";
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
            xRoot.ElementName = "ArrayOfUsers";
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
            Brojilo br = null;
            List<Brojilo> deserializedList = new List<Brojilo>();

            deserializedList = ReadXMLCounters();
            try { 
            br = deserializedList.Single(r => r.Id == id.ToString());
            }
            catch(Exception e)
            {
                return br;
            }
            return br;
        }
        #endregion
        public void SaveCountersToXml(List<Brojilo> serializedList)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ArrayOfBrojila";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Brojilo>));
            using (StreamWriter streamWriter = System.IO.File.CreateText("Baza.xml"))
            {
                xmlSerializer.Serialize(streamWriter, serializedList);
            }
        }
        public void SaveEntityToXml(List<User> dezerializedList)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ArrayOfUsers";
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
           Brojilo counter = ReadCounterFromXml(id);
            do
            {
                foreach (var keyValuePair in WorkerLB.workers)
                {
                    double cena = 0; // ako je cena nula, onda znaci da taj ID ne postoji u bazi 
                   //  vrednost Free-a je false , a treba da bude na true
                    if (keyValuePair.Value.Free)
                    {
                        keyValuePair.Value.Free = false;
                        IWorkerService workerChannel  = getChannelToWorker(keyValuePair.Value);
                        try
                        {
                             cena = workerChannel.GetPrice(counter.Potrosnja);    //Sva logika za getPrice
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        keyValuePair.Value.Free = true; // oznacimo ga kao slobodnog 
                       return cena;
                    }
                }
                Thread.Sleep(1000);

            } while (true);
        }
      
        private IWorkerService getChannelToWorker(WorkerInformations worker)
        {
            NetTcpBinding myBinding = new NetTcpBinding();
            EndpointAddress myEndpoint = new EndpointAddress(worker.URL);

            ChannelFactory<IWorkerService> myChannelFactory = new ChannelFactory<IWorkerService>(myBinding, myEndpoint);

            IWorkerService workerChannel = myChannelFactory.CreateChannel();
            return workerChannel;

        }

        public double Process_Id(int id_Brojila)
        {
            double x=0;
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (principal.IsInRole(Permissions.addentity.ToString()))
            {
                 x = GetPotrosnja(id_Brojila);
                _listID[id_counter] = OperationContext.Current.SessionId;
                id_counter++;
                int z = IDS2(_listID);

                if (IDS2(_listID) == 3)
                {
                    
                    Console.WriteLine("Prijavljujem DooS napad");
                    Audit.Dos_Attack_Report(principal.Identity.Name);
                    _listID = new string[15];
                    id_counter = 0;
                    // ovde pozvati IPS , koji ce samo blokirati ovaj SessionID :D i cao zdravoo poyyy


                }
                Console.WriteLine(OperationContext.Current.SessionId);
                Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.execute.ToString());  
            }
            return x;
        }
		public EnumType Detekcija()
		{
			CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
			if (principal.IsInRole(Permissions.addentity.ToString()) && principal.IsInRole(Permissions.execute.ToString()) && principal.IsInRole(Permissions.modify.ToString()))
			{
				return EnumType.Administrator;
			}
			if (principal.IsInRole(Permissions.execute.ToString()) && principal.IsInRole(Permissions.modify.ToString()))
			{
               return EnumType.Operator;
            }
			else
			{
                return EnumType.Customer; 
            }
		}
        public Brojilo SearchId(int id)
        {
            Brojilo br = null;
            br = ReadCounterFromXml(id);
            return br;
        }
    }
}


