using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Generic;

namespace ShapeDraw
{
    public class DPolyLine : Shape
    {
		//Author			 : ysisl
		//Udpate Date	 : 2/3/2010

        private Polyline _polyline;
        private PointCollection Points = new PointCollection();
        private List<Ellipse> _joinPoints = new List<Ellipse>(); 
        private Canvas _parent;
        private bool IsCompleted;

        public event EventHandler PolylineStartDrawed;
        public event EventHandler<DrawEventArgs> PolylineEndDrawed;
        public event EventHandler<DrawEventArgs> PointDrawed;

        public DPolyLine()
        {
            this.Loaded += (s, e) => InitVisualize();
            PointDrawed += (s,e) => 
            {
                DrawJoinPoint(e.CurrentPoint,Colors.Red,Colors.Black,15,5);
            };
        }

        private void InitVisualize()
        {
            try
            {
                _parent = this.Parent as Canvas;
            }
            catch (Exception)
            {
                throw new Exception("DPolyLine 的父元素应该指定为Canvas类型！");
            }

            _polyline = new Polyline();
            _polyline.StrokeThickness = this.StrokeThickness;
            _polyline.StrokeLineJoin = this.StrokeLineJoin;
            _polyline.StrokeDashCap = this.StrokeDashCap;
            _polyline.StrokeEndLineCap = this.StrokeEndLineCap;
            _polyline.StrokeStartLineCap = this.StrokeStartLineCap;
            _polyline.Fill = this.Fill == null ? new SolidColorBrush(Color.FromArgb(0x88, 0xff, 0xff, 0x00)) : this.Fill;
            _polyline.Stroke = this.Stroke == null ? new SolidColorBrush(Colors.Black) : this.Stroke;
            _polyline.StrokeThickness = _polyline.StrokeThickness < 1? 2.0 : _polyline.StrokeThickness;
            _polyline.Points = this.Points;
            
            _parent.Children.Add(_polyline);
            _parent.MouseLeftButtonDown += new MouseButtonEventHandler(_parent_MouseLeftButtonDown);
            _parent.MouseMove += new MouseEventHandler(_parent_MouseMove);

        }

        private void _parent_MouseMove(object sender, MouseEventArgs e)
        {
            LastPoint = e.GetPosition(_parent);
            if (CheckCompleted())
                _parent.Cursor = Cursors.Hand;
            else
                _parent.Cursor = Cursors.Arrow;
        }

        private void _parent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCompleted)
            {
                MessageBoxResult res = MessageBox.Show("Graphic is Drawed Completed,Draw it again ?", "Tip", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                    ClearCanvas();
                return;
            }
            
            DrawEventArgs args = new DrawEventArgs();

            if (Points.Count == 0)
            {
                if (PolylineStartDrawed != null)
                    PolylineStartDrawed(sender, e);
                Points.Add(e.GetPosition(_parent));

                args.CurrentPoint = Points[0];
                if (PointDrawed != null)
                    PointDrawed(sender, args);
            }
            else
            {
                Points[Points.Count - 1] = LastPoint;
                if (CheckCompleted())
                {
                    LastPoint = Points[0];
                    IsCompleted = true;
                    if (PolylineEndDrawed != null)
                    {
                        args.Points = GetPoints();
                        PolylineEndDrawed(sender, args);
                    }
                    return;
                }
                args.CurrentPoint = LastPoint;
                if (PointDrawed != null)
                    PointDrawed(sender, args);
            }
            Points.Add(e.GetPosition(_parent));
            

        }

        public void ClearCanvas()
        {
            this.Points.Clear();
            foreach (Ellipse e in _joinPoints)
            {
                _parent.Children.Remove(e);
            }
            _joinPoints.Clear();
            IsCompleted = false;
        }
        
        private void DrawJoinPoint(Point p,Color fill,Color stoke,double radio,double lineThickness)
        {
                Ellipse e = new Ellipse();
                e.Fill = new SolidColorBrush(fill);
                e.Stroke = new SolidColorBrush(stoke);
                e.Height = e.Width = radio + lineThickness ;
                e.StrokeThickness = lineThickness ;

                e.SetValue(Canvas.LeftProperty, p.X - e.Width / 2);
                e.SetValue(Canvas.TopProperty, p.Y - e.Height / 2);
                
                _parent.Children.Add(e);
                _joinPoints.Add(e);
        }

        private bool CheckCompleted()
        {
            return Points.Count > 0 && Math.Abs(Points[0].X - LastPoint.X) < 2 && Math.Abs(Points[0].Y - LastPoint.Y) < 2;
        }

        private static DependencyProperty LastPointProperty = DependencyProperty.Register(
            "LastPoint", typeof(Point), typeof(DPolyLine),
            new PropertyMetadata(new PropertyChangedCallback(LastPointChanged)));

        public Point LastPoint
        {
            get { return (Point)GetValue(LastPointProperty); }
            set { SetValue(LastPointProperty, value); }
        }

        public static void LastPointChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DPolyLine poly = sender as DPolyLine;
            if (!poly.IsCompleted && poly.Points.Count > 0)
            {
                poly.Points[poly.Points.Count - 1] = (Point)e.NewValue;
            }
        }

        public List<Point> GetPoints()
        {
            List<Point> points = new List<Point>(Points.Count);
            foreach (Point p in Points)
                points.Add(p);
            return points;
        }

    }
}
