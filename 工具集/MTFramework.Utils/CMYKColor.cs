using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MTFramework.Utils
{
    /// <summary>
    /// CMYK列印颜色
    /// </summary>
    public sealed class CMYKColor
    {
        #region --- 成员 Begin ---

        [CompilerGenerated]
        private double _C;
        /// <summary>
        /// C值
        /// </summary>
        public double C
        {
            [CompilerGenerated]
            get { return _C; }
            [CompilerGenerated]
            set { _C = value; }
        }
        [CompilerGenerated]
        private double _M;
        /// <summary>
        /// M值
        /// </summary>
        public double M
        {
            [CompilerGenerated]
            get { return _M; }
            [CompilerGenerated]
            set { _M = value; }
        }
        [CompilerGenerated]
        private double _Y;
        /// <summary>
        /// Y值
        /// </summary>
        public double Y
        {
            [CompilerGenerated]
            get { return _Y; }
            [CompilerGenerated]
            set { _Y = value; }
        }
        [CompilerGenerated]
        private double _K;
        /// <summary>
        /// K值
        /// </summary>
        public double K
        {
            [CompilerGenerated]
            get { return _K; }
            [CompilerGenerated]
            set { _K = value; }
        }

        #endregion --- 成员 End ---

        #region --- 构造方法 Begin ---

        /// <summary>
        /// CMYKColor默认构造方法
        /// </summary>
        public CMYKColor()
        {
            this._C = 0.0;
            this._M = 0.0;
            this._Y = 0.0;
            this._K = 1.0;
        }

        /// <summary>
        /// CMYKColor使用Int值模式赋值
        /// </summary>
        /// <param name="c">C 取值范围0～100</param>
        /// <param name="m">M 取值范围0～100</param>
        /// <param name="y">Y 取值范围0～100</param>
        /// <param name="k">K 取值范围0～100</param>
        public CMYKColor(int c, int m, int y, int k)
        {
            if (c >= 0 && c <= 100)
            {
                this._C = c / 100.0;
            }
            else
            {
                this._C = 0.0;
            }
            if (m >= 0 && m <= 100)
            {
                this._M = m / 100.0;
            }
            else
            {
                this._M = 0.0;
            }
            if (y >= 0 && y <= 100)
            {
                this._Y = y / 100.0;
            }
            else
            {
                this._Y = 0.0;
            }
            if (k >= 0 && k <= 100)
            {
                this._K = k / 100.0;
            }
            else
            {
                this._K = 1.0;
            }
        }

        /// <summary>
        /// CMYKColor使用Double值模式赋值
        /// </summary>
        /// <param name="c">C 取值范围1～0.00</param>
        /// <param name="m">M 取值范围1～0.00</param>
        /// <param name="y">Y 取值范围1～0.00</param>
        /// <param name="k">K 取值范围1～0.00</param>
        public CMYKColor(double c, double m, double y, double k)
        {
            this._C = c;
            this._M = m;
            this._Y = y;
            this._K = k;
        }

        /// <summary>
        /// CMYKColor使用System.Drawing.Color赋值
        /// </summary>
        /// <param name="color">颜色类型</param>
        public CMYKColor(Color color)
        {
            this.ColorToCMYKColor(color);
        }

        #endregion --- 构造方法 End ---

        #region --- 转换方法 Begin ---

        /// <summary>
        /// 从颜色类型转换为CMYKColor
        /// </summary>
        /// <param name="color">需要转换的颜色</param>
        public void ColorToCMYKColor(Color color)
        {
            double numR = ((double)(0xff - color.R)) / 255;
            double numG = ((double)(0xff - color.G)) / 255;
            double numB = ((double)(0xff - color.B)) / 255;
            double numContrast = Math.Min(numR, Math.Min(numG, numB));
            if (numContrast == 1)
            {
                this._C = 0.0;
                this._M = 0.0;
                this._Y = 0.0;
                this._K = 1.0;
            }
            else
            {
                this._C = (numR - numContrast) / (1 - numContrast);
                this._M = (numG - numContrast) / (1 - numContrast);
                this._Y = (numB - numContrast) / (1 - numContrast);
                this._K = numContrast;
            }
        }

        /// <summary>
        /// 从CMYK转换为Color
        /// </summary>
        /// <returns></returns>
        public Color CMYKColorToColor()
        {
            return Color.FromArgb(Convert.ToInt32((double)(((1 - this._C) * (1 - this._K)) * 255)), Convert.ToInt32((double)(((1 - this._M) * (1 - this._K)) * 255)), Convert.ToInt32((double)(((1 - this._Y) * (1 - this._K)) * 255)));
        }

        /// <summary>
        /// 获取CMYK的百分比
        /// </summary>
        /// <param name="value">C or M or Y or K</param>
        /// <param name="digits">小数后多少位</param>
        /// <returns></returns>
        public double GetPercent(double value, int digits)
        {
            return Math.Round((double)(value * 100), digits);
        }

        #endregion --- 转换方法 End ---

        /// <summary>
        /// 显示CMYK各值的百分比
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int digits = 2;
            return string.Format("C={0}%; M={1}%; Y={2}%; K={3}%", this.GetPercent(this._C, digits), this.GetPercent(this._M, digits), this.GetPercent(this._Y, digits), this.GetPercent(this._K, digits));
        }
    }
}
