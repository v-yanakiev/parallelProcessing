namespace GraphGeneration
{
    public class Edge
    {
        public Edge(Node firstNode, Node secondNode, double weighting)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weighting;
        }

        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }
        public double Weight { get; set; }
    }
}