using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

using GraphService.Model;

namespace GraphService
{
    // Implementation of the IGraphService interface
    public class GraphService : IGraphService
    {
        private const string inputfolder = "Input";
        public void UpdateDB()
        {
            GraphDB db = new GraphDB();
            //get the path to the folder with all the xml files
            var pathtoproject = AppDomain.CurrentDomain.BaseDirectory;

            foreach (string file in Directory.EnumerateFiles(Path.Combine(pathtoproject,inputfolder), "*.xml"))
            {
                //use the datacontractserialize to deserialize the xml content 
                // check the database and update changed values or insert values if they dont exist
                DataContractSerializer deserializer =  new DataContractSerializer(typeof(Node));
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    using (
                        var reader = XmlDictionaryReader.CreateTextReader(fs,
                            new XmlDictionaryReaderQuotas()))
                    {
                        var node = (Node)deserializer.ReadObject(reader, true);
                        bool changed;
                        if (changed = !db.Nodes.Any(x => x.Id == node.Id && x.label.Equals(node.Label)))
                        {
                            db.Nodes.RemoveRange(db.Nodes.Where(x => x.Id == node.Id || x.label.Equals(node.Label)));
                            db.Nodes.Add(new Nodes() {Id = node.Id, label = node.Label});
                        }

                        foreach (var id in node.AdjacentNodes)
                        {
                            if (!changed) db.Edges.RemoveRange(db.Edges.Where(x => x.FromNode == node.Id));
                            if(!db.Edges.Any(x => x.FromNode == node.Id && x.ToNode == id)) db.Edges.Add(new Edges() {FromNode = node.Id, ToNode = id, Weight = 1});
                        }
                        db.SaveChanges();
                    }
    
                }
            }
        }

        //calculate the shortest path 
        //The Dijkstras algorithm ís used with some modifications to correspond our situation
        public List<int> ShortestPath(int start, int finish, Dictionary<int, Dictionary<int, int>> vlist)
        {
            var previous = new Dictionary<int, int>();
            var distances = new Dictionary<int, int>();
            var nodes = new List<int>();

            List<int> path = null;

            foreach (var node in vlist)
            {
               distances[node.Key] = node.Key == start? 0 : int.MaxValue;
               nodes.Add(node.Key);
            }

           
            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<int>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue) break;

                foreach (var neighbor in vlist[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

    

    }
}
