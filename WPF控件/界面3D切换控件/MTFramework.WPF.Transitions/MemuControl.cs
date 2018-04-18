using System.Windows;

namespace MTFramework.WPF.Transitions
{
    /// <summary>
    /// 菜单容器
    /// </summary>
    public class MemuControl : DependencyObject
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MemuControl));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get { return (string)GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }

        /// <summary>
        /// 引用文件
        /// </summary>
        public string AssemblyFile { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
