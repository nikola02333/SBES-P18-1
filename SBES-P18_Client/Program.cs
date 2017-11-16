using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SBES_P18_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ILoadBalancerService> factory = new ChannelFactory<ILoadBalancerService>(typeof(ILoadBalancerService).ToString());

            ILoadBalancerService proxy = factory.CreateChannel();
            Brojilo br = new Brojilo("123", "Niki", "Velickovic", "666");
           proxy.AddEntyty(br);

         
            User u = new User("nikola023", "xxx", EnumType.Administrator);

            if (proxy.Login(u))
            {
                Console.WriteLine("Korisnik {0} se uspesno ulogovao", u.Username);
            }
            else
            {

                Console.WriteLine("Korisnik {0}  nije uspeo da se uloguje", u.Username);

            }

          bool x=  proxy.RemoveEntyty(br);

            Console.ReadKey();
        }
    }
}
