using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GraphService
{

    [DataContract(Name = "node", Namespace = "")]
    public class Node
    {
        [DataMember(Name = "id", Order = 0)]
        public int Id { get; set; }

        [DataMember (Name = "label", Order = 1)]
        public string Label { get; set; }

        [DataMember(Name = "adjacentNodes", Order = 2)]
        public AdjacentNodes AdjacentNodes { get; set; }

    }

    [CollectionDataContract(Name = "adjacentNodes",ItemName = "id", Namespace = "")]
    public class AdjacentNodes : List<int>
    {
        [DataMember(Name = "id", Order = 0)]
        public List<int> Id { get; set; }

    }

}
