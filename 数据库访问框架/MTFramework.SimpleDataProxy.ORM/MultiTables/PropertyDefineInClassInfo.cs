using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.MultiTables
{
    /// <summary>
    /// PropertyDefineInClassInfo
    /// </summary>
    [Attribute_Class(typeof(MultiTables_PropertyDefineInClassAttribute), typeof(FieldInfo), RestrictPropertyName = "MultiTablesName")]
    internal class PropertyDefineInClassInfo : FieldInfo
    {
    }
}
