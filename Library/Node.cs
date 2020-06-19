using System;
using System.Collections.Generic;

namespace Library
{
    public class Node
    {
        int id;

        public int Id { get => id; set => id = value; }

        public List<int> parentIds = new List<int>();
        public List<int> childIds = new List<int>();

        public Node(int id, List<int> parentIds, List<int> childIds)
        {
            this.id = id;
            foreach (int pid in parentIds)
            {
                this.parentIds.Add(pid);
            }
            foreach (int cid in childIds)
            {
                this.childIds.Add(cid);
            }
        }

        public Node(int id)
        {
            this.id = id;
        }

        public static List<Node> TupleToNodes(List<Tuple<int, int>> edges)
        {
            List<Node> result = new List<Node>();

            foreach (var tp in edges)
            {
                if (!result.Exists((x) => x.Id == tp.Item1))
                {
                    result.Add(new Node(tp.Item1));
                }

                if (!result.Exists((x) => x.Id == tp.Item2))
                {
                    result.Add(new Node(tp.Item2));
                }
            }

            foreach (var tp in edges)
            {
                result.Find((x) => x.Id == tp.Item1).childIds.Add(tp.Item2);

                result.Find((x) => x.Id == tp.Item2).parentIds.Add(tp.Item1);
            }
            
            return result;
        }

    }
}
