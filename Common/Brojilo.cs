using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
<<<<<<< HEAD
using System.Xml.Serialization;
=======
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139

namespace Common
{
    [DataContract]
    public class Brojilo
    {
        public Brojilo()
        { }
<<<<<<< HEAD
        public Brojilo(string id, string ime, string prz, string pot)
=======
        public Brojilo(string id, string ime , string prz, string pot)
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
        {
            this.Id = id;
            this.Ime = ime;
            this.Prezime = prz;
            this.Potrosnja = pot;
        }
        private string id;
        [DataMember]
<<<<<<< HEAD
        [XmlElement("Id")]
=======
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
<<<<<<< HEAD
        private string ime;
        [DataMember]
        [XmlElement("Ime")]
        public string Ime
=======
        private string  ime;
        [DataMember]
        public string  Ime
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
        {
            get { return ime; }
            set { ime = value; }
        }
        private string prezime;
        [DataMember]
<<<<<<< HEAD
        [XmlElement("Prezime")]
=======
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; }
        }
        private string potrosnja;
        [DataMember]
<<<<<<< HEAD
        [XmlElement("Potrosnja")]
=======
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
        public string Potrosnja
        {
            get { return potrosnja; }
            set { potrosnja = value; }
        }



    }
}
