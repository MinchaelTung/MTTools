using System.Drawing;
using System.IO;

namespace MTFramework.Utils.ConvertUitls
{
    /// <summary>
    /// 图片对象转换工具
    /// </summary>
    public sealed class ImageConvert
    {
        /// <summary>
        /// byte数组转换为Image
        /// </summary>
        /// <param name="inputBytes">传入byte数组参数</param>
        /// <returns>Image</returns>
        public static Image ByteArrayConvertToImage(byte[] inputBytes)
        {
            MemoryStream ms = new MemoryStream(inputBytes);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// 图片转换为byte数组
        /// </summary>
        /// <param name="image">传入图片对象参数</param>
        /// <returns>byte[]</returns>
        public static byte[] ImageConvertToByteArray(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// 字符串转换为图片
        /// </summary>
        /// <param name="inputString">传入字符串参数</param>
        /// <returns>Image</returns>
        public static Image StringConvertToImage(string inputString)
        {
            byte[] bytesArray = StringConvert.StringConvertToBytes(inputString);
            return ImageConvert.ByteArrayConvertToImage(bytesArray);
        }

        /// <summary>
        /// 图片转换为字符串
        /// </summary>
        /// <param name="image">传入图片参数</param>
        /// <returns>string</returns>
        public static string ImageConvertToString(Image image)
        {
            byte[] bytesArray = ImageConvert.ImageConvertToByteArray(image);
            return StringConvert.BytesConvertToString(bytesArray);
        }
    }
}
