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

namespace MTFramework.SL.Controls
{
    public partial class WindowResize : UserControl
    {
        public WindowResize()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 填充图型的阴影颜色
        /// <para>SolidColorBrush s = new SolidColorBrush(Color.FromArgb(255, 255, 255,255));</para>
        /// </summary>
        public Brush PtResizeShadowFill
        {
            get
            {
                return this.ptResizeShadow.Fill;
            }
            set
            {
                this.ptResizeShadow.Fill = value;
            }
        }

        /// <summary>
        /// 填充图型的颜色
        /// <para>SolidColorBrush s = new SolidColorBrush(Color.FromArgb(255, 255, 255,255));</para>
        /// </summary>
        public Brush PtResizeFill
        {
            get
            {
                return this.ptResize.Fill;
            }
            set
            {
                this.ptResize.Fill = value;
            }
        }



        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseEnter_1(object sender, MouseEventArgs e)
        {
            //更改鼠标图案更改为修改大小
            this.Cursor = Cursors.SizeNWSE;
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeave_1(object sender, MouseEventArgs e)
        {
            //更改鼠标图案更改为箭头
            this.Cursor = Cursors.Arrow;
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragResize(WindowResizeEdge.BottomRight);
        }


    }
}
