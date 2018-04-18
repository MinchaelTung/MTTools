using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTFramework.Iso8583PackageUtil
{
    /// <summary>
    /// 基础信息类接口
    /// </summary>
    public interface IBaseMessage
    {
        /// <summary>
        /// 报文类型
        /// </summary>
        string TypeID { get; set; }

        /// <summary>
        /// 报文头
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// 位图信息
        /// </summary>
        BitArray Bitmap { get; }

        /// <summary>
        /// 获取报文协议
        /// </summary>
        BaseMessageProtocol Protocol { get; }

        /// <summary>
        /// 根据序号获取对应的域
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        MessageField GetField(int index);

        /// <summary>
        /// 根据序号获取对应的域
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        MessageField this[int index] { get; }

        /// <summary>
        /// 检查序号的域数据是否存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true = 该域数据不存在,false=该域数据存在</returns>
        bool IsDataNullMessageField(int index);

        /// <summary>
        /// 设置指定序号的域
        /// </summary>
        /// <param name="index"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        void SetField(int index, MessageField field);

        /// <summary>
        /// 将报文内容写入缓存, 不包括报文长度
        /// </summary>
        /// <returns></returns>
        byte[] GetContent();

        /// <summary>
        /// 将字节数组转换为字符串报文头
        /// </summary>
        /// <param name="bytes"></param>
        void SetHeader(byte[] bytes);

        /// <summary>
        /// 获取该报文的MAC值
        /// </summary>
        /// <returns></returns>
        string GetMac();

    }
}
