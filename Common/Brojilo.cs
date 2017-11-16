using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Brojilo
    {
        public Brojilo()
        { }
        public Brojilo(string id, string ime , string prz, string pot)
        {
            this.Id = id;
            this.Ime = ime;
            this.Prezime = prz;
            this.Potrosnja = pot;
        }
        private string id;
        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string  ime;
        [DataMember]
        public string  Ime
        {
            get { return ime; }
            set { ime = value; }
        }
        private string prezime;
        [DataMember]
        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; }
        }
        private string potrosnja;
        [DataMember]
        public string Potrosnja
        {
            get { return potrosnja; }
            set { potrosnja = value; }
        }



    }
}
