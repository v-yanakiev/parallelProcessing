using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGeneration.BoruvkaMST
{
    public class Graph
    {
        public Graph(List<Node> nodes,List<Edge> edges)
        {
            this.Nodes = nodes;
            this.edges = edges;
        }

        public List<Node> Nodes { get; private set; } //nodes
        //public List<int> nodeIds { get; set; }
        public List<Edge> edges { get; private set; }
        public Tuple<List<Edge>,double> BoruvkaMST()
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
                rank.Append(0);
                cheapest.Add(null);
            }
            while (numberOfTrees > 1)
            {
                foreach (var i in Enumerable.Range(0,this.edges.Count))
                {
                    var u = this.edges[i].FirstNode.Id;
                    var v = this.edges[i].SecondNode.Id;
                    var w = this.edges[i].Weighting;
                    var setNumberOfU = this.find(parent, u);
                    var setNumberOfV = this.find(parent, v);
                    if (setNumberOfU != setNumberOfV)
                    {
                        if (cheapest[setNumberOfU] == null || cheapest[setNumberOfU].Weighting > w)
                        {
                            cheapest[setNumberOfU] = this.edges[i];
                        }
                        if (cheapest[setNumberOfV] == null || cheapest[setNumberOfV].Weighting > w)
                        {
                            cheapest[setNumberOfV] = this.edges[i];
                        }
                    }
                    
                }
                foreach (var node in Enumerable.Range(0,this.Nodes.Count))
                {
                    if (cheapest[node] != null)
                    {
                        var u = cheapest[node].FirstNode.Id;
                        var v = cheapest[node].SecondNode.Id;
                        var w = cheapest[node].Weighting;
                        var setNumberOfU = this.find(parent, u);
                        var setNumberOfV = this.find(parent, v);
                        if (setNumberOfU != setNumberOfV)
                        {
                            MST.Add(cheapest[node]);
                            this.Union(parent, rank, setNumberOfU, setNumberOfV);
                            MSTWeight += w;
                        }
                    }
                }
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
