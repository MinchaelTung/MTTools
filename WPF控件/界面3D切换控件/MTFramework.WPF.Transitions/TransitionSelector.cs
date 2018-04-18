using System.Windows;

namespace MTFramework.WPF.Transitions
{
    public class TransitionSelector : DependencyObject
    {
        public virtual Transition SelectTransition(object oldContent, object newContent)
        {
            return null;
        }
    }
}
