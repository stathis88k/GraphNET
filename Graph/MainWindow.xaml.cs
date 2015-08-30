using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GraphApp.GraphService;
using GraphApp.Models;
using GraphService.Model;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.PCL.Logic.Algorithms.OverlapRemoval;


namespace GraphApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, string> nodeList = new Dictionary<int, string>();
        Dictionary<int, Dictionary<int, int>> vertices = new Dictionary<int, Dictionary<int, int>>();
        public MainWindow()
        {
            InitializeComponent();

            // create the object to call the service
            GraphServiceClient client = new GraphServiceClient();

            //update the db with the latest info
             client.UpdateDB();

            //populate the comboboxes
            cbFromNode.ItemsSource = nodeList;
            cbFromNode.SelectedValuePath = "Key";
            cbFromNode.DisplayMemberPath  = "Value";
            cbFromNode.SelectedValue = 1;
            cbToNode.ItemsSource = nodeList;
            cbToNode.SelectedValuePath = "Key";
            cbToNode.DisplayMemberPath = "Value";
            cbToNode.SelectedValue = 2;

            //generate the graph
            GraphGenerate();

        }


        private void GraphGenerate()
        {
            //the graphx library is used to generate the graph
            var LogicCore = new GXCore() { Graph = GraphSetup()};
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            LogicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set optional parameters using AlgorithmFactory
            //NOTE: default parameters can be automatically created each time you change Default algorithms
            LogicCore.DefaultLayoutAlgorithmParams =
                               LogicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)LogicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            LogicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Setup optional params
            LogicCore.DefaultOverlapRemovalAlgorithmParams =
                              LogicCore.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            ((OverlapRemovalParameters)LogicCore.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 50;
            ((OverlapRemovalParameters)LogicCore.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 50;

            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            LogicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            LogicCore.AsyncAlgorithmCompute = false;

            //Finally assign logic core to GraphArea object
            
            LogicCore.EnableParallelEdges = true;
            LogicCore.ParallelEdgeDistance = 25;
            LogicCore.EdgeCurvingEnabled = true;

            GraphArea.SetVerticesDrag(true, true);
            GraphArea.LogicCore = LogicCore;
            GraphArea.GenerateGraph();
            GraphArea.RelayoutGraph(true);
        }

        private GraphModel GraphSetup()
        {
            GraphDB db = new GraphDB();
            var graph = new GraphModel();

            //get all the nodes 
            foreach (var node in db.Nodes)
            {
                var dataVertex = new DataVertex() { ID = node.Id, Text = node.label };
                graph.AddVertex(dataVertex);
                nodeList.Add(node.Id,node.label);
            }

            var vlist = graph.Vertices.ToList();

            //add all the edges
            foreach (var node in vlist)
            {
                Dictionary<int,int> edges =  new Dictionary<int, int>();
                foreach (var edge in db.Edges.Where(x => x.FromNode == node.ID))
                {
                    var dataEdge = new DataEdge(node, vlist[vlist.FindIndex(x => x.ID == edge.ToNode)]);
                    graph.AddEdge(dataEdge);
                    edges.Add(edge.ToNode,1);
                }

                vertices.Add(node.ID,edges);

            }

            return graph;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //call the service and calculate the shortest path and display it
            GraphServiceClient client = new GraphServiceClient();
            var start = Convert.ToInt32(cbFromNode.SelectedValue);
            var finish = Convert.ToInt32(cbToNode.SelectedValue);
            List<int> list =  client.ShortestPath(start, finish , vertices);

            List<string> path =  new List<string>();

            foreach (var i in list)
            {
                path.Add(nodeList[i]);
            }

            txtPath.Text = string.Join(" -> ", path);
        }
    }
}
