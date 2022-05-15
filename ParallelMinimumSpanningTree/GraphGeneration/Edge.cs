namespace GraphGeneration
{
    public class Edge
    {
        public Edge(Node firstNode, Node secondNode, double weighting)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weighting = weighting;
        }

        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }
        public double Weighting { get; set; }
    }
}