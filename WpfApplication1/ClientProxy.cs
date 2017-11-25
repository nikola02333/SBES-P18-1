using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class ClientProxy : ChannelFactory<ILoadBalancerService>, ILoadBalancerService, IDisposable
    {
        ILoadBalancerService factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
            factory = this.CreateChannel();
        }


        public bool AddEntyty(Brojilo counter)
        {
            bool result = false;
            try
            {
                result= factory.AddEntyty(counter);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld)
        {
            bool result = false;
            try
            {
                result= factory.ChangeEntyty(counterNew, counterOld);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public bool ChangeIdBrojila(string newId, string oldId)
        {
            throw new NotImplementedException();
        }

        public bool ChangeValueBrojila(string id, string potrosnja)
        {
            throw new NotImplementedException();
        }

        public EnumType Detekcija()
        {
          return  factory.Detekcija();
        }

        public double Process_Id(int id_Brojila)
        {
            double result=0;

            try
            {
                result= factory.Process_Id(id_Brojila);
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public bool RemoveEntyty(int id_counter)
        {
            bool is_deleted = false; 
            try
            {
                is_deleted= factory.RemoveEntyty(id_counter);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return is_deleted;
           
        }

        public Brojilo SearchId(int id)
        {
            Brojilo br = null;
            try
            {
                br = factory.SearchId(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return br;
        }
    }
}
