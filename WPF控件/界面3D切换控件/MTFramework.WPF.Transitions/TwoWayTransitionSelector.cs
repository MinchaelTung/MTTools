using System.Windows;

namespace MTFramework.WPF.Transitions
{
    /// <summary>
    /// 双向切换选择容器
    /// </summary>
    public class TwoWayTransitionSelector : TransitionSelector
    {
        public TwoWayTransitionSelector() { }

        public Transition Forward
        {
            get { return (Transition)GetValue(ForwardProperty); }
            set { SetValue(ForwardProperty, value); }
        }

        public static readonly DependencyProperty ForwardProperty = DependencyProperty.Register("Forward", typeof(Transition), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));

        public Transition Backward
        {
            get { return (Transition)GetValue(BackwardProperty); }
            set { SetValue(BackwardProperty, value); }
        }

        public static readonly DependencyProperty BackwardProperty = DependencyProperty.Register("Backward", typeof(Transition), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));


        public TransitionDirection Direction
        {
            get { return (TransitionDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(TransitionDirection), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(TransitionDirection.Forward));


        public override Transition SelectTransition(object oldContent, object newContent)
        {
            return Direction == TransitionDirection.Forward ? Forward : Backward;
        }
    }
    public enum TransitionDirection
    {
        Forward,
        Backward,
    }
}
