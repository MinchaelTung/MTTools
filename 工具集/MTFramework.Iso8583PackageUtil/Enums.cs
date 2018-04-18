
namespace MTFramework.Iso8583PackageUtil
{
    /// <summary>
    /// 传输模式
    /// </summary>
    public enum TransportMode
    {
        // 二进制传输
        BINARY,

        // 靠左右补' '
        LEFT_ASCII,

        // 左靠右补0 bcd码
        LEFT_BCD,

        // 靠右左补' '
        RIGHT_ASCII,

        // 右靠左补0 bcd码
        RIGHT_BCD
    }
    /// <summary>
    /// 消息文件类型
    /// </summary>
    public enum MessageFieldType
    {
        ALPHA,

        AMOUNT,
        /// <summary>
        /// 二进制
        /// </summary>
        BINARY,

        DATE,

        DATE_EXP,

        DATE10,

        DATE4,
        /// <summary>
        /// 可变长域的长度值（三位），占用3个字符。
        /// </summary>
        LLLVAR,
        /// <summary>
        /// 可变长域的长度值（二位），占用2个字符。
        /// </summary>
        LLVAR,

        NUMERIC,

        TIME
    }

}
