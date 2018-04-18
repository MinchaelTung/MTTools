using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Samples;
using System.Windows.Media.Imaging;
using PDFTron.SilverDox.Samples.Resources;

namespace PDFTron.SilverDox.Samples
{
    /// <summary>
    /// Represents a drop-down button control that changes the current page layout
    /// </summary>
    public partial class LayoutControl : UserControl
    {

        private ReaderControl _readerControl;


        private string[] ImagePathArray;

        public LayoutControl(ReaderControl readerControl)
        {
            InitializeComponent();
            this._readerControl = readerControl;
            ImagePathArray = new string[6];
            ImagePathArray[0] = "/ReaderControl;component/Resources/page_cont.png";
            ImagePathArray[1] = "/ReaderControl;component/Resources/page_facing_cont.png";
            ImagePathArray[2] = "/ReaderControl;component/Resources/page_cover_facing_cont.png";
            ImagePathArray[3] = "/ReaderControl;component/Resources/page_single.png";            
            ImagePathArray[4] = "/ReaderControl;component/Resources/page_facing.png";
            ImagePathArray[5] = "/ReaderControl;component/Resources/page_cover_facing.png";
            _readerControl.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_readerControl_PropertyChanged);
            UpdateButtonIcon();
        }


        void _readerControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LayoutMode")
            {
                UpdateButtonIcon();
            }
        }

        private void UpdateButtonIcon()
        {
            int index = (int)_readerControl.LayoutMode;
            (this.LayoutIcon).Source = new BitmapImage(new Uri(ImagePathArray[index], UriKind.Relative));
        }


        private void LayoutButton_Click(object sender, RoutedEventArgs e)
        {

            GeneralTransform gt = ((Button)sender).TransformToVisual(Application.Current.RootVisual);
            Point offset = gt.Transform(new Point(0, 0));

            ContextMenu conMenu = new ContextMenu()
            {
                VerticalOffset = offset.Y + (sender as Button).ActualHeight,
                HorizontalOffset = offset.X,
            };

            MenuItem[] menuItemArray = new MenuItem[6];


            menuItemArray[0] = new MenuItem()
            {
                Icon = new Image() { Source = new BitmapImage(new Uri(ImagePathArray[0], UriKind.Relative)) },
                Header = StringResource.Continuous,
                Tag = ReaderControl.LayoutModes.Continuous,
            };
            
            menuItemArray[1] = new MenuItem()
            {
                Icon = new Image() { Source = new BitmapImage(new Uri(ImagePathArray[1], UriKind.Relative)) },
                
                Header = StringResource.FacingContinuous,
                Tag = ReaderControl.LayoutModes.FacingContinous,
            };
            menuItemArray[2] = new MenuItem()
            {
                Icon = new Image() { Source = new BitmapImage(new Uri(ImagePathArray[2], UriKind.Relative)) },
                Header = StringResource.CoverFacingContinuous,
                Tag = ReaderControl.LayoutModes.FacingCoverContinuous,
            };
            menuItemArray[3] = new MenuItem()
            {
                Icon = new Image() { Source = new BitmapImage(new Uri(ImagePathArray[3], UriKind.Relative)) },
                Header = StringResource.SinglePage,
                Tag = ReaderControl.LayoutModes.SinglePage,
            };
            menuItemArray[4] = new MenuItem()
            {
                Icon = new Image() { Source = new BitmapImage(new Uri(ImagePathArray[4], UriKind.Relative)) },
                Header = StringResource.Facing,
                Tag = ReaderControl.LayoutModes.Facing,
            };
            menuItemArray[5] = new MenuItem()
            {
                Icon = new Image() { Source = new BitmapImage(new Uri(ImagePathArray[5], UriKind.Relative)) },
                Header = StringResource.CoverFacing,
                Tag = ReaderControl.LayoutModes.FacingCover,
            };

            if (_readerControl != null)
            {
                int selectedIndex = (int)_readerControl.LayoutMode;
                menuItemArray[selectedIndex].Background = new SolidColorBrush(Color.FromArgb(0x55, 0x87, 0xCE, 0xFA));
            }

            menuItemArray[0].Click += new RoutedEventHandler(MenuItem_Click);
            menuItemArray[1].Click += new RoutedEventHandler(MenuItem_Click);
            menuItemArray[2].Click += new RoutedEventHandler(MenuItem_Click);
            menuItemArray[3].Click += new RoutedEventHandler(MenuItem_Click);
            menuItemArray[4].Click += new RoutedEventHandler(MenuItem_Click);
            menuItemArray[5].Click += new RoutedEventHandler(MenuItem_Click);

            conMenu.Items.Add(menuItemArray[0]);
            conMenu.Items.Add(menuItemArray[1]);
            conMenu.Items.Add(menuItemArray[2]);
            conMenu.Items.Add(new Separator());
            conMenu.Items.Add(menuItemArray[3]);
            conMenu.Items.Add(menuItemArray[4]);
            conMenu.Items.Add(menuItemArray[5]);
            
            conMenu.IsOpen = true;
            conMenu.Closed += new RoutedEventHandler(conMenu_Closed);
        }

        void conMenu_Closed(object sender, RoutedEventArgs e)
        {
            this.LayoutButton.Focus();
        }

        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_readerControl != null && (sender as MenuItem).Tag.GetType() == typeof(ReaderControl.LayoutModes))
            {
                _readerControl.LayoutMode = (ReaderControl.LayoutModes)(sender as MenuItem).Tag;
            }
        }
    }
}
