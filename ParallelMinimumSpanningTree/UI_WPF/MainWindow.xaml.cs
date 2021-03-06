using GraphGeneration;
using GraphGeneration.BoruvkaMST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        bool doNotDisplayGraphOnGeneration = false;
        bool doNotDisplayNodes = false;
        bool disableAllVisualDisplay = false;

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
            var canvas = this.FindName("Canvas") as Canvas;
            canvas.Children.Clear();
            this.Graph = GraphGeneration.GraphGeneration.GenerateGraph(this.nodeCount);
            if (!disableAllVisualDisplay)
            {
                DrawGraph(this.Graph, canvas);
            }
            (this.FindName("Generate_Minimum_Spanning_Tree") as Button).IsEnabled = true;
            (this.FindName("MstWeightLabel") as Label).Content = "";
        }
        private void DrawGraph(Graph graph, Canvas canvas)
        {
            if (!this.doNotDisplayGraphOnGeneration)
            {
                if (!this.doNotDisplayNodes)
                {
                    const int CircleDiameter = 20;
                    foreach (var node in graph.Nodes)
                    {
                        Ellipse ellipse = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                        canvas.Children.Add(ellipse);
                        Canvas.SetTop(ellipse, node.YCoordinate - CircleDiameter / 2);
                        Canvas.SetLeft(ellipse, node.XCoordinate - CircleDiameter / 2);
                    }
                }
                foreach (var edge in graph.Edges)
                {
                    Line line = new Line { Stroke = Brushes.LightBlue, X1 = edge.FirstNode.XCoordinate, X2 = edge.SecondNode.XCoordinate, Y1 = edge.FirstNode.YCoordinate, Y2 = edge.SecondNode.YCoordinate };
                    canvas.Children.Add(line);
                }
            }
        }

        private void Generate_Minimum_Spanning_Tree_Click(object sender, RoutedEventArgs e)
        {
            var canvas = this.FindName("Canvas") as Canvas;
            BoruvkaMSTGenerator MSTGenerator = new BoruvkaMSTGenerator(this.Graph.Nodes.ToList(),this.Graph.Edges.ToList());
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var a =MSTGenerator.Generate();
            stopwatch.Stop();
            (this.FindName("MstWeightLabel") as Label).Content = "MST Weight: " + Math.Round(a.Item2, 2)+"\nTime taken (milliseconds): "+stopwatch.ElapsedMilliseconds;
            (this.FindName("Generate_Minimum_Spanning_Tree") as Button).IsEnabled = false;

            if (!this.disableAllVisualDisplay)
            {
                DrawMST(a, canvas);
            }


        }
        private void DrawMST(Tuple<List<Edge>, double> MST, Canvas canvas)
        {
            if (this.doNotDisplayNodes)
            {
                canvas.Children.Clear();
            }
            foreach (var edge in MST.Item1)
            {
                //if (this.doNotDisplayGraphOnGeneration && !this.doNotDisplayNodes)
                //{
                //    const int CircleDiameter = 20;
                //    Ellipse ellipse1 = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                //    canvas.Children.Add(ellipse1);
                //    Canvas.SetTop(ellipse1, edge.FirstNode.YCoordinate - CircleDiameter / 2);
                //    Canvas.SetLeft(ellipse1, edge.FirstNode.XCoordinate - CircleDiameter / 2);
                //    Ellipse ellipse2 = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                //    canvas.Children.Add(ellipse2);
                //    Canvas.SetTop(ellipse2, edge.SecondNode.YCoordinate - CircleDiameter / 2);
                //    Canvas.SetLeft(ellipse2, edge.SecondNode.XCoordinate - CircleDiameter / 2);
                //}
                Line line = new Line { Stroke = Brushes.Green, StrokeThickness=3, X1 = edge.FirstNode.XCoordinate, X2 = edge.SecondNode.XCoordinate, Y1 = edge.FirstNode.YCoordinate, Y2 = edge.SecondNode.YCoordinate };
                canvas.Children.Add(line);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.doNotDisplayGraphOnGeneration = !this.doNotDisplayGraphOnGeneration;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            this.doNotDisplayNodes = !this.doNotDisplayNodes;
        }
        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            this.disableAllVisualDisplay = !this.disableAllVisualDisplay;
        }
    }
}
