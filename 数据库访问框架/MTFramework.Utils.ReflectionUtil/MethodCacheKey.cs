using System;
using System.Text;

namespace MTFramework.Utils.ReflectionUtil
{
    internal class MethodCacheKey
    {
        private string _hashKey;
        private string methodName = string.Empty;
        public Type[] paramTypes;
        private string typeName = string.Empty;

        public MethodCacheKey(string typeName, string methodName, Type[] paramTypes)
        {
            this.typeName = typeName;
            this.methodName = methodName;
            this.paramTypes = paramTypes;
            StringBuilder builder = new StringBuilder();
            builder.Append(typeName);
            builder.Append("|");
            builder.Append(methodName);
            foreach (Type type in paramTypes)
            {
                builder.Append("@");
                builder.Append(type.Name);
            }
            this._hashKey = builder.ToString();
        }

        private bool ArrayEquals(Type[] a1, Type[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            MethodCacheKey key = obj as MethodCacheKey;
            return (((key != null) && (key.TypeName == this.TypeName)) && ((key.MethodName == this.MethodName) && this.ArrayEquals(key.ParamTypes, this.ParamTypes)));
        }

        public override int GetHashCode()
        {
            return this._hashKey.GetHashCode();
        }

        public string MethodName
        {
            get
            {
                return this.methodName;
            }
        }

        public Type[] ParamTypes
        {
            get
            {
                return this.paramTypes;
            }
        }

        public string TypeName
        {
            get
            {
                return this.typeName;
            }
        }
    }

}
