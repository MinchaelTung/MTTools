using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// IDInfo
    /// </summary>
    [Attribute_Class(typeof(CombinedTable_IDAttribute), typeof(FieldInfo))]
    internal class IDInfo : FieldInfo
    {
    }
}
