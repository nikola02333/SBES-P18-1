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
using System.ServiceModel.Channels;

namespace SBES_P18_Server
{
    public class LoadBalancerService : ILoadBalancerService
    {
        static string[] _listID = new string[15];
        static string[] Block_listID = new string[15];
        static int id_counter = 0;
        static int block_id_counter = 0;
        static int IDS( string[] elements)
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
        public static bool IPS(string ip)
        {
            if (Block_listID.Contains(ip))
            {
                return true;
            }
            else
                return false;

        }
        #region Entyty
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

                        Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.addentity.ToString()); 
                        return true;
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
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(Permissions.addentity.ToString()))
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
              
                Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.addentity.ToString());  
                return success_remove;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (principal.IsInRole(Permissions.addentity.ToString()) || principal.IsInRole(Permissions.modify.ToString()))
            {

                List<Brojilo> couters = ReadXMLCounters();
                foreach (var item in couters)
                {
                    if (item.Id == counterOld.Id)
                    {
                        item.Id = counterNew.Id;
                        item.Potrosnja = counterNew.Potrosnja;
                        item.Ime = counterNew.Ime;
                        item.Prezime = counterNew.Prezime;
                    }
                }
                SaveCountersToXml(couters);
                if (principal.IsInRole(Permissions.modify.ToString()))
                {
                    Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.modify.ToString());
                }
                else
                {
                    Audit.AuthorizationSuccess(principal.Identity.Name, Permissions.modify.ToString());
                }
                
            }
            return true;
        }
        #endregion 


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
        /// <summary>
        /// //////////////////////////////
        /// </summary>
        /// <returns></returns>
        public double GetPotrosnja(int id)
        {
            double cena = 0;
           
            Brojilo counter = ReadCounterFromXml(id);
            do
            {
                foreach (var keyValuePair in WorkerLB.workers)
                {
                    if (keyValuePair.Value.Free)
                    {
                        keyValuePair.Value.Free = false;
                        IWorkerService workerChannel  = getChannelToWorker(keyValuePair.Value);
                        try
                        {
                        cena = workerChannel.GetPrice(counter.Potrosnja);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                      //  Thread.Sleep(5000);
                        keyValuePair.Value.Free = true;
                        return cena;
                    }
                }
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
            if (principal.IsInRole(Permissions.execute.ToString()))
            {
                x = GetPotrosnja(id_Brojila);
                ///
                OperationContext context = OperationContext.Current;
                MessageProperties prop = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                string ip = endpoint.Address;
                ///
                // _listID[id_counter] = OperationContext.Current.SessionId;
                _listID[id_counter] = ip;
                id_counter++;
                if (IPS(ip)) // sad pogledaj da li ta ip adresa je na crnoj listi , ako jeste prekini  izvrsavanje!!!
                {
                    return 0; // prekini operaciju !!!
                }
                int z = IDS(_listID);
                if (IDS(_listID) == 3)
                {
                    Console.WriteLine("Prijavljujem DooS napad");
                    Audit.Dos_Attack_Report(principal.Identity.Name);
                    _listID = new string[15];
                    Block_listID[block_id_counter] = ip;
                    block_id_counter++;
                    id_counter = 0;
                }
                
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


