﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GraphService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGraphService" in both code and config file together.
    [ServiceContract]
    public interface IGraphService
    {
        [OperationContract]
        void UpdateDB();

        [OperationContract]
        void ShortestPath(string a, string b);
    }
}
