using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// IDInfo
    /// </summary>
    [Attribute_Class(typeof(MultiTables_IDAttribute), typeof(FieldInfo), RestrictPropertyName = "MultiTablesName")]
    internal class IDInfo : FieldInfo
    {
    }
}
