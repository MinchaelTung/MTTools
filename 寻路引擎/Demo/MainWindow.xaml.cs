using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MTFramework.Engines.MapPathfindingEngine;

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle _Rect;
        private IPathFinder _PathFinder = null;
        //寻路用二维矩阵
        private byte[,] _Matrix = new byte[1024, 1024];
        //单位格子大小
        private int _GridSize = 20;
        //移动起点坐标
        private System.Drawing.Point _Start = System.Drawing.Point.Empty;
        //移动终点坐标
        private System.Drawing.Point _End = System.Drawing.Point.Empty;
        //游戏角色
        Ellipse _Player = new Ellipse();

        public MainWindow()
        {
            InitializeComponent();
            //初始化二维矩阵
            this.resetMatrix();
            //构建障碍物
            this.initObstacles();
            //初始化目标对象
            this.initPlayer();
        }

        /// <summary>
        /// 初始化目标对象
        /// </summary>
        private void initPlayer()
        {
            _Player.Fill = new SolidColorBrush(Colors.Blue);
            _Player.Width = _GridSize;
            _Player.Height = _GridSize;
            this.Carrier.Children.Add(_Player);
            Canvas.SetLeft(_Player, _GridSize);
            Canvas.SetTop(_Player, 5 * _GridSize);
            //Start = new System.Drawing.Point(1, 1); //设置起点坐标
        }

        /// <summary>
        /// 初始化二维矩阵
        /// </summary>
        private void resetMatrix()
        {
            for (int y = 0; y < _Matrix.GetUpperBound(1); y++)
            {
                for (int x = 0; x < _Matrix.GetUpperBound(0); x++)
                {
                    //默认值可以通过在矩阵中用1表示
                    _Matrix[x, y] = 1;
                }
            }
        }

        #region --- 构建障碍物 Begin ---

        /// <summary>
        /// 构建障碍物
        /// </summary>
        private void initObstacles()
        {
            for (int y = 3; y < 36; y++)
            {
                //障碍物在矩阵中用0表示
                _Matrix[3, y] = 0;
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Red);
                _Rect.Width = _GridSize;
                _Rect.Height = _GridSize;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, 3 * _GridSize);
                Canvas.SetTop(_Rect, y * _GridSize);
            }
            for (int y = 3; y < 30; y++)
            {
                //障碍物在矩阵中用0表示
                _Matrix[24, y] = 0;
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Red);
                _Rect.Width = _GridSize;
                _Rect.Height = _GridSize;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, 24 * _GridSize);
                Canvas.SetTop(_Rect, y * _GridSize);
            }


            for (int y = 3; y < 25; y++)
            {
                //障碍物在矩阵中用0表示
                _Matrix[30, y] = 0;
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Red);
                _Rect.Width = _GridSize;
                _Rect.Height = _GridSize;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, 30 * _GridSize);
                Canvas.SetTop(_Rect, y * _GridSize);
            }

            for (int i = 0; i < 18; i++)
            {
                //障碍物在矩阵中用0表示
                _Matrix[i, 12] = 0;
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Red);
                _Rect.Width = _GridSize;
                _Rect.Height = _GridSize;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, i * _GridSize);
                Canvas.SetTop(_Rect, 12 * _GridSize);
            }
            for (int i = 12; i < 17; i++)
            {
                _Matrix[17, i] = 0;
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Red);
                _Rect.Width = _GridSize;
                _Rect.Height = _GridSize;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, 17 * _GridSize);
                Canvas.SetTop(_Rect, i * _GridSize);
            }
            for (int i = 3; i < 18; i++)
            {
                _Matrix[i, 16] = 0;
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Red);
                _Rect.Width = _GridSize;
                _Rect.Height = _GridSize;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, i * _GridSize);
                Canvas.SetTop(_Rect, 16 * _GridSize);
            }
        }

        #endregion --- 构建障碍物 End ---

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Carrier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(Carrier);
            //开始位置
            int start_x = (int)Canvas.GetLeft(_Player) / _GridSize;
            int start_y = (int)Canvas.GetTop(_Player) / _GridSize;
            _Start = new System.Drawing.Point(start_x, start_y);
            //结束位置
            int end_x = (int)p.X / _GridSize;
            int end_y = (int)p.Y / _GridSize;
            _End = new System.Drawing.Point(end_x, end_y);

            _PathFinder = new PathFinderFast(_Matrix);
            //使用算法
            _PathFinder.Formula = HeuristicFormula.Manhattan;
            //使用对角线 8方向
            _PathFinder.Diagonals = true;
            //发动对角线计算
            _PathFinder.HeavyDiagonals = true;
            //发动计算数量
            _PathFinder.HeuristicEstimate = 20000;

            List<PathFinderNode> path = _PathFinder.FindPath(_Start, _End);

            if (path == null)
            {
                MessageBox.Show("路径不存在");
                return;
            }
            //定义关键帧坐标集
            Point[] framsPosition = new Point[path.Count];

            for (int i = path.Count - 1; i >= 0; i--)
            {
                //从起点开始以GridSize为单位，顺序填充关键帧坐标集，并进行坐标系放大
                framsPosition[path.Count - 1 - i] = new Point(path[i].X * _GridSize, path[i].Y * _GridSize);
            }

            //创建故事板
            Storyboard storyboard = new Storyboard();
            //每移动一个小方格(20*20)花费100毫秒
            int cost = 100;
            //创建X轴方向逐帧动画
            DoubleAnimationUsingKeyFrames keyFramesAnimationX = new DoubleAnimationUsingKeyFrames();
            //总共花费时间 = path.Count * cost
            keyFramesAnimationX.Duration = new Duration(TimeSpan.FromMilliseconds(path.Count * cost));
            Storyboard.SetTarget(keyFramesAnimationX, _Player);
            Storyboard.SetTargetProperty(keyFramesAnimationX, new PropertyPath("(Canvas.Left)"));
            //创建Y轴方向逐帧动画
            DoubleAnimationUsingKeyFrames keyFramesAnimationY = new DoubleAnimationUsingKeyFrames();
            keyFramesAnimationY.Duration = new Duration(TimeSpan.FromMilliseconds(path.Count * cost));
            Storyboard.SetTarget(keyFramesAnimationY, _Player);
            Storyboard.SetTargetProperty(keyFramesAnimationY, new PropertyPath("(Canvas.Top)"));

            for (int i = 0; i < framsPosition.Count(); i++)
            {
                //加入X轴方向的匀速关键帧
                LinearDoubleKeyFrame keyFrame = new LinearDoubleKeyFrame();
                keyFrame.Value = (i == 0 ? Canvas.GetLeft(_Player) : framsPosition[i].X);
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(cost * i));
                keyFramesAnimationX.KeyFrames.Add(keyFrame);
                //加入Y轴方向的匀速关键帧
                keyFrame = new LinearDoubleKeyFrame();
                keyFrame.Value = i == 0 ? Canvas.GetTop(_Player) : framsPosition[i].Y;
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(cost * i));
                keyFramesAnimationY.KeyFrames.Add(keyFrame);

            }
            storyboard.Children.Add(keyFramesAnimationX);
            storyboard.Children.Add(keyFramesAnimationY);
            //添加进资源
            if (!Resources.Contains("storyboard") == true)
            {
                Resources.Add("storyboard", storyboard);
            }
            //故事板动画开始
            storyboard.Begin();
            //用白色点记录移动轨迹
            for (int i = path.Count - 1; i >= 0; i--)
            {
                _Rect = new Rectangle();
                _Rect.Fill = new SolidColorBrush(Colors.Snow);
                _Rect.Width = 5;
                _Rect.Height = 5;
                Carrier.Children.Add(_Rect);
                Canvas.SetLeft(_Rect, path[i].X * _GridSize);
                Canvas.SetTop(_Rect, path[i].Y * _GridSize);

            }
        }
    }
}
