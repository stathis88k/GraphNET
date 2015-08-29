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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GraphService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GraphService.svc or GraphService.svc.cs at the Solution Explorer and start debugging.
    public class GraphService : IGraphService
    {
        private const string inputfolder = "Input";
        public void UpdateDB()
        {
            GraphDB db = new GraphDB();
            var pathtoproject = AppDomain.CurrentDomain.BaseDirectory;

            foreach (string file in Directory.EnumerateFiles(Path.Combine(pathtoproject,inputfolder), "*.xml"))
            {
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
        
        
        public void ShortestPath(string a, string b)
        {
            throw new NotImplementedException();
        }
    }
}
