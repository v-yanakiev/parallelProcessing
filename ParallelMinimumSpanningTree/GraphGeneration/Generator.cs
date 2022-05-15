namespace GraphGeneration
{
    public static class Generator
    {
        public static Graph GenerateGraph(int nodeCount)
        {
            var canvasSize = 100;
            var graph = new Graph();
            var rand = new Random();

            FillGraphWithNodes(nodeCount, canvasSize, graph,rand);
            AddEdgesToGraph(graph,rand);
            return graph;
        }

        private static void AddEdgesToGraph(Graph graph, Random rand)
        {
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                Node node = graph.Nodes[i];
                for (int i2 = i+1; i2 < graph.Nodes.Count; i2++)
                {
                    Node node2 = graph.Nodes[i2];
                    int randomValue = rand.Next(0, 100);
                    if (randomValue < 25)
                    {
                        graph.Edges.Add(new Edge(node, node2, Math.Sqrt(Math.Pow(node.XCoordinate - node2.XCoordinate, 2) + Math.Pow(node.YCoordinate - node2.YCoordinate, 2))));
                    }
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