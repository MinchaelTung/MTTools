using System.Collections.Generic;

namespace MTFramework.Iso8583PackageUtil
{
    /// <summary>
    /// 描述: 报文协议, 定义报文的域接口
    /// </summary>
    public interface BaseMessageProtocol
    {
        /// <summary>
        /// 获取报文头长度
        /// </summary>
        int HeaderLength { get; }

        /// <summary>
        /// 获取报文类型标识长度
        /// </summary>
        int MessageTypeIdLength { get; }

        /// <summary>
        /// 获取报文位图长度
        /// </summary>
        int BitmapLength { get; }

        /// <summary>
        /// 获取指定域的定义
        /// </summary>
        /// <param name="field">域位置</param>
        /// <returns></returns>
        MessageFieldDefinition GetFieldDefinition(int field);

        /// <summary>
        /// 获取最大报文域数
        /// </summary>
        int MaxFieldCount { get; }

        /// <summary>
        /// 说明报文是否是二进制编码
        /// </summary>
        bool IsBinary { get; }

        /// <summary>
        /// 返回协议名称英文简写
        /// </summary>
        string Name { get; }

    }
}
