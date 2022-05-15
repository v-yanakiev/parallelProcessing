﻿using GraphGeneration;
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
            this.Graph = Generator.GenerateGraph(this.nodeCount);
            var canvas = this.FindName("Canvas") as Canvas;
            DrawGraph(this.Graph, canvas);
        }
        private void DrawGraph(Graph graph, Canvas canvas)
        {
            canvas.Children.Clear();
            const int CircleDiameter = 20;
            foreach (var node in graph.Nodes)
            {
                Ellipse ellipse = new Ellipse() { Height = CircleDiameter, Width = CircleDiameter, Fill = Brushes.Black };
                canvas.Children.Add(ellipse);
                Canvas.SetTop(ellipse, node.YCoordinate*5- CircleDiameter/2);
                Canvas.SetLeft(ellipse, node.XCoordinate*10- CircleDiameter/2);
            }
            foreach(var edge in graph.Edges)
            {
                Line line =new Line {Stroke=Brushes.LightBlue, X1=edge.FirstNode.XCoordinate * 10, X2=edge.SecondNode.XCoordinate * 10, Y1=edge.FirstNode.YCoordinate * 5, Y2=edge.SecondNode.YCoordinate * 5 };
                canvas.Children.Add(line);
            }
        }
    }
}