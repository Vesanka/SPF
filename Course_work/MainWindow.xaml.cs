using Library;
using Microsoft.Win32;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Course_work
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IBidirectionalGraph<object, IEdge<object>> _graphToVisualize;

        string fileName;
        Layers gr;
        List<Node> rawNodes;


        public IBidirectionalGraph<object, IEdge<object>> GraphToVisualize
        {
            get { return _graphToVisualize; }
        }

        public MainWindow()
        {  

            CreateGraphToVisualize();

            InitializeComponent();
            

        }
        

        private void CreateGraphToVisualize()
        {
            var g = new BidirectionalGraph<object, IEdge<object>>();
            int many = 5;
            string[] verts = new string[many];
            for (int i = 0; i < many; i++)
            {
                verts[i] = i.ToString();
                g.AddVertex(verts[i]);
            }
               
                
            g.AddEdge(new Edge<object>(verts[0], verts[4]));
            g.AddEdge(new Edge<object>(verts[0], verts[2]));
            g.AddEdge(new Edge<object>(verts[2], verts[4]));
            g.AddEdge(new Edge<object>(verts[1], verts[3]));
            g.AddEdge(new Edge<object>(verts[3], verts[4]));
            g.AddEdge(new Edge<object>(verts[2], verts[3]));
            

            _graphToVisualize = g;

        }

        private void onFileBtnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = false;
            file.Filter = "Edg Files (.edg)|*.edg";
            file.FilterIndex = 1;
            
            if (file.ShowDialog() == true)
            {
                fileName = file.FileName;
                fileTextLabel.Content = System.IO.Path.GetFileName(fileName);
                try
                {
                    List<Tuple<int, int>> edges = ReadWriteParser.ReadToEdges(fileName);

                    rawNodes = Node.TupleToNodes(edges);

                    gr = new Layers(rawNodes);

                    fillRichTextBox();

                    MakeGraphFromTuples(edges);
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);                    
                }
            }
            
        }

        private void fillRichTextBox()
        {
            string textToTextBox = $"Total nodes: {gr.GetNodesCount()}\r\n\r\nTotal layers: {gr.GetGraphDepth()}\r\n\r\nAverage width: {gr.GetAverageWidth():f3}\r\n\r\nMax width: {gr.GetMaxWidth()}\r\n\r\nIrregularity factor: {gr.GetKoeff():f3}\r\n\r\nDispertion: {gr.GetSigma():f3}";

            dataLabel.Content = textToTextBox;
        }

        private void MakeGraphFromTuples(List<Tuple<int, int>> edges)
        {
            var g = new BidirectionalGraph<object, IEdge<object>>();
            List<int> nodes = new List<int>();

            foreach (var tuple in edges)
            {
                if (!nodes.Contains(tuple.Item1))
                {
                    nodes.Add(tuple.Item1);
                    g.AddVertex(tuple.Item1);
                }
                if (!nodes.Contains(tuple.Item2))
                {
                    nodes.Add(tuple.Item2);
                    g.AddVertex(tuple.Item2);
                }
                g.AddEdge(new Edge<object>(tuple.Item1, tuple.Item2));
            }

            graphLayuot.Graph = g;
            zoomControl.Mode = WPFExtensions.Controls.ZoomControlModes.Fill;

        }

        private void onSaveBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadWriteParser.Write(gr, fileName);
                MessageBox.Show($"Description saved to {System.IO.Path.GetFileName(fileName)}.descr");
            }
            catch (NullReferenceException ex)
            {

                MessageBox.Show(ex.Message);
            }
          
        }

    }
}
