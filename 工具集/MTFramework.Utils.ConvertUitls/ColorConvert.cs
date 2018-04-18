using System;
using System.Drawing;
using System.Text;

namespace MTFramework.Utils.ConvertUitls
{
    /// <summary>
    /// 颜色文字值 和 颜色对象 互换工具
    /// </summary>
    public sealed class ColorConvert
    {
        /// <summary>
        /// 颜色字符串值转换为Color类型 
        /// </summary>
        /// <param name="color">R,G,B的字符串或者A,R,G,B的字符串参数，如255,255,255,255</param>
        /// <param name="separator">分隔符数组,如"," </param>
        /// <returns>Color</returns>
        public static Color StringConvertToColor(string color, char separator)
        {
            int red;
            int green;
            int blue = 0;
            string[] textArray = color.Split(separator);
            switch (textArray.Length)
            {
                case 3:
                    {
                        red = Convert.ToByte(textArray[0].Trim());
                        green = Convert.ToByte(textArray[1].Trim());
                        blue = Convert.ToByte(textArray[2].Trim());
                        return Color.FromArgb(red, green, blue);
                    }
                case 4:
                    {
                        red = Convert.ToByte(textArray[1].Trim());
                        green = Convert.ToByte(textArray[2].Trim());
                        blue = Convert.ToByte(textArray[3].Trim());
                        return Color.FromArgb(Convert.ToByte(textArray[0].Trim()), red, green, blue);
                    }
                default:
                    {
                        return Color.Empty;
                    }
            }
        }

        /// <summary>
        /// 颜色对象转换为颜色字符串
        /// </summary>
        /// <param name="color">颜色对象</param>
        /// <param name="separator">分隔符</param>
        /// <returns>返回颜色对象的颜色字符串</returns>
        public static string ColorConvertToString(Color color, char separator)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(color.A).Append(separator).Append(color.R).Append(separator).Append(color.G).Append(separator).Append(color.B);
            return sb.ToString();
        }
    }
}
