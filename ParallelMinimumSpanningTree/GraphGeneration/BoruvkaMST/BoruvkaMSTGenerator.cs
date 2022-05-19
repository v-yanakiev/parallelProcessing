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
            }
            while (numberOfTrees > 1)
            {
                var threadCount = 8;
                var cheapestEdgeSelectionGeneration = (int partitionNumber) =>()=>
                {
                    var cheapest = new List<Edge?>();
                    foreach (var node in this.Nodes)
                    {
                        cheapest.Add(null);
                    }
                    int?[] setOfSetNumbers=new int?[Nodes.Count];
                    for (int i = (edges.Count / threadCount) * (partitionNumber-1); i < (edges.Count/ threadCount)*partitionNumber; i++)
                    {
                        Edge? edge = this.edges[i];
                        var u = edge.FirstNode.Id;
                        var v = edge.SecondNode.Id;
                        var w = edge.Weight;
                        if (setOfSetNumbers[u] == null)
                        {
                            setOfSetNumbers[u] = this.find(parent, u);
                        }
                        if (setOfSetNumbers[v] == null)
                        {
                            setOfSetNumbers[v] = this.find(parent, v);
                        }
                        var setNumberOfU = setOfSetNumbers[u].Value;
                        var setNumberOfV = setOfSetNumbers[v].Value;
                        if (setNumberOfU != setNumberOfV)
                        {
                            if (cheapest[setNumberOfU] == null || cheapest[setNumberOfU].Weight > w)
                            {
                                cheapest[setNumberOfU] = edge;
                            }
                            if (cheapest[setNumberOfV] == null || cheapest[setNumberOfV].Weight > w)
                            {
                                cheapest[setNumberOfV] = edge;
                            }
                        }
                    }
                    return cheapest;
                };
                var arrayOfTasks=Enumerable.Range(1, threadCount).Select(i=>Task.Run(cheapestEdgeSelectionGeneration(i))).ToArray();
                Task.WaitAll(arrayOfTasks);
                var cheapestAcrossAllPartitionComputations = new List<Edge?>();

             
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    cheapestAcrossAllPartitionComputations.Add(arrayOfTasks.Select(a => a.Result).MinBy(a => a[i] == null ? int.MaxValue : a[i].Weight)?[i]);
                }
                foreach (var node in this.Nodes)
                {
                    if (cheapestAcrossAllPartitionComputations[node.Id] != null)
                    {
                        var u = cheapestAcrossAllPartitionComputations[node.Id].FirstNode.Id;
                        var v = cheapestAcrossAllPartitionComputations[node.Id].SecondNode.Id;
                        var w = cheapestAcrossAllPartitionComputations[node.Id].Weight;
                        var setNumberOfU = this.find(parent, u);
                        var setNumberOfV = this.find(parent, v);
                        if (setNumberOfU != setNumberOfV)
                        {
                            MST.Add(cheapestAcrossAllPartitionComputations[node.Id]);
                            this.Union(parent, rank, setNumberOfU, setNumberOfV);
                            MSTWeight += w;
                            numberOfTrees--;
                        }
                    }
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
