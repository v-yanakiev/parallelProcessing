using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGeneration
{
    public class Graph
    {
        public Graph()
        {
            Nodes = new HashSet<Node>();
            Edges = new HashSet<Edge>();
        }

        public HashSet<Node> Nodes { get; set; }
        public HashSet<Edge> Edges { get; set; }
    }
}
