using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShapeDraw
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void DPolyLine_PolylineEndDrawed(object sender, DrawEventArgs e)
        {
            string s = "";
            foreach(Point p in e.Points)
            {
                s += string.Format("{0},{1} ",p.X,p.Y);
            }
            
            MessageBox.Show(s);
        }

        private void DPolyLine_PointDrawed(object sender, DrawEventArgs e)
        {
            
        }

        



    }
}
