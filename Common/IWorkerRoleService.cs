﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
    public   interface IWorkerRoleService
    {
        [OperationContract]
        int Work();

    }
}
