using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGeneration
{
    public static class BoruvkaParallelMSTGeneration
    {
        public static Graph GenerateMST(Graph graph)
        {
            var MST = new HashSet<Edge>();
            var components = new List<HashSet<Node>>();
            var nodesAndTheirEdges=new Dictionary<Node, HashSet<Edge>>();
            foreach (var node in graph.Nodes)
            {
                var hashSet = new HashSet<Node>();
                hashSet.Add(node);
                components.Add(hashSet);
            }
            foreach (var edge in graph.Edges)
            {
                if (!nodesAndTheirEdges.ContainsKey(edge.FirstNode))
                {
                    nodesAndTheirEdges.Add(edge.FirstNode, new HashSet<Edge>());
                }
                if (!nodesAndTheirEdges.ContainsKey(edge.SecondNode))
                {
                    nodesAndTheirEdges.Add(edge.SecondNode, new HashSet<Edge>());
                }
                nodesAndTheirEdges[edge.FirstNode].Add(edge);
                nodesAndTheirEdges[edge.SecondNode].Add(edge);
            }
            var edges= new HashSet<Edge>();
            while (components.Count > 1)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    HashSet<Node>? component = components[i];
                    foreach (var node in component)
                    {
                        double minEdgeWeight = double.MaxValue;
                        Edge? lowestWeightEdge = null;

                        for (int i1 = i+1; i1 < components.Count; i1++)
                        {
                            HashSet<Node>? component2 = components[i1];
                            foreach (var node2 in component2)
                            {
                                foreach (var edge in nodesAndTheirEdges[node])
                                {
                                    if (minEdgeWeight > edge.Weighting && (
                                        (edge.FirstNode == node && edge.SecondNode == node2) || 
                                        (edge.SecondNode == node2 && edge.FirstNode == node)
                                    ))
                                    {
                                        lowestWeightEdge = edge;
                                        minEdgeWeight = edge.Weighting;
                                    }
                                }
                                
                            }
                        }
                        if (lowestWeightEdge != null)
                        {
                            component.Add(lowestWeightEdge.FirstNode);
                            component.Add(lowestWeightEdge.SecondNode);
                            MST.Add(lowestWeightEdge);
                            edges.Add(lowestWeightEdge);
                        }

                    }
                }
                
            }
        }
        public static HashSet<HashSet<Node>> GetSetOfNodeSets(HashSet<Edge> edges)
        {
            var nodesAndTheirEdges = new Dictionary<Node, HashSet<Edge>>();
            foreach (var edge in edges)
            {
                if (!nodesAndTheirEdges.ContainsKey(edge.FirstNode))
                {
                    nodesAndTheirEdges.Add(edge.FirstNode, new HashSet<Edge>());
                }
                if (!nodesAndTheirEdges.ContainsKey(edge.SecondNode))
                {
                    nodesAndTheirEdges.Add(edge.SecondNode, new HashSet<Edge>());
                }
                nodesAndTheirEdges[edge.FirstNode].Add(edge);
                nodesAndTheirEdges[edge.SecondNode].Add(edge);
            }
            var setsOfNodes = edges.Select(a => { 
                var set = new HashSet<Node>();
                set.Add(a.FirstNode);
                set.Add(a.SecondNode);
                return set; 
            }).ToHashSet();
            while (true)
            {
                setsOfNodes = setsOfNodes.Select(setOfNodes =>
                {
                    var set = new HashSet<Node>();
                    foreach (var node in setOfNodes)
                    {
                        set.Add(node);
                        foreach (var setOfNodes2 in setsOfNodes)
                        {
                            if (setOfNodes!=setOfNodes2&&setOfNodes2.Contains(node))
                            {
                                set.Add(node);
                                setOfNodes2.Remove(node);
                            }
                        }
                    };
                    return set;

                }).ToHashSet();
            }

        }
        public static HashSet<Node> GetAllConnectedNodes(Node node, HashSet<Edge> edges, Dictionary<Node, HashSet<Edge>> nodesAndTheirEdges)
        {
            var directlyConnected=nodesAndTheirEdges[node].
        }
    }
}
