using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MTFramework.Utils.ReflectionUtil.Properties;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// MethodCaller
    /// </summary>
    public static class MethodCaller
    {
        private static Dictionary<Type, DynamicCtorDelegate> _ctorCache = new Dictionary<Type, DynamicCtorDelegate>();
        private static readonly Dictionary<MethodCacheKey, DynamicMemberHandle> _memberCache = new Dictionary<MethodCacheKey, DynamicMemberHandle>();
        private static Dictionary<MethodCacheKey, DynamicMethodHandle> _methodCache = new Dictionary<MethodCacheKey, DynamicMethodHandle>();
        private const BindingFlags allLevelFlags = (BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        private const BindingFlags ctorFlags = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        private const BindingFlags fieldFlags = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        private const BindingFlags oneLevelFlags = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private const BindingFlags propertyFlags = (BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

        [DebuggerHidden, DebuggerStepThrough]
        private static object CallMethod(object obj, DynamicMethodHandle methodHandle, params object[] parameters)
        {
            object obj2 = null;
            DynamicMethodDelegate dynamicMethod = methodHandle.DynamicMethod;
            object[] args = null;
            if (parameters == null)
            {
                args = new object[1];
            }
            else
            {
                args = new object[parameters.Length];
                for (int i = 0; i < methodHandle.ParameterInfoList.Length; i++)
                {
                    if (methodHandle.ParameterInfoList[i].ParameterType == parameters[i].GetType())
                    {
                        args[i] = parameters[i];
                    }
                    else if (methodHandle.ParameterInfoList[i].ParameterType.IsAssignableFrom(parameters[i].GetType()))
                    {
                        args[i] = parameters[i];
                    }
                    else if (ReflectionUtilities.IsImplementFromInterface(methodHandle.ParameterInfoList[i].ParameterType, typeof(IConvertible)))
                    {
                        args[i] = Convert.ChangeType(parameters[i], methodHandle.ParameterInfoList[i].ParameterType);
                    }
                }
            }
            if (methodHandle.HasFinalArrayParam)
            {
                int methodParamsLength = methodHandle.MethodParamsLength;
                int length = args.Length - (methodParamsLength - 1);
                object[] objArray2 = (object[])Array.CreateInstance(typeof(object), length);
                for (int j = 0; j < length; j++)
                {
                    objArray2[j] = args[j];
                }
                object[] objArray3 = new object[methodParamsLength];
                for (int k = 0; k <= (methodParamsLength - 2); k++)
                {
                    objArray3[k] = parameters[k];
                }
                objArray3[objArray3.Length - 1] = objArray2;
                args = objArray3;
            }
            try
            {
                obj2 = methodHandle.DynamicMethod(obj, args);
            }
            catch (System.Exception exception)
            {
                throw new ReflectionException(methodHandle.MethodName + " " + Resources.MethodCallFailed, exception);
            }
            return obj2;
        }

        /// <summary>
        /// CallMethod
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="methodInfo">methodInfo</param>
        /// <param name="parameters">parameters</param>
        /// <returns>object</returns>
        public static object CallMethod(object obj, MethodInfo methodInfo, params object[] parameters)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (methodInfo == null)
            {
                throw new ArgumentException("参数:'methodInfo'不能为 Null");
            }
            DynamicMethodHandle methodHandle = GetCachedMethod(obj, methodInfo, parameters);
            if ((methodHandle == null) || (methodHandle.DynamicMethod == null))
            {
                throw new ReflectionException(string.Format("Type:{0}没有找到{1}方法", obj.GetType(), methodInfo.Name));
            }
            return CallMethod(obj, methodHandle, parameters);
        }
        /// <summary>
        /// CallMethod
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="method">method</param>
        /// <param name="parameters">parameters</param>
        /// <returns>object</returns>
        public static object CallMethod(object obj, string method, params object[] parameters)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (method == null)
            {
                throw new ArgumentException("参数:'method'不能为 Null");
            }
            DynamicMethodHandle methodHandle = GetCachedMethod(obj, method, parameters);
            if ((methodHandle == null) || (methodHandle.DynamicMethod == null))
            {
                throw new ReflectionException(string.Format("Type:{0}没有找到{1}方法", obj.GetType(), method));
            }
            return CallMethod(obj, methodHandle, parameters);
        }
        /// <summary>
        /// CallMethodIfImplemented
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="method">method</param>
        /// <param name="parameters">parameters</param>
        /// <returns>object</returns>
        public static object CallMethodIfImplemented(object obj, string method, params object[] parameters)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数:'obj'不能为 Null");
            }
            if (method == null)
            {
                throw new ArgumentException("参数:'method'不能为 Null");
            }
            DynamicMethodHandle methodHandle = GetCachedMethod(obj, method, parameters);
            if ((methodHandle != null) && (methodHandle.DynamicMethod != null))
            {
                return CallMethod(obj, methodHandle, parameters);
            }
            return null;
        }
        /// <summary>
        /// CallPropertyGetter
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="property">property</param>
        /// <returns>object</returns>
        public static object CallPropertyGetter(object obj, string property)
        {
            return GetCachedProperty(obj.GetType(), property).DynamicMemberGet(obj);
        }
        /// <summary>
        /// CallPropertySetter
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="property">property</param>
        /// <param name="value">value</param>
        public static void CallPropertySetter(object obj, string property, object value)
        {
            DynamicMemberHandle cachedProperty = GetCachedProperty(obj.GetType(), property);
            object obj2 = value;
            if ((cachedProperty.MemberType != value.GetType()) && ReflectionUtilities.IsImplementFromInterface(cachedProperty.MemberType, typeof(IConvertible)))
            {
                obj2 = Convert.ChangeType(value, cachedProperty.MemberType);
            }
            cachedProperty.DynamicMemberSet(obj, obj2);
        }

        /// <summary>
        /// CreateInstance
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <returns>object</returns>
        public static object CreateInstance(Type objectType)
        {
            DynamicCtorDelegate cachedConstructor = GetCachedConstructor(objectType);
            if (cachedConstructor == null)
            {
                throw new ReflectionException(string.Format("Type:{0}没有找到无参数的构造方法", objectType));
            }
            return cachedConstructor();
        }
        /// <summary>
        /// FindMethod
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="method">method</param>
        /// <param name="types">types</param>
        /// <returns></returns>
        public static MethodInfo FindMethod(Type objectType, string method, Type[] types)
        {
            MethodInfo info = null;
            do
            {
                info = objectType.GetMethod(method, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, types, null);
                if (info != null)
                {
                    return info;
                }
                objectType = objectType.BaseType;
            }
            while (objectType != null);
            return info;
        }
        /// <summary>
        /// FindMethod
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="method">method</param>
        /// <param name="parameterCount">parameterCount</param>
        /// <returns>MethodInfo</returns>
        public static MethodInfo FindMethod(Type objectType, string method, int parameterCount)
        {
            Type baseType = objectType;
            do
            {
                MethodInfo info2 = baseType.GetMethod(method, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (info2 != null)
                {
                    ParameterInfo[] parameters = info2.GetParameters();
                    int length = parameters.Length;
                    if ((length > 0) && (((length == 1) && parameters[0].ParameterType.IsArray) || (parameters[length - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0)))
                    {
                        if (parameterCount >= (length - 1))
                        {
                            return info2;
                        }
                    }
                    else if (length == parameterCount)
                    {
                        return info2;
                    }
                }
                baseType = baseType.BaseType;
            }
            while (baseType != null);
            return null;
        }
        /// <summary>
        /// FindMethodUsingFuzzyMatching
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="method">method</param>
        /// <param name="parameters">parameters</param>
        /// <returns>MethodInfo</returns>
        private static MethodInfo FindMethodUsingFuzzyMatching(Type objectType, string method, object[] parameters)
        {
            MethodInfo info = null;
            MethodInfo[] infoArray;
            Type baseType = objectType;
        Label_0004:
            infoArray = baseType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            int length = parameters.Length;
            foreach (MethodInfo info2 in infoArray)
            {
                if (info2.Name == method)
                {
                    ParameterInfo[] infoArray2 = info2.GetParameters();
                    int num2 = infoArray2.Length;
                    if (((num2 > 0) && (((num2 == 1) && infoArray2[0].ParameterType.IsArray) || (infoArray2[num2 - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0))) && (length >= (num2 - 1)))
                    {
                        info = info2;
                        break;
                    }
                }
            }
            if (info == null)
            {
                foreach (MethodInfo info3 in infoArray)
                {
                    if ((info3.Name == method) && (info3.GetParameters().Length == length))
                    {
                        info = info3;
                        break;
                    }
                }
            }
            if (info == null)
            {
                baseType = baseType.BaseType;
                if (baseType != null)
                {
                    goto Label_0004;
                }
            }
            return info;
        }
        /// <summary>
        /// GetCachedConstructor
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <returns>DynamicCtorDelegate</returns>
        private static DynamicCtorDelegate GetCachedConstructor(Type objectType)
        {
            DynamicCtorDelegate delegate2 = null;
            if (!_ctorCache.TryGetValue(objectType, out delegate2))
            {
                lock (_ctorCache)
                {
                    if (!_ctorCache.TryGetValue(objectType, out delegate2))
                    {
                        delegate2 = DynamicMethodHandlerFactory.CreateConstructor(objectType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null));
                        _ctorCache.Add(objectType, delegate2);
                    }
                }
            }
            return delegate2;
        }
        /// <summary>
        /// GetCachedField
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="fieldName">fieldName</param>
        /// <returns>DynamicMemberHandle</returns>
        internal static DynamicMemberHandle GetCachedField(Type objectType, string fieldName)
        {
            MethodCacheKey key = new MethodCacheKey(objectType.FullName, fieldName, GetParameterTypes(null));
            DynamicMemberHandle handle = null;
            if (!_memberCache.TryGetValue(key, out handle))
            {
                lock (_memberCache)
                {
                    if (!_memberCache.TryGetValue(key, out handle))
                    {
                        handle = new DynamicMemberHandle(objectType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                        _memberCache.Add(key, handle);
                    }
                }
            }
            return handle;
        }
        /// <summary>
        /// GetCachedMethod
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="info">MethodInfo</param>
        /// <param name="parameters">parameters</param>
        /// <returns>DynamicMethodHandle</returns>
        private static DynamicMethodHandle GetCachedMethod(object obj, MethodInfo info, params object[] parameters)
        {
            MethodCacheKey key = new MethodCacheKey(obj.GetType().FullName, info.Name, GetParameterTypes(parameters));
            DynamicMethodHandle handle = null;
            if (!_methodCache.TryGetValue(key, out handle))
            {
                lock (_methodCache)
                {
                    if (!_methodCache.TryGetValue(key, out handle))
                    {
                        handle = new DynamicMethodHandle(info);
                        _methodCache.Add(key, handle);
                    }
                }
            }
            return handle;
        }
        /// <summary>
        /// GetCachedMethod
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="method">method</param>
        /// <param name="parameters">parameters</param>
        /// <returns>DynamicMethodHandle</returns>
        private static DynamicMethodHandle GetCachedMethod(object obj, string method, params object[] parameters)
        {
            MethodCacheKey key = new MethodCacheKey(obj.GetType().FullName, method, GetParameterTypes(parameters));
            DynamicMethodHandle handle = null;
            if (!_methodCache.TryGetValue(key, out handle))
            {
                lock (_methodCache)
                {
                    if (!_methodCache.TryGetValue(key, out handle))
                    {
                        handle = new DynamicMethodHandle(GetMethod(obj.GetType(), method, parameters));
                        _methodCache.Add(key, handle);
                    }
                }
            }
            return handle;
        }
        /// <summary>
        /// GetCachedProperty
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="propertyName">propertyName</param>
        /// <returns>DynamicMemberHandle</returns>
        internal static DynamicMemberHandle GetCachedProperty(Type objectType, string propertyName)
        {
            MethodCacheKey key = new MethodCacheKey(objectType.FullName, propertyName, GetParameterTypes(null));
            DynamicMemberHandle handle = null;
            if (!_memberCache.TryGetValue(key, out handle))
            {
                lock (_memberCache)
                {
                    if (!_memberCache.TryGetValue(key, out handle))
                    {
                        handle = new DynamicMemberHandle(objectType.GetProperty(propertyName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance));
                        _memberCache.Add(key, handle);
                    }
                }
            }
            return handle;
        }
        /// <summary>
        /// GetMethod
        /// </summary>
        /// <param name="objectType">objectType</param>
        /// <param name="method">method</param>
        /// <param name="parameters">parameters</param>
        /// <returns>MethodInfo</returns>
        public static MethodInfo GetMethod(Type objectType, string method, params object[] parameters)
        {
            MethodInfo info = null;
            object[] objArray = null;
            if (parameters == null)
            {
                objArray = new object[1];
            }
            else
            {
                objArray = parameters;
            }
            info = FindMethod(objectType, method, GetParameterTypes(objArray));
            if (info == null)
            {
                try
                {
                    info = FindMethod(objectType, method, objArray.Length);
                }
                catch (AmbiguousMatchException)
                {
                    info = FindMethodUsingFuzzyMatching(objectType, method, objArray);
                }
            }
            if (info == null)
            {
                info = objectType.GetMethod(method, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            return info;
        }
        /// <summary>
        /// GetParameterTypes
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>Type[]</returns>
        public static Type[] GetParameterTypes(object[] parameters)
        {
            List<Type> list = new List<Type>();
            if (parameters == null)
            {
                list.Add(typeof(object));
            }
            else
            {
                foreach (object obj2 in parameters)
                {
                    if (obj2 == null)
                    {
                        list.Add(typeof(object));
                    }
                    else
                    {
                        list.Add(obj2.GetType());
                    }
                }
            }
            return list.ToArray();
        }
    }
}
