﻿using System;
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
        // List<string> _listId = new List<string>();
        static string[] _listID = new string[15];
        static int id_counter = 0;
        static int i_counter = 0;
       
        static int IDS2( string[] elements)
        {
            // Saves the maximal number of consecutive number with the
            // same numeric value
            int occurances = 0;

            // Counts how many numbers with the same value stands right
            // behind each other
            int count = 1;

            // Run from the second index because of the in the body following
            // if-clause
            for (int index = 1; index < elements.Length; index++)
            {

                // In case the current and the previous element
                // have the same numeric value
                if (elements[index] == elements[index - 1] && (elements[index] != null))
                {
                    // Increase the count (which is initialised with 1)
                    // And save the maximal number of consecutive numbers
                    // with the same value in occurances
                    count++;
                    occurances = Math.Max(occurances, count);
                }
                else
                {
                    // If the two numbers differ, reset the counter
                    count = 1;

                }
            }

           // Console.WriteLine(occurances);
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
            /// audit both successfull and failed authorization checks
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
            /// ovo jedino valja
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
            // sad ovde  LB izvuce ceo ENTITET iz XML-a i  posalje WORKERu !!!
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

            return -1;

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
            double x = GetPotrosnja(id_Brojila);
            _listID[id_counter]=OperationContext.Current.SessionId;
            id_counter++;
            int doos=IDS2(_listID);
            if(doos ==3)
            {
                Console.WriteLine("Prijavljujem DooS napad");
                // ovde pozvati IPS , koji ce samo blokirati ovaj SessionID :D i cao zdravoo poyyy
            }
            Console.WriteLine(OperationContext.Current.SessionId);
            return x;
        }

		public EnumType Detekcija()
		{
			CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
			/// audit both successfull and failed authorization checks
			if (principal.IsInRole(Permissions.addentity.ToString()) && principal.IsInRole(Permissions.execute.ToString()) && principal.IsInRole(Permissions.modify.ToString()))
			{
				// za admina
				return EnumType.Administrator;
			}
			if (principal.IsInRole(Permissions.execute.ToString()) && principal.IsInRole(Permissions.modify.ToString()))
			{
                // 2 je za operatora
                return EnumType.Operator;
            }
			else
			{
                return EnumType.Customer; // za klijenta
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


