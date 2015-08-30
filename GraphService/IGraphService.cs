using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GraphService.Model;


namespace GraphService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGraphService" in both code and config file together.
    [ServiceContract]
    public interface IGraphService
    {
        [OperationContract]
        void UpdateDB();

        [OperationContract]
        List<int> ShortestPath(int start, int finish, Dictionary<int,Dictionary<int,int>> vlist);
    }
}
