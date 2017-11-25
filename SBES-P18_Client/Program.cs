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
            //proxy.AddEntyty(br);

            // double x = proxy.Process_Id(666);
            double x;
          //  Console.WriteLine("Vas racun za struju  iznosi : {0}",x);

           //  x = proxy.Process_Id(666);
           // Console.WriteLine("Vas racun za struju  iznosi : {0}", x);
            int t = 0;

                x = proxy.Process_Id(666);
                t++;
                Console.WriteLine(t);
            
            Console.ReadKey();
        }
    }
}
