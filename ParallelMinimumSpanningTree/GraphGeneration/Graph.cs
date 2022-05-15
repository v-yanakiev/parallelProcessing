﻿using System;
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
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }

        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }
    }
}