using GraphGeneration;
using GraphGeneration.BoruvkaMST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Graph? Graph { get; set; }
        int nodeCount=0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int.TryParse(textBox.Text, out this.nodeCount);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Graph = GraphGeneration.GraphGeneration.GenerateGraph(this.nodeCount);
            var canvas = this.FindName("Canvas") as Canvas;
            DrawGraph(this.Graph, canvas);
            (this.FindName("Generate_Minimum_Spanning_Tree") as Button).IsEnabled = true;
            (this.FindName("MstWeightLabel") as Label).Content = "";
        }
        private void DrawGraph(Graph graph, Canvas canvas)
        {
            canvas.Children.Clear();
            const int CircleDiameter = 20;
            foreach (var node in graph.Nodes)
            {
                Ellipse ellipse = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                canvas.Children.Add(ellipse);
                Canvas.SetTop(ellipse, node.YCoordinate - CircleDiameter / 2);
                Canvas.SetLeft(ellipse, node.XCoordinate - CircleDiameter / 2);
            }
            foreach (var edge in graph.Edges)
            {
                Line line = new Line { Stroke = Brushes.LightBlue, X1 = edge.FirstNode.XCoordinate, X2 = edge.SecondNode.XCoordinate, Y1 = edge.FirstNode.YCoordinate, Y2 = edge.SecondNode.YCoordinate };
                canvas.Children.Add(line);
            }
        }

        private void Generate_Minimum_Spanning_Tree_Click(object sender, RoutedEventArgs e)
        {
            BoruvkaMSTGenerator MSTGenerator = new BoruvkaMSTGenerator(this.Graph.Nodes.ToList(),this.Graph.Edges.ToList());
            var a =MSTGenerator.Generate();
            var canvas = this.FindName("Canvas") as Canvas;
            DrawMST(a, canvas);

        }
        private void DrawMST(Tuple<List<Edge>, double> MST, Canvas canvas)
        {
            //canvas.Children.Clear();
            //const int CircleDiameter = 20;
            foreach (var edge in MST.Item1)
            {
                //Ellipse ellipse1 = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                //canvas.Children.Add(ellipse1);
                //Canvas.SetTop(ellipse1, edge.FirstNode.YCoordinate - CircleDiameter / 2);
                //Canvas.SetLeft(ellipse1, edge.FirstNode.XCoordinate - CircleDiameter / 2);
                //Ellipse ellipse2 = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                //canvas.Children.Add(ellipse2);
                //Canvas.SetTop(ellipse2, edge.SecondNode.YCoordinate - CircleDiameter / 2);
                //Canvas.SetLeft(ellipse2, edge.SecondNode.XCoordinate - CircleDiameter / 2);
                Line line = new Line { Stroke = Brushes.Green, StrokeThickness=3, X1 = edge.FirstNode.XCoordinate, X2 = edge.SecondNode.XCoordinate, Y1 = edge.FirstNode.YCoordinate, Y2 = edge.SecondNode.YCoordinate };
                canvas.Children.Add(line);
            }
            (this.FindName("MstWeightLabel") as Label).Content = "MST Weight: " + Math.Round(MST.Item2,2);
            (this.FindName("Generate_Minimum_Spanning_Tree") as Button).IsEnabled = false;
        }
    }
}
