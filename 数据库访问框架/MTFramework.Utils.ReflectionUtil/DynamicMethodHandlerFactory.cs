using System;
using System.Reflection.Emit;
using System.Reflection;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 异步方法处理工厂
    /// </summary>
    internal static class DynamicMethodHandlerFactory
    {
        /// <summary>
        /// 创建构造函数代理
        /// </summary>
        /// <param name="constructor">构造函数</param>
        /// <returns>返回核心代理</returns>
        public static DynamicCtorDelegate CreateConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException("参数:'constructor'不能为 Null");
            }
            if (constructor.GetParameters().Length > 0)
            {
                throw new NotSupportedException("不支持带参数的构造函数");
            }
            DynamicMethod method = new DynamicMethod("ctor", constructor.DeclaringType, Type.EmptyTypes, true);
            ILGenerator iLGenerator = method.GetILGenerator();
            iLGenerator.Emit(OpCodes.Nop);
            iLGenerator.Emit(OpCodes.Newobj, constructor);
            iLGenerator.Emit(OpCodes.Ret);
            return (DynamicCtorDelegate)method.CreateDelegate(typeof(DynamicCtorDelegate));
        }

        /// <summary>
        /// 创建获取成员代理
        /// </summary>
        /// <param name="field">成员信息</param>
        /// <returns>返回获取成员代理</returns>
        public static DynamicMemberGetDelegate CreateFieldGetter(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("参数:'field'不能为 Null");
            }
            DynamicMethod method = new DynamicMethod("fldg", typeof(object), new Type[] { typeof(object) }, field.DeclaringType, true);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!field.IsStatic)
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                EmitCastToReference(iLGenerator, field.DeclaringType);
                iLGenerator.Emit(OpCodes.Ldfld, field);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Ldsfld, field);
            }
            if (field.FieldType.IsValueType)
            {
                iLGenerator.Emit(OpCodes.Box, field.FieldType);
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (DynamicMemberGetDelegate)method.CreateDelegate(typeof(DynamicMemberGetDelegate));
        }

        /// <summary>
        /// 创建设置成员代理
        /// </summary>
        /// <param name="field">成员信息</param>
        /// <returns>返回设置成员代理</returns>
        public static DynamicMemberSetDelegate CreateFieldSetter(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("参数:'field'不能为 Null");
            }
            DynamicMethod method = new DynamicMethod("flds", null, new Type[] { typeof(object), typeof(object) }, field.DeclaringType, true);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!field.IsStatic)
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
            }
            iLGenerator.Emit(OpCodes.Ldarg_1);
            EmitCastToReference(iLGenerator, field.FieldType);
            if (!field.IsStatic)
            {
                iLGenerator.Emit(OpCodes.Stfld, field);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Stsfld, field);
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (DynamicMemberSetDelegate)method.CreateDelegate(typeof(DynamicMemberSetDelegate));
        }

        /// <summary>
        /// 创建方法代理
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <returns>返回方法代理</returns>
        public static DynamicMethodDelegate CreateMethod(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            DynamicMethod method2 = new DynamicMethod("dm", typeof(object), new Type[] { typeof(object), typeof(object[]) }, typeof(DynamicMethodHandlerFactory), true);
            ILGenerator iLGenerator = method2.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameters.Length; i++)
            {
                iLGenerator.Emit(OpCodes.Ldarg_1);
                iLGenerator.Emit(OpCodes.Ldc_I4, i);
                Type parameterType = parameters[i].ParameterType;
                if (parameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.IsValueType)
                    {
                        iLGenerator.Emit(OpCodes.Ldelem_Ref);
                        iLGenerator.Emit(OpCodes.Unbox, parameterType);
                    }
                    else
                    {
                        iLGenerator.Emit(OpCodes.Ldelema, parameterType);
                    }
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Ldelem_Ref);
                    if (parameterType.IsValueType)
                    {
                        iLGenerator.Emit(OpCodes.Unbox, parameterType);
                        iLGenerator.Emit(OpCodes.Ldobj, parameterType);
                    }
                }
            }
            if ((method.IsAbstract || method.IsVirtual) && (!method.IsFinal && !method.DeclaringType.IsSealed))
            {
                iLGenerator.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Call, method);
            }
            if (method.ReturnType == typeof(void))
            {
                iLGenerator.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                iLGenerator.Emit(OpCodes.Box, method.ReturnType);
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (DynamicMethodDelegate)method2.CreateDelegate(typeof(DynamicMethodDelegate));
        }
        /// <summary>
        /// 创建获取成员代理
        /// </summary>
        /// <param name="property">成员信息</param>
        /// <returns>返回获取成员代理</returns>
        public static DynamicMemberGetDelegate CreatePropertyGetter(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("参数:'property'不能为 Null");
            }
            if (!property.CanRead)
            {
                return null;
            }
            MethodInfo getMethod = property.GetGetMethod();
            if (getMethod == null)
            {
                getMethod = property.GetGetMethod(true);
            }
            DynamicMethod method = new DynamicMethod("propg", typeof(object), new Type[] { typeof(object) }, property.DeclaringType, true);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!getMethod.IsStatic)
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
            {
                iLGenerator.EmitCall(OpCodes.Call, getMethod, null);
            }
            if (property.PropertyType.IsValueType)
            {
                iLGenerator.Emit(OpCodes.Box, property.PropertyType);
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (DynamicMemberGetDelegate)method.CreateDelegate(typeof(DynamicMemberGetDelegate));
        }

        /// <summary>
        /// 创建设置成员代理
        /// </summary>
        /// <param name="property">成员信息</param>
        /// <returns>返回设置成员代理</returns>
        public static DynamicMemberSetDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("参数:'property'不能为 Null");
            }
            if (!property.CanWrite)
            {
                return null;
            }
            MethodInfo setMethod = property.GetSetMethod();
            if (setMethod == null)
            {
                setMethod = property.GetSetMethod(true);
            }
            DynamicMethod method = new DynamicMethod("props", null, new Type[] { typeof(object), typeof(object) }, property.DeclaringType, true);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!setMethod.IsStatic)
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
            }
            iLGenerator.Emit(OpCodes.Ldarg_1);
            EmitCastToReference(iLGenerator, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                iLGenerator.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
            {
                iLGenerator.EmitCall(OpCodes.Call, setMethod, null);
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (DynamicMemberSetDelegate)method.CreateDelegate(typeof(DynamicMemberSetDelegate));
        }

        /// <summary>
        /// 反射类型标记
        /// </summary>
        /// <param name="il">生成 Microsoft 中间语言 (MSIL) 指令。</param>
        /// <param name="type">类型</param>
        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }
    }
}
