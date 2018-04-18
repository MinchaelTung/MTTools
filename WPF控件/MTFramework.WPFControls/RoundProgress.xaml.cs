using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MTFramework.WPFControls
{
    /// <summary>
    /// RoundProgress.xaml 的交互逻辑
    /// </summary>
    public partial class RoundProgress : UserControl
    {
        public RoundProgress()
        {
            InitializeComponent();
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                       typeof(Timeline),
                           new FrameworkPropertyMetadata { DefaultValue = 20 });
        }
    }
}
