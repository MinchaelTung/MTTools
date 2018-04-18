using System;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 读取标记信息工具
    /// </summary>
    public static class LoadAttributeUtil
    {
        /// <summary>
        /// 读取对象标记信息
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>ObjectInfo</returns>
        public static ObjectInfo GetObjectInfo(Type type)
        {
            ObjectInfo info = (ObjectInfo)AttributeLoader.LoadData(type, typeof(ObjectInfo));
            info.Type = type;
            foreach (ExtParameterInfo info2 in info.ExtParameterInfoList)
            {
                foreach (AccessInfo info3 in info.AccessInfoList)
                {
                    if (info3.Name == info2.AccessName)
                    {
                        info3.ExtParameters.Add(info2);
                    }
                }
            }
            return info;
        }
    }
}
