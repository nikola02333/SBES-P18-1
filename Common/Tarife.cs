using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    [DataContract]
    public enum Tarifa { ZELENA = 0, PLAVA = 1, CRVENA = 2 };
    public class Tarife
    {
        private int cena;
        private Tarifa tar;
        private int donja_granica;
        private int gornja_granica;

        public Tarife()
        { }

        public Tarife(Tarifa t, int c)
        {
            tar = t;
            cena = c;

        }
        [DataMember]
        public Tarifa tarifa
        {
            get { return tar; }
            set { tar = value; }
        }
        [DataMember]
        public int Cena
        {
            get { return cena; }
            set { cena = value; }
        }

        [DataMember]
        public int DG
        {
            get { return donja_granica; }
            set { donja_granica = value; }
        }

        [DataMember]
        public int GG
        {
            get { return gornja_granica; }
            set { gornja_granica = value; }
        }



    }
}
