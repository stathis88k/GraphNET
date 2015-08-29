using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphApp.GraphService;
using GraphApp.Models;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Controls;
using GraphX.PCL.Logic.Algorithms.OverlapRemoval;


namespace GraphApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GraphServiceClient client = new GraphServiceClient();
             client.UpdateDB();
            //Lets setup GraphArea settings
            GraphGenerate();


            
            
        }


        private void GraphGenerate()
        {
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
            GraphArea.LogicCore = LogicCore;
            GraphArea.GenerateGraph();
            GraphArea.RelayoutGraph(true);
        }

        private GraphModel GraphSetup()
        {
            GraphDB db = new GraphDB();
            var graph =  new GraphModel();

            foreach (var node in db.Nodes)
            {
                var dataVertex = new DataVertex() {ID = node.Id, Text = node.label};
                graph.AddVertex(dataVertex);
            }


            //Now lets make some edges that will connect our vertices
            //get the indexed list of graph vertices we have already added
            var vlist = graph.Vertices.ToList();
            //Then create two edges optionaly defining Text property to show who are connected
            foreach (var node in vlist)
            {
                foreach (var edge in db.Edges.Where(x => x.FromNode == node.ID))
                {
                    var dataEdge = new DataEdge(node, vlist[vlist.FindIndex(x=> x.ID == edge.ToNode)]) { Text =
                        $"{node} -> {vlist[vlist.FindIndex(x => x.ID == edge.ToNode)]}"
                    };
                    graph.AddEdge(dataEdge);
                }
               
            }
           
            
            return graph;
        }
    }
}
