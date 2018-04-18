using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTFramework.SimpleDataProxy.ConfigManagement
{
    /// <summary>
    /// 数据库连接数据信息
    /// </summary>
    [Serializable]
    public class DBConfigEntity
    {

        private string _ConnectionString;
        /// <summary>
        /// 连接
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }


        private string _AssemblyName;
        /// <summary>
        ///数据库文件
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return this._AssemblyName;
            }
            set
            {
                this._AssemblyName = value;
            }
        }


        private string _ClassTypeName;
        /// <summary>
        /// 实现类全名
        /// </summary>
        public string ClassTypeName
        {
            get
            {
                return this._ClassTypeName;
            }
            set
            {
                this._ClassTypeName = value;
            }
        }

        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static dynamic LoadFile(string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();
                    object obj = formatter.Deserialize(file);
                    if (obj is DBConfigEntity)
                    {
                        return obj as DBConfigEntity;
                    }
                    else if (obj is List<DBConfigEntity>)
                    {
                        return obj as List<DBConfigEntity>;
                    }
                    else if (obj is DBConfigEntity[])
                    {
                        return obj as DBConfigEntity[];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="dbConfigEntity"></param>
        /// <param name="filePath"></param>
        public static void SaveToFile(DBConfigEntity dbConfigEntity, string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, dbConfigEntity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="dbConfigEntity"></param>
        /// <param name="filePath"></param>
        public static void SaveToFile(List<DBConfigEntity> dbConfigEntityList, string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, dbConfigEntityList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="dbConfigEntity"></param>
        /// <param name="filePath"></param>
        public static void SaveToFile(DBConfigEntity[] dbConfigEntityArray, string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, dbConfigEntityArray);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
