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
            double x;
            int t = 0;
          //  while (t < 3)
            {
                x = proxy.Process_Id(666);
                t++;
                Console.WriteLine("{0} poziv = {1}",t,x);
            }
            Console.ReadKey();
        }
    }
}
