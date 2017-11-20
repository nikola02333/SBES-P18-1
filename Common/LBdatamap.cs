using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    [DataContract]
    public class LBdatamap
    {
        string WorkerId;
        bool avalivable;
        int Id;
        int potrosnja;
        double racun;

        public LBdatamap() { }

        public LBdatamap(string WID)
        {
            WorkerId = WID;
            avalivable = true;
            Id = 0;
            potrosnja = 0;
            racun = 0;
        }

        [DataMember]
        public bool Avalivable
        {
            get { return avalivable; }
            set { avalivable = value; }

        }
        [DataMember]
        public string WorkerID
        {
            get { return WorkerId; }
            set { WorkerId = value; }

        }
        [DataMember]
        public int ID
        {
            get { return Id; }
            set { Id = value; }
        }
        [DataMember]
        public int Potrosnja
        {
            get { return potrosnja; }
            set { potrosnja = value; }
        }
        [DataMember]
        public double Racun
        {
            get { return racun; }
            set { racun = value; }
        }

    }


}
