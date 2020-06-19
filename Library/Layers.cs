using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Layers
    {

        List<List<Node>> nodes;

        public Node this[int layer, int node]
        {
            get
            {
                return nodes[layer][node];
            }
            set
            {
                nodes[layer][node] = value;
            }
        }

        public Layers(List<Node> rawNodes)
        {
            nodes = new List<List<Node>>();
            nodes.Add(new List<Node>());

            foreach (Node node in rawNodes)
            {
                if (node.parentIds.Count==0)
                {
                    nodes[0].Add(new Node(node.Id, node.parentIds, node.childIds));
                    node.Id = -1;
                }
            }

            int layerIndex = 0;

            while (rawNodes.Exists(match: (x) => x.Id != -1))
            {
                foreach (Node nd in rawNodes)
                {
                    if (nd.Id == -1)
                        continue;

                    if (IfInNextLayer(layerIndex, nd))
                    {

                        if (nodes.Count<=layerIndex+1)
                        {
                            nodes.Add(new List<Node>());
                        }

                        nodes[layerIndex + 1].Add(new Node(nd.Id, nd.parentIds, nd.childIds));
                        nd.Id = -1;

                    }
                }
                layerIndex++;
            }
        }

        bool IfInNextLayer(int layer, Node nd)
        {
            bool flag = true;
            List<Node> previous = new List<Node>();
            for (int i = 0; i <= layer; i++)
            {
                try
                {
                    previous.AddRange(nodes[i]);
                }
                catch (System.Exception)
                {

                    throw new System.Exception("Something wrong with graph!");
                }
                
            }
            
            for (int i = 0; i < nd.parentIds.Count; i++)
            {

                flag &= previous.Exists(match: (x) => x.Id == nd.parentIds[i]);               

            }
            return flag;
        }


        public List<Node> GetLayer(int layer) => nodes[layer];

        public int GetLayerWidth(int layer) => nodes[layer].Count;

        public int GetGraphDepth() => nodes.Count;

        public int GetMaxWidth() => nodes.Max((l) => l.Count);

        public double GetKoeff() => (double)nodes.Max((l) => l.Count) / nodes.Min((l) => l.Count);

        public double GetAverageWidth() => nodes.Average((l) => l.Count);

        public int GetNodesCount() => nodes.Sum((x) => x.Count);

        public double GetSigma()
        {
            double aver = GetAverageWidth();
            double sum = 0;
            for (int i = 0; i < nodes.Count; i++)
                sum += Math.Pow(GetLayerWidth(i) - aver, 2);
            return Math.Sqrt(sum / nodes.Count);
        }

    }
}
