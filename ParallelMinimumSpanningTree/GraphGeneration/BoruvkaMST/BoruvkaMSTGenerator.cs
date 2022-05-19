using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGeneration.BoruvkaMST
{
    public class BoruvkaMSTGenerator
    {
        public BoruvkaMSTGenerator(List<Node> nodes,List<Edge> edges)
        {
            this.Nodes = nodes;
            this.edges = edges;
        }

        public List<Node> Nodes { get; private set; } //nodes
        //public List<int> nodeIds { get; set; }
        public List<Edge> edges { get; private set; }
        public Tuple<List<Edge>,double> Generate()
        {
            var parent = new List<int>();
            var rank = new List<int>();
            var numberOfTrees = this.Nodes.Count;
            var cheapest = new List<Edge?>();
            var MST = new List<Edge>();
            var MSTWeight = 0.0;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                this.Nodes[i].Id = i;
            }
            foreach (var node in this.Nodes)
            {
                parent.Add(node.Id);
                rank.Add(0);
                cheapest.Add(null);
            }
            while (numberOfTrees > 1)
            {
                Parallel.ForEach(this.edges, edge => {
                    var u = edge.FirstNode.Id;
                    var v = edge.SecondNode.Id;
                    var w = edge.Weighting;
                    var setNumberOfU = this.find(parent, u);
                    var setNumberOfV = this.find(parent, v);
                    if (setNumberOfU != setNumberOfV)
                    {
                        if (cheapest[setNumberOfU] == null || cheapest[setNumberOfU].Weighting > w)
                        {
                            cheapest[setNumberOfU] = edge;
                        }
                        if (cheapest[setNumberOfV] == null || cheapest[setNumberOfV].Weighting > w)
                        {
                            cheapest[setNumberOfV] = edge;
                        }
                    }
                });
                Parallel.ForEach(this.Nodes, node =>
                {
                    if (cheapest[node.Id] != null)
                    {
                        var u = cheapest[node.Id].FirstNode.Id;
                        var v = cheapest[node.Id].SecondNode.Id;
                        var w = cheapest[node.Id].Weighting;
                        var setNumberOfU = this.find(parent, u);
                        var setNumberOfV = this.find(parent, v);
                        if (setNumberOfU != setNumberOfV)
                        {
                            MST.Add(cheapest[node.Id]);
                            this.Union(parent, rank, setNumberOfU, setNumberOfV);
                            MSTWeight += w;
                            numberOfTrees--;
                        }
                    }
                });
                for (int i = 0; i < cheapest.Count; i++)
                {
                    cheapest[i] = null;
                }
            }
            return new Tuple<List<Edge>, double>(MST, MSTWeight);
        }
        int find(List<int> parent, int i)
        {
            if (parent[i] == i)
                return i;

            return find(parent, parent[i]);
        }
        void Union(List<int> parent,List<int> rank, int x, int y)
        {
            var xroot = this.find(parent, x);
            var yroot = this.find(parent, y);
            if (rank[xroot] < rank[yroot])
            {
                parent[xroot] = yroot;
            }
            else if(rank[xroot] > rank[yroot])
            {
                parent[yroot] = xroot;
            }
            else
            {
                parent[yroot] = xroot;
                rank[xroot] += 1;
            }
        }
    }
}
