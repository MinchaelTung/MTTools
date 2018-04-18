using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDFTron.SilverDox.Controls;

using PDFTron.SilverDox.Samples.SubControls;
using System.Diagnostics;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using PDFTron.SilverDox.Samples.Resources;




namespace PDFTron.SilverDox.Samples
{
    /// <summary>
    /// Represents a customizable toolbar for manipulating a DocumentViewer.
    /// The DataContext of this control must be a DocumentViewer.
    /// </summary>
    public partial class ToolbarControl : UserControl
    {

        /// <summary>
        /// Creates a new instance of the ToolbarControl
        /// </summary>
        public ToolbarControl()
        {
            InitializeComponent();
            this.ToolBorder.GotFocus += new RoutedEventHandler(ToolBorder_GotFocus);
        }

        void ToolBorder_GotFocus(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a toolbar based on customization options from ReaderControl
        /// </summary>
        /// <param name="reader">a ReaderControl that holds customization options </param>
        public void CreateToolbar(ReaderControl reader)
        {

            // first group of tools: outline toggle 
            if (reader.EnableOutlineToggleControl)
            {
                this.LeftToolStackPanel.Children.Add(new OutlineToggleButtonControl(reader));
            }

            int toolsLeft = CountNumberOfTools(reader);
            int i = 0;

            if (reader.EnableOpenLocalFileControl)
            {
                this.ToolStackPanel.Children.Add(new OpenLocalFileButtonControl(reader));
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            // next group of tools: Page 
            if (reader.EnablePageNumberControl)
            {
                this.ToolStackPanel.Children.Add(new PageNumberControl());
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            if (reader.EnableLayoutControl)
            {
                this.ToolStackPanel.Children.Add(new LayoutControl(reader));
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            if (reader.EnableRotateControl)
            {
                this.ToolStackPanel.Children.Add(new RotatePagesControl());
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            if (reader.EnablePageNavigationControl)
            {
                this.ToolStackPanel.Children.Add(new PageNavigationControl(false, true, true, false));
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            // next group of tools: Zoom
            if (reader.EnableZoomSliderControl)
            {
                this.ToolStackPanel.Children.Add(new ZoomSliderControl());
                i++;
            }

            if (reader.EnableZoomTextBoxControl)
            {
                this.ToolStackPanel.Children.Add(new ZoomTextBoxControl());
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            // next group of tools: Fit Mode
            if (reader.EnableFitModeControl)
            {
                this.ToolStackPanel.Children.Add(new FitModeControl(reader));
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            // next group of tools: ToolMode
            if (reader.EnableToolModeControl)
            {
                this.ToolStackPanel.Children.Add(new ToolModeControl());
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            // next group of tools: Search
            if (reader.EnableSearchControl)
            {
                this.ToolStackPanel.Children.Add(new SearchControl() { Name = "seachControl" });
                i++;
            }

            // add divider if necessary
            toolsLeft = toolsLeft - i;
            if (i > 0 && toolsLeft > 0)
            {
                this.ToolStackPanel.Children.Add(new DividerControl());
                i = 0;
            }

            // last group of tools: Auxilary            
            if (reader.EnablePrintControl)
            {
                this.ToolStackPanel.Children.Add(new PrintButtonControl());
            }
            if (reader.EnableFullScreenControl)
            {
                this.ToolStackPanel.Children.Add(new FullScreenButtonControl());
            }



            this.IsPinned = true;
            PinnedVisualState(true);
        }

        private void ZoomLevelBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DocumentViewer fixedDocumentViewer = ((ComboBox)sender).DataContext as DocumentViewer;
            if (fixedDocumentViewer != null)
            {
                double zoomLevel = 0;
                if (double.TryParse((string)(e.AddedItems[0] as ComboBoxItem).Tag, out zoomLevel))
                {
                    fixedDocumentViewer.Zoom = zoomLevel;
                }
            }
        }

        private int CountNumberOfTools(ReaderControl s)
        {
            int i = 0;


            if (s.EnableOpenLocalFileControl) i++;
            if (s.EnablePageNumberControl) i++;
            if (s.EnablePageNavigationControl) i++;

            if (s.EnableRotateControl) i++;
            if (s.EnableLayoutControl) i++;

            if (s.EnableZoomSliderControl) i++;
            if (s.EnableZoomTextBoxControl) i++;

            if (s.EnableFitModeControl) i++;
            if (s.EnableToolModeControl) i++;
            if (s.EnablePrintControl) i++;
            if (s.EnableFullScreenControl) i++;

            return i;
        }


        public void FadeIn()
        {
            this.FadeInBar.Begin();
        }

        public void FadeOut()
        {
            this.FadeOutBar.Begin();
        }


        private bool isPinned;
        public bool IsPinned
        {
            get
            {
                return isPinned;
            }
            set
            {
                if (value != isPinned)
                {
                    isPinned = value;

                    if (this.IsPinnedChanged != null)
                    {
                        IsPinnedChanged(this, new RoutedPropertyChangedEventArgs<bool>(!isPinned, isPinned));
                    }
                    PinnedVisualState(isPinned);
                }
            }
        }

        private void PinnedVisualState(bool isPinned)
        {
            if (isPinned)
            {
                //pinned visual state
                this.ToolBorder.Opacity = 1.0;
                ToolBorder.CornerRadius = new CornerRadius(0);
                ToolBorder.Margin = new Thickness(0);
                ToolGrid.Margin = new Thickness(0);
                ToolBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                ToolGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                ToolBorder.Effect = null;

                //show tools
                for (int i = 0; i < this.ToolStackPanel.Children.Count; i++)
                {
                    if (this.ToolStackPanel.Children[i].GetType() == typeof(SearchControl))
                    {
                        (this.ToolStackPanel.Children[i] as FrameworkElement).Visibility = Visibility.Visible;

                        if (i > 0 && this.ToolStackPanel.Children[i - 1].GetType() == typeof(DividerControl))
                            (this.ToolStackPanel.Children[i - 1] as FrameworkElement).Visibility = Visibility.Visible;
                        break;
                    }
                }
            }
            else
            {
                //unpinned visual state
                ToolBorder.CornerRadius = new CornerRadius(0, 0, 10, 10);
                ToolBorder.Margin = new Thickness(30, 0, 30, 0);
                ToolGrid.Margin = new Thickness(30, 0, 30, 0);
                ToolBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ToolGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ToolBorder.Effect = new DropShadowEffect() { ShadowDepth = 1 };

                //hide tools                
                for (int i = 0; i < this.ToolStackPanel.Children.Count; i++)
                {
                    if (this.ToolStackPanel.Children[i].GetType() == typeof(SearchControl))
                    {
                        (this.ToolStackPanel.Children[i] as FrameworkElement).Visibility = Visibility.Collapsed;

                        if (i > 0 && this.ToolStackPanel.Children[i - 1].GetType() == typeof(DividerControl))
                            (this.ToolStackPanel.Children[i - 1] as FrameworkElement).Visibility = Visibility.Collapsed;

                        break;
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            IsPinned = !IsPinned;

            if (IsPinned)
            {
                this.FadeInBar.Stop();
                this.FadeOutBar.Stop();
                this.ToolBorder.Opacity = 1.0;
            }
        }

        private void ToolGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            if (!IsPinned && this.ToolBorder.Opacity != 1.0)
            {
                this.FadeOutBar.Stop();
                this.FadeInBar.Begin();
            }
        }

        private void ToolGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            Object focusedObject = FocusManager.GetFocusedElement();

            if (focusedObject is ContextMenu)
                return;

            System.Diagnostics.Debug.WriteLine(e.GetPosition(sender as FrameworkElement).ToString());
            if (!IsPinned && !suppressFadeOut)
            {
                this.FadeInBar.Stop();
                this.FadeOutBar.Begin();
                this.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private bool suppressFadeOut = false;
        private void MenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPinned)
                (sender as MenuItem).Header = StringResource.UnpinToolbarToolTip;
            else
                (sender as MenuItem).Header = StringResource.PinToolbarToolTip;

            suppressFadeOut = true;
        }

        private void MenuItem_Unloaded(object sender, RoutedEventArgs e)
        {
            suppressFadeOut = false;
        }


        public event RoutedPropertyChangedEventHandler<bool> IsPinnedChanged;

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
        }




    }
}
