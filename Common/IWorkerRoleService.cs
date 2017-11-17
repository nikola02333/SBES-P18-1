using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
    public interface IWorkerRoleService
    {
        [OperationContract]
        double Price(int p, List<Tarife> lt);
        [OperationContract]
        int Work(int id);
        [OperationContract]
        void StartId(int id);
        

    } 
}

