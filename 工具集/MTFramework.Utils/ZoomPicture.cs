using System;

namespace MTFramework.Utils
{
    /// <summary>
    /// 图片大小规格变换工具
    /// </summary>
    public sealed class ZoomPicture
    {
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="srcPicPath">来源图片文件完整路径</param>
        /// <param name="tagPicPath">目标图片文件完整路径</param>
        /// <param name="width">目标图片宽度</param>
        /// <param name="height">目标图片高度</param>
        public static void SmallPicture(string srcPicPath, string tagPicPath, int width, int height)
        {
            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(srcPicPath);
                objNewPic = new System.Drawing.Bitmap(objPic, width, height);
                objNewPic.Save(tagPicPath);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="srcPicPath">来源图片文件完整路径</param>
        /// <param name="tagPicPath">目标图片文件完整路径</param>
        /// <param name="proportion">缩小比例0～100</param>
        public static void SmallPicture(string srcPicPath, string tagPicPath, int proportion)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(srcPicPath);
                int width = objPic.Width * proportion / 100;
                int intHeight = (width / objPic.Width) * objPic.Height;
                objNewPic = new System.Drawing.Bitmap(objPic, width, intHeight);
                objNewPic.Save(tagPicPath);

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        /// <summary>
        /// 按宽度比例缩放图片，自动计算高度
        /// </summary>
        /// <param name="srcPicPath">来源图片文件完整路径</param>
        /// <param name="tagPicPath">目标图片文件完整路径</param>
        /// <param name="intWidth">目标图片宽度</param>
        public static void SmallPictureForWidth(string srcPicPath, string tagPicPath, int intWidth)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(srcPicPath);
                int intHeight = (intWidth / objPic.Width) * objPic.Height;
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(srcPicPath);

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        /// <summary>
        /// 按高度比例缩放图片，自动计算宽度
        /// </summary>
        /// <param name="srcPicPath">来源图片文件完整路径</param>
        /// <param name="tagPicPath">目标图片文件完整路径</param>
        /// <param name="intHeight">目标图片高度</param>
        public static void SmallPictureForHeight(string srcPicPath, string tagPicPath, int intHeight)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(srcPicPath);
                int intWidth = (intHeight / objPic.Height) * objPic.Width;
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(tagPicPath);

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

    }
}
