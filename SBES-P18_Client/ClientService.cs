using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SBES_P18_Client
{
    public class ClientService : ChannelFactory<ILoadBalancerService>, ILoadBalancerService, IDisposable
    {
        public bool AddEntyty(Brojilo counter)
        {
            throw new NotImplementedException();
        }

        public bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld)
        {
            throw new NotImplementedException();
        }

        public bool Login(User u)
        {
            throw new NotImplementedException();
        }

        public List<Brojilo> ReadXML()
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntyty(Brojilo counter)
        {
            throw new NotImplementedException();
        }

        public bool Work()
        {
            throw new NotImplementedException();
        }
    }
}
