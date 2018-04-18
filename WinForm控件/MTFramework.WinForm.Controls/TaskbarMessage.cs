/**
 * 创建对象
 * TaskbarMessage taskbarMessage1 = new TaskbarMessage();
 * 定义背景
 * taskbarMessage1.SetBackgroundBitmap(new Bitmap(GetType(),"skin.bmp"),Color.FromArgb(255,0,255));
 * 定义关闭按钮
 * taskbarMessage1.SetCloseBitmap(new Bitmap(GetType(),"close.bmp"),Color.FromArgb(255,0,255),new Point(127,8));
 * 定义标题绘制矩形位置
 * taskbarMessage1.TitleRectangle=new Rectangle(40,9,70,25);
 * 定义信息内容绘制矩形位置
 * taskbarMessage1.ContentRectangle=new Rectangle(8,41,133,68);
 * 定义点击事件
 * taskbarMessage1.TitleClick+=new EventHandler(TitleClick);
 * taskbarMessage1.ContentClick+=new EventHandler(ContentClick);
 * taskbarMessage1.CloseClick+=new EventHandler(CloseClick); 
 * 
 * 
 * 触发显示及可以动态更变属性
 * 是否使用点击关闭
 * taskbarMessage1.CloseClickable = true||false;
 * 是否使用点击标题
 * taskbarMessage1.TitleClickable =  true||false;
 * 是否使用点击内容
 * taskbarMessage1.ContentClickable =  true||false;
 * 是否使用选择绘制矩形
 * taskbarMessage1.EnableSelectionRectangle =  true||false;
 * 鼠标在任务栏消息上是否维持显示状态
 * taskbarMessage1.KeepVisibleOnMouseOver =  true||false;	
 * 再次出现的举动老鼠它当它的消失
 * taskbarMessage1.ReShowOnMouseOver =  true||false;			
 * 显示任务栏消息框
 * taskbarMessage1.Show("标题文本","内容文本",(int)出现时间,(int)维持时间,(int)隐藏时间);
 * 
 **/



using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace MTFramework.WinForm.Controls
{

    /// <summary>
    /// 任务栏消息
    /// </summary>
    public class TaskbarMessage : System.Windows.Forms.Form
    {
        /// <summary>
        /// 任务栏显示状态
        /// </summary>
        public enum TaskbarStates
        {
            /// <summary>
            /// 隐藏
            /// </summary>
            Hidden = 0,
            /// <summary>
            /// 出现
            /// </summary>
            Appearing = 1,
            /// <summary>
            /// 可见
            /// </summary>
            Visible = 2,
            /// <summary>
            /// 消失
            /// </summary>
            Disappearing = 3
        }

        #region --- 成员 Begin ---

        /// <summary>
        /// 背景图片
        /// </summary>
        private Bitmap _BackgroundImage = null;

        /// <summary>
        /// 关闭按钮图片
        /// </summary>
        private Bitmap _CloseImage = null;

        /// <summary>
        /// 关闭按钮图片位置
        /// </summary>
        private Point _CloseImageLocation;

        /// <summary>
        /// 关闭按钮图片尺寸
        /// </summary>
        private Size _CloseImageSize;

        /// <summary>
        /// 读取标题框绘制图形
        /// </summary>
        private Rectangle _RealTitleRectangle;

        /// <summary>
        /// 读取内容框绘制图形
        /// </summary>
        private Rectangle _RealContentRectangle;

        /// <summary>
        /// 工作区域绘制图形
        /// </summary>
        private Rectangle _WorkAreaRectangle;

        /// <summary>
        /// 任务栏信息动态速度时间
        /// </summary>
        private System.Windows.Forms.Timer _TaskbarMessageTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// 任务栏状态
        /// </summary>
        private TaskbarStates _TaskbarState = TaskbarStates.Hidden;
        /// <summary>
        /// 任务栏状态
        /// </summary>
        public TaskbarStates TaskbarState
        {
            get
            {
                return this._TaskbarState;
            }
        }

        #region --- Title Begin ---

        /// <summary>
        /// 标题文本
        /// </summary>
        private string _TitleText;
        /// <summary>
        /// 标题文本
        /// </summary>
        public string TitleText
        {
            get
            {
                return this._TitleText;
            }
            set
            {
                this._TitleText = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 标题标准颜色
        /// </summary>
        private Color _NormalTitleColor = Color.FromArgb(255, 0, 0);
        /// <summary>
        /// 标题标准颜色
        /// </summary>
        public Color NormalTitleColor
        {
            get
            {
                return this._NormalTitleColor;
            }
            set
            {
                this._NormalTitleColor = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 标题焦点颜色
        /// </summary>
        private Color _HoverTitleColor = Color.FromArgb(255, 0, 0);
        /// <summary>
        /// 标题焦点颜色
        /// </summary>
        public Color HoverTitleColor
        {
            get
            {
                return this._HoverTitleColor;
            }
            set
            {
                this._HoverTitleColor = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 标题标准字体
        /// </summary>
        private Font _NormalTitleFont = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        /// <summary>
        /// 标题标准字体
        /// </summary>
        public Font NormalTitleFont
        {
            get
            {
                return this._NormalTitleFont;
            }
            set
            {
                this._NormalTitleFont = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 标题焦点字体
        /// </summary>
        private Font _HoverTitleFont = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
        /// <summary>
        /// 标题焦点字体
        /// </summary>
        public Font HoverTitleFont
        {
            get
            {
                return this._HoverTitleFont;
            }
            set
            {
                this._HoverTitleFont = value;
                this.Refresh();
            }
        }

        #endregion --- Title End ---

        #region --- Content Begin ---

        /// <summary>
        /// 内容文本
        /// </summary>
        private string _ContentText;
        /// <summary>
        /// 内容文本
        /// </summary>
        public string ContentText
        {
            get
            {
                return this._ContentText;
            }
            set
            {
                this._ContentText = value;
                this.Refresh();

            }
        }

        /// <summary>
        /// 内容标准颜色
        /// </summary>
        private Color _NormalContentColor = Color.FromArgb(0, 0, 0);
        /// <summary>
        /// 内容标准颜色
        /// </summary>
        public Color NormalContentColor
        {
            get
            {
                return this._NormalContentColor;
            }
            set
            {
                this._NormalContentColor = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 内容焦点颜色
        /// </summary>
        private Color _HoverContentColor = Color.FromArgb(0, 0, 102);
        /// <summary>
        /// 内容焦点颜色
        /// </summary>
        public Color HoverContentColor
        {
            get
            {
                return this._HoverContentColor;
            }
            set
            {
                this._HoverContentColor = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 内容标准字体
        /// </summary>
        private Font _NormalContentFont = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel);
        /// <summary>
        /// 内容标准字体
        /// </summary>
        public Font NormalContentFont
        {
            get
            {
                return this._NormalContentFont;
            }
            set
            {
                this._NormalContentFont = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 内容焦点字体
        /// </summary>
        private Font _HoverContentFont = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel);

        #endregion --- Content End ---

        /// <summary>
        /// 显示事件时间轴
        /// </summary>
        private int _ShowEvents;

        /// <summary>
        /// 隐藏时间时间轴
        /// </summary>
        private int _HideEvents;

        /// <summary>
        /// 可见事件时间轴
        /// </summary>
        private int _VisibleEvents;

        /// <summary>
        /// 增加显示时间轴
        /// </summary>
        private int _IncrementShow;

        /// <summary>
        /// 增加隐藏时间轴
        /// </summary>
        private int _IncrementHide;

        /// <summary>
        /// 鼠标是否在任务栏信息窗口上
        /// </summary>
        private bool _IsMouseOverPopup = false;

        /// <summary>
        /// 鼠标是否在关闭按钮位置上
        /// </summary>
        private bool _IsMouseOverClose = false;

        /// <summary>
        /// 鼠标是否在标题位置上
        /// </summary>
        private bool _IsMouseOverTitle = false;

        /// <summary>
        /// 鼠标是否在内容位置上
        /// </summary>
        private bool _IsMouseOverContent = false;

        /// <summary>
        /// 鼠标是否点击
        /// </summary>
        private bool _IsMouseDown = false;

        /// <summary>
        /// 鼠标在任务栏信息上是否保持可见
        /// </summary>
        private bool _KeepVisibleOnMouseOver = true;
        /// <summary>
        /// 再次出现的举动老鼠它当它的消失
        /// </summary>
        public bool KeepVisibleOnMouseOver
        {
            get
            {
                return this._KeepVisibleOnMouseOver;
            }
            set
            {
                this._KeepVisibleOnMouseOver = value;
            }
        }

        /// <summary>
        /// 鼠标在任务栏信息出现动作中时快速出现
        /// </summary>
        private bool _ReShowOnMouseOver = false;
        /// <summary>
        /// 鼠标在任务栏信息出现动作中时快速出现
        /// </summary>
        public bool ReShowOnMouseOver
        {
            get
            {
                return this._ReShowOnMouseOver;
            }
            set
            {
                this._ReShowOnMouseOver = value;
            }
        }

        /// <summary>
        /// 绘制标题图形
        /// </summary>
        private Rectangle _TitleRectangle;
        /// <summary>
        /// 绘制标题图形
        /// </summary>
        public Rectangle TitleRectangle
        {
            get
            {
                return this._TitleRectangle;
            }
            set
            {
                this._TitleRectangle = value;
            }
        }

        /// <summary>
        /// 绘制内容图形
        /// </summary>
        private Rectangle _ContentRectangle;
        /// <summary>
        /// 绘制内容图形
        /// </summary>
        public Rectangle ContentRectangle
        {
            get
            {
                return this._ContentRectangle;
            }
            set
            {
                this._ContentRectangle = value;
            }
        }

        /// <summary>
        /// 点击标题
        /// </summary>
        private bool _TitleClickable = false;
        /// <summary>
        /// 点击标题
        /// </summary>
        public bool TitleClickable
        {
            get
            {
                return this._TitleClickable;
            }
            set
            {
                this._TitleClickable = value;
            }
        }

        /// <summary>
        /// 点击内容
        /// </summary>
        private bool _ContentClickable = true;
        /// <summary>
        /// 点击内容
        /// </summary>
        public bool ContentClickable
        {
            get
            {
                return this._ContentClickable;
            }
            set
            {
                this._ContentClickable = value;
            }
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        private bool _CloseClickable = true;
        /// <summary>
        /// 点击关闭
        /// </summary>
        public bool CloseClickable
        {
            get
            {
                return this._CloseClickable;
            }
            set
            {
                this._CloseClickable = value;
            }
        }

        /// <summary>
        /// 使用选择图形
        /// </summary>
        private bool _EnableSelectionRectangle = true;
        /// <summary>
        /// 使用选择图形
        /// </summary>
        public bool EnableSelectionRectangle
        {
            get
            {
                return this._EnableSelectionRectangle;
            }
            set
            {
                this._EnableSelectionRectangle = value;
            }
        }


        #endregion --- 成员 End ---

        #region --- 事件定义 Begin ---
        /// <summary>
        /// 点击关闭事件
        /// </summary>
        public event EventHandler CloseClick = null;

        /// <summary>
        /// 点击标题事件
        /// </summary>
        public event EventHandler TitleClick = null;

        /// <summary>
        /// 点击内容事件
        /// </summary>
        public event EventHandler ContentClick = null;

        #endregion --- 事件定义 End ---

        #region --- Ctor Begin ---

        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskbarMessage()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            base.Show();
            base.Hide();
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this._TaskbarMessageTimer.Enabled = true;
            this._TaskbarMessageTimer.Tick += new EventHandler(_TaskbarMessageTimer_Tick);
        }

        #endregion --- Ctor End ---

        #region --- 调用方法 Begin ---
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        /// <summary>
        /// 显示信息标题、内容和显示时间
        /// </summary>
        /// <param name="title">标题文本</param>
        /// <param name="content">内容文本</param>
        /// <param name="showTime">出现时间</param>
        /// <param name="stayTime">维持时间</param>
        /// <param name="hideTime">隐藏时间</param>
        public void Show(string title, string content, int showTime, int stayTime, int hideTime)
        {
            this._WorkAreaRectangle = System.Windows.Forms.Screen.GetWorkingArea(this._WorkAreaRectangle);
            this._TitleText = title;
            this._ContentText = content;
            this._VisibleEvents = stayTime;
            this.CalculateRectangles();

            //临时计算参数
            int nEvents;

            #region --- 计算显示动画效果时间轴 Begin ---

            if (showTime > 10)
            {
                nEvents = Math.Min((showTime / 10), this._BackgroundImage.Height);
                this._ShowEvents = showTime / nEvents;
                this._IncrementShow = this._BackgroundImage.Height / nEvents;
            }
            else
            {
                this._ShowEvents = 10;
                this._IncrementShow = this._BackgroundImage.Height;
            }

            #endregion --- 计算显示动画效果时间轴 End ---

            #region --- 计算隐藏动画效果时间轴 Begin ---

            if (hideTime > 10)
            {
                nEvents = Math.Min((hideTime / 10), this._BackgroundImage.Height);
                this._HideEvents = hideTime / nEvents;
                this._IncrementHide = this._BackgroundImage.Height / nEvents;
            }
            else
            {
                this._HideEvents = 10;
                this._IncrementHide = this._BackgroundImage.Height;
            }

            #endregion --- 计算隐藏动画效果时间轴 End ---

            switch (this._TaskbarState)
            {
                case TaskbarStates.Hidden:
                    this._TaskbarState = TaskbarStates.Appearing;
                    this.SetBounds(this._WorkAreaRectangle.Right - this._BackgroundImage.Width - 17, this._WorkAreaRectangle.Bottom - 1, this._BackgroundImage.Width, 0);
                    this._TaskbarMessageTimer.Interval = this._ShowEvents;
                    this._TaskbarMessageTimer.Start();
                    ShowWindow(this.Handle, 4);
                    break;
                case TaskbarStates.Appearing:
                    this.Refresh();
                    break;
                case TaskbarStates.Visible:
                    this._TaskbarMessageTimer.Stop();
                    this._TaskbarMessageTimer.Interval = this._VisibleEvents;
                    this._TaskbarMessageTimer.Start();
                    this.Refresh();
                    break;
                case TaskbarStates.Disappearing:
                    this._TaskbarMessageTimer.Stop();
                    this._TaskbarState = TaskbarStates.Visible;
                    this.SetBounds(this._WorkAreaRectangle.Right - this._BackgroundImage.Width - 17, this._WorkAreaRectangle.Bottom - this._BackgroundImage.Height - 1, this._BackgroundImage.Width, this._BackgroundImage.Height);
                    this._TaskbarMessageTimer.Interval = this._VisibleEvents;
                    this._TaskbarMessageTimer.Start();
                    this.Refresh();
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 隐藏任务栏消息框
        /// </summary>
        public new void Hide()
        {
            if (this._TaskbarState != TaskbarStates.Hidden)
            {
                this._TaskbarMessageTimer.Stop();
                this._TaskbarState = TaskbarStates.Hidden;
                base.Hide();
            }
        }

        /// <summary>
        /// 设置背景图案和透明度颜色
        /// </summary>
        /// <param name="fileName">背景图案路径</param>
        /// <param name="transparencyColor">透明度颜色</param>
        public void SetBackgroundBitmap(string backgroundImageFileName, Color transparencyColor)
        {
            this._BackgroundImage = new Bitmap(backgroundImageFileName);
            this.Width = this._BackgroundImage.Width;
            this.Height = this._BackgroundImage.Height;
            this.Region = this.BimapToRegion(this._BackgroundImage, transparencyColor);
        }

        /// <summary>
        /// 设置背景图案和透明度颜色
        /// </summary>
        /// <param name="image">背景图案</param>
        /// <param name="transparencyColor">透明度颜色</param>
        public void SetBackgroundBitmap(Image backgroundImage, Color transparencyColor)
        {
            this._BackgroundImage = new Bitmap(backgroundImage);
            this.Width = this._BackgroundImage.Width;
            this.Height = this._BackgroundImage.Height;
            this.Region = this.BimapToRegion(this._BackgroundImage, transparencyColor);

        }

        /// <summary>
        /// 设置关闭按钮背景和透明度颜色
        /// </summary>
        /// <param name="closeImageFileName">关闭按钮背景图案路径</param>
        /// <param name="transparencyColor">透明度颜色</param>
        /// <param name="position">关闭按钮位置</param>
        public void SetCloseBitmap(string closeImageFileName, Color transparencyColor, Point position)
        {
            this._CloseImage = new Bitmap(closeImageFileName);
            this._CloseImage.MakeTransparent(transparencyColor);
            this._CloseImageSize = new Size(this._CloseImage.Width , this._CloseImage.Height);
            this._CloseImageLocation = position;
        }

        /// <summary>
        /// 设置关闭按钮背景和透明度颜色
        /// </summary>
        /// <param name="closeImage">关闭按钮图案</param>
        /// <param name="transparencyColor">透明度颜色</param>
        /// <param name="position">关闭按钮位置</param>
        public void SetCloseBitmap(Image closeImage, Color transparencyColor, Point position)
        {
            this._CloseImage = new Bitmap(closeImage);
            this._CloseImage.MakeTransparent(transparencyColor);
            this._CloseImageSize = new Size(this._CloseImage.Width , this._CloseImage.Height);
            this._CloseImageLocation = position;
        }

        #endregion --- 调用方法 End ---

        #region --- 内部辅助方法 Begin ---

        /// <summary>
        /// 绘画关闭按钮
        /// </summary>
        /// <param name="grfx"></param>
        private void DrawCloseButton(Graphics grfx)
        {
            if (this._CloseImage != null)
            {
                Rectangle rect = new Rectangle(this._CloseImageLocation, this._CloseImageSize);
                Rectangle strRect;
                if (this._IsMouseOverClose == true)
                {
                    if (this._IsMouseDown == true)
                    {
                        strRect = new Rectangle(new Point(this._CloseImageSize.Width * 2, 0), this._CloseImageSize);
                    }
                    else
                    {
                        strRect = new Rectangle(new Point(this._CloseImageSize.Width, 0), this._CloseImageSize);
                    }
                }
                else
                {
                    strRect = new Rectangle(new Point(0, 0), this._CloseImageSize);
                }
                grfx.DrawImage(this._CloseImage, rect, strRect, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 绘画标题和内容文本
        /// </summary>
        /// <param name="grfx"></param>
        private void DrawText(Graphics grfx)
        {
            #region --- 绘画标题 Begin ---

            if (string.IsNullOrEmpty(this._TitleText) == false)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                if (this._IsMouseOverTitle == true)
                {
                    grfx.DrawString(this._TitleText, this._HoverTitleFont, new SolidBrush(this._HoverTitleColor), this._TitleRectangle, sf);
                }
                else
                {
                    grfx.DrawString(this._TitleText, this._NormalTitleFont, new SolidBrush(this._NormalTitleColor), this._TitleRectangle, sf);
                }
            }

            #endregion --- 绘画标题 End ---

            #region --- 绘画内容 Begin ---

            if (string.IsNullOrEmpty(this._ContentText) == false)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                sf.Trimming = StringTrimming.Word;

                if (this._IsMouseOverContent == true)
                {
                    grfx.DrawString(this._ContentText, this._HoverContentFont, new SolidBrush(this._HoverContentColor), this._ContentRectangle, sf);
                    if (this._EnableSelectionRectangle == true)
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(grfx, this._RealContentRectangle, System.Windows.Forms.Border3DStyle.Etched, System.Windows.Forms.Border3DSide.Top | System.Windows.Forms.Border3DSide.Bottom | System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Right);
                    }
                }
                else
                {
                    grfx.DrawString(this._ContentText, this._NormalContentFont, new SolidBrush(this._NormalContentColor), this._ContentRectangle, sf);
                }
            }
            #endregion --- 绘画内容 End ---
        }

        /// <summary>
        /// 计算矩形
        /// </summary>
        private void CalculateRectangles()
        {
            Graphics grfx = this.CreateGraphics();
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            SizeF sizefTitle = grfx.MeasureString(this._TitleText, this._HoverTitleFont, this._TitleRectangle.Width, sf);
            SizeF sizefContent = grfx.MeasureString(this._ContentText, this._HoverTitleFont, this._ContentRectangle.Width, sf);
            grfx.Dispose();
            //检查标题是否在预定绘制矩形内，大于原先预定则重新绘画
            if (sizefTitle.Height > this._TitleRectangle.Height)
            {
                this._RealTitleRectangle = new Rectangle(this._TitleRectangle.Left, this._TitleRectangle.Top, this._TitleRectangle.Width, this._TitleRectangle.Height);
            }
            else
            {
                this._RealTitleRectangle = new Rectangle(this._TitleRectangle.Left, this._TitleRectangle.Top, (int)sizefTitle.Width, (int)sizefTitle.Height);
            }
            this._RealTitleRectangle.Inflate(0, 2);

            //检查内容是否在预定绘制矩形内，大于原先预定则重新绘画
            if (sizefContent.Height > this._ContentRectangle.Height)
            {
                this._RealContentRectangle = new Rectangle((this._ContentRectangle.Width - (int)sizefContent.Width) / 2 + this._ContentRectangle.Left, this._ContentRectangle.Top, (int)sizefContent.Width, this._ContentRectangle.Height);
            }
            else
            {
                this._RealContentRectangle = new Rectangle((this._ContentRectangle.Width - (int)sizefContent.Width) / 2 + this._ContentRectangle.Left, (this._ContentRectangle.Height - (int)sizefContent.Height) / 2 + this._ContentRectangle.Top, (int)sizefContent.Width, (int)sizefContent.Height);
            }
            this._RealContentRectangle.Inflate(0, 2);

        }

        /// <summary>
        /// Bitmap转换为Region
        /// </summary>
        /// <param name="bitmap">图片对象</param>
        /// <param name="transparencyColor">透明度颜色</param>
        /// <returns></returns>
        private System.Drawing.Region BimapToRegion(Bitmap bitmap, Color transparencyColor)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("Bitmap", "图片参数不能为 Null !");
            }
            int bitmapHeight = bitmap.Height;
            int bitmapWidth = bitmap.Width;
            GraphicsPath gPath = new GraphicsPath();
            for (int y = 0; y < bitmapHeight; y++)
            {
                for (int x = 0; x < bitmapWidth; x++)
                {
                    if (bitmap.GetPixel(x, y) == transparencyColor)
                    {
                        continue;
                    }
                    int z = x;
                    while ((x < bitmapWidth) && (bitmap.GetPixel(x, y) != transparencyColor))
                    {
                        x++;
                    }
                    gPath.AddRectangle(new Rectangle(z, y, x - z, 1));
                }
            }
            System.Drawing.Region region = new Region(gPath);
            gPath.Dispose();
            return region;
        }

        #endregion --- 内部辅助方法 End ---

        #region --- 触发事件 Begin ---

        /// <summary>
        /// 时间器触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _TaskbarMessageTimer_Tick(object sender, EventArgs e)
        {
            switch (this._TaskbarState)
            {
                case TaskbarStates.Hidden:
                    break;
                case TaskbarStates.Appearing:
                    if (this.Height < this._BackgroundImage.Height)
                    {
                        this.SetBounds(this.Left, this.Top - this._IncrementShow, this.Width, this.Height + this._IncrementShow);
                    }
                    else
                    {
                        this._TaskbarMessageTimer.Stop();
                        this.Height = this._BackgroundImage.Height;
                        this._TaskbarMessageTimer.Interval = this._VisibleEvents;
                        this._TaskbarState = TaskbarStates.Visible;
                        this._TaskbarMessageTimer.Start();
                    }
                    break;
                case TaskbarStates.Visible:
                    this._TaskbarMessageTimer.Stop();
                    this._TaskbarMessageTimer.Interval = this._HideEvents;
                    if ((this._KeepVisibleOnMouseOver == true && this._IsMouseOverPopup == false) || this._KeepVisibleOnMouseOver == false)
                    {
                        this._TaskbarState = TaskbarStates.Disappearing;
                    }
                    this._TaskbarMessageTimer.Start();
                    break;
                case TaskbarStates.Disappearing:
                    if (this._ReShowOnMouseOver == true && this._IsMouseOverPopup)
                    {
                        this._TaskbarState = TaskbarStates.Appearing;
                    }
                    else
                    {
                        if (this.Top < this._WorkAreaRectangle.Bottom)
                        {
                            SetBounds(this.Left, this.Top + this._IncrementHide, this.Width, this.Height - this._IncrementHide);
                        }
                        else
                        {
                            this.Hide();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region --- 重写父类事件 Begin ---

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this._IsMouseOverPopup = true;
            this.Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this._IsMouseOverPopup = false;
            this._IsMouseOverClose = false;
            this._IsMouseOverTitle = false;
            this._IsMouseOverContent = false;
            this.Refresh();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //是否需要刷新界面
            bool isRefresh = false;

            if ((e.X > this._CloseImageLocation.X)
                && (e.X < this._CloseImageLocation.X + this._CloseImageSize.Width)
                && (e.Y > this._CloseImageLocation.Y)
                && (e.Y < this._CloseImageLocation.Y + this._CloseImageSize.Height)
                && this._CloseClickable == true)
            {
                if (this._IsMouseOverClose == false)
                {
                    this._IsMouseOverClose = true;
                    this._IsMouseOverTitle = false;
                    this._IsMouseOverContent = false;
                    this.Cursor = System.Windows.Forms.Cursors.Hand;
                    //isRefresh = true;
                }
            }
            else if (this._RealContentRectangle.Contains(new Point(e.X, e.Y)) && this._ContentClickable == true)
            {
                if (this._IsMouseOverContent == false)
                {
                    this._IsMouseOverClose = false;
                    this._IsMouseOverTitle = false;
                    this._IsMouseOverContent = true;
                    this.Cursor = System.Windows.Forms.Cursors.Hand;
                    isRefresh = true;
                }
            }
            else if (this._RealTitleRectangle.Contains(new Point(e.X, e.Y)) && this._TitleClickable == true)
            {
                if (this._IsMouseOverTitle == false)
                {
                    this._IsMouseOverClose = false;
                    this._IsMouseOverTitle = true;
                    this._IsMouseOverContent = false;
                    this.Cursor = System.Windows.Forms.Cursors.Hand;
                    isRefresh = true;
                }
            }
            else
            {
                if (this._IsMouseOverClose == true || this._IsMouseOverTitle == true || this._IsMouseOverContent == true)
                {
                    isRefresh = true;
                }
                this._IsMouseOverClose = false;
                this._IsMouseOverTitle = false;
                this._IsMouseOverContent = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            if (isRefresh == true)
            {
                this.Refresh();
            }

        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this._IsMouseDown = true;
            if (this._IsMouseOverClose == true)
            {
                this.Refresh();
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this._IsMouseDown = false;
            if (this._IsMouseOverClose == true)
            {
                this.Hide();
                if (CloseClick != null)
                {
                    CloseClick(this, new EventArgs());
                }
            }
            else if (this._IsMouseOverTitle == true)
            {
                if (TitleClick != null)
                {
                    TitleClick(this, new EventArgs());
                }
            }
            else if (this._IsMouseOverContent == true)
            {
                if (ContentClick != null)
                {
                    ContentClick(this, new EventArgs());
                }
            }


        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;
            grfx.PageUnit = GraphicsUnit.Pixel;

            Graphics offScreenGrfx;
            Bitmap offScreenImage;
            offScreenImage = new Bitmap(this._BackgroundImage.Width, this._BackgroundImage.Height);
            offScreenGrfx = Graphics.FromImage(offScreenImage);

            if (this._BackgroundImage != null)
            {
                offScreenGrfx.DrawImage(this._BackgroundImage, 0, 0, this._BackgroundImage.Width, this._BackgroundImage.Height);
            }
            this.DrawCloseButton(offScreenGrfx);
            this.DrawText(offScreenGrfx);
            grfx.DrawImage(offScreenImage, 0, 0);
        }

        #endregion --- 重写父类事件 End ---

        #endregion --- 触发事件 End ---
    }
}
