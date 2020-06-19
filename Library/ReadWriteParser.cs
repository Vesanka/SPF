using System;
using System.Collections.Generic;
using System.IO;

namespace Library
{
    public static class ReadWriteParser
    {

        public static List<Tuple<int, int>> ReadToEdges(string filename)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            int index = 0;
            using (StreamReader stream = new StreamReader(filename))
            {
                string line;
                string[] words;
                int a, b;

                while ((line = stream.ReadLine()) != null)
                {
                    words = line.Split();
                    if (index==0)
                    {
                        try
                        {
                            words[0].ToString();
                        }
                        catch (Exception)
                        {

                            throw new FormatException("Bad file format!"); ;
                        }
                        if(!int.TryParse(words[0], out a) || a <= 0)
                            throw new FormatException("Bad file format!");
                        
                        result = new List<Tuple<int, int>>(a);
                        index++;
                        continue;                        
                    }
                    try
                    {
                        words[0].ToString();
                        words[1].ToString();
                    }
                    catch (Exception)
                    {

                        throw new FormatException("Bad file format!"); ;
                    }
                    if (!int.TryParse(words[0], out a)||a <= 0)
                        throw new FormatException("Bad file format!");
                    if (!int.TryParse(words[1], out b) || b <= 0)
                        throw new FormatException("Bad file format!");

                    result.Add(new Tuple<int, int>(a, b));
                }
                return result;
            }
        }

        public static void Write(Layers gr, string fileName)
        {
            if (gr == null)
            {
                throw new NullReferenceException("No graph description to save!");
            }

            string linetoWrite = $"Origin .edg file: {fileName}\r\n\r\n\r\n" +
                $"Total nodes in graph: {gr.GetNodesCount()}\r\n\r\n" +
                $"Max width of graph: {gr.GetMaxWidth()}\r\n\r\n" +
                $"Average width of graph/Acceleration: {gr.GetAverageWidth()}\r\n\r\n" +
                $"Depth of graph: {gr.GetGraphDepth()}\r\n\r\n" +
                $"Dispertion: {gr.GetSigma()}\r\n\r\n" +
                $"Irregularity factor: {gr.GetKoeff()}\r\n\r\n\r\n";

            for (int i = 0; i < gr.GetGraphDepth(); i++)
            {
                linetoWrite += $"Layer №{i+1}:\r\n" +
                    $"Number of nodes: {gr.GetLayerWidth(i)}\r\n" +
                    $"Nodes in layer: ";
                for (int j = 0; j < gr.GetLayer(i).Count; j++)
                {
                    linetoWrite += $"{gr.GetLayer(i)[j].Id}; ";
                }
                linetoWrite += "\r\n\r\n";
            }

            using (StreamWriter stream = new StreamWriter(fileName+".descr"))
            {
                stream.Write(linetoWrite);
            }
        }
    }
}
