namespace GraphGeneration
{
    public static class GraphGeneration
    {
        public static Graph GenerateGraph(int nodeCount)
        {
            var canvasSize = 600;
            var graph = new Graph();
            var rand = new Random();

            FillGraphWithNodes(nodeCount, canvasSize, graph,rand);
            AddEdgesToGraph(graph,rand);
            return graph;
        }

        private static void AddEdgesToGraph(Graph graph, Random rand)
        {
            var nodeArray = graph.Nodes.ToArray();
            for (int i = 0; i < nodeArray.Length; i++)
            {
                Node node = nodeArray[i];
                for (int i2 = i+1; i2 < graph.Nodes.Count; i2++)
                {
                    Node node2 = nodeArray[i2];
                    //if (graph.Nodes.Count<20 || rand.Next(0, 100) < 25)
                    //{
                        var edge = new Edge(node, node2, Math.Sqrt(Math.Pow(node.XCoordinate - node2.XCoordinate, 2) + Math.Pow(node.YCoordinate - node2.YCoordinate, 2)));
                        graph.Edges.Add(edge);
                        //node.Edges.Add(edge);
                        //node2.Edges.Add(edge);
                    //}
                }
            }
        }

        private static void FillGraphWithNodes(int nodeCount, int canvasSize, Graph graph, Random rand)
        {
            for (int i = 0; i < nodeCount; i++)
            {
                var x = rand.Next(0, canvasSize);
                var y = rand.Next(0, canvasSize);
                if (!graph.Nodes.Any(a => a.XCoordinate == x && a.YCoordinate == y))
                {
                    graph.Nodes.Add(new Node() { XCoordinate = x, YCoordinate = y });
                }
                else
                {
                    i--;
                }
            }
        }
    }
}