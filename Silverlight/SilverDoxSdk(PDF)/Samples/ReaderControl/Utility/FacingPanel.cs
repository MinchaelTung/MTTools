using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace PDFTron.SilverDox.Samples.Utility
{

    public class FacingPanel : Panel
    {

        public static readonly DependencyProperty CoverFacingProperty;

        public bool CoverFacing
        {
            get { return (bool)GetValue(CoverFacingProperty); }
            set { SetValue(CoverFacingProperty, (bool)value); }
        }

        static FacingPanel()
        {
            CoverFacingProperty = DependencyProperty.Register("CoverFacing", typeof(bool), typeof(FacingPanel), null);
        }

        // Overrides
        protected override Size MeasureOverride(Size availableSize)
        {
            int i = 0;

            if (this.CoverFacing == true)
                i = 1;
            else
                i = 0;

            double h = 0;
            double w = 0;
            double rowHeight = 0;
            double maxWidthEven = 0, maxWidthOdd = 0;
            int lastVisilblePage = i;
            // Size of entire panel
            foreach (UIElement element in this.Children)
            {
                if (element.Visibility != Visibility.Collapsed)
                {
                    element.Measure(availableSize);

                    if (i % 2 == 0)
                    {
                        //even page
                        rowHeight = element.DesiredSize.Height;
                        maxWidthEven = Math.Max(maxWidthEven, element.DesiredSize.Width);
                    }
                    else
                    {
                        //odd page
                        h += Math.Max(rowHeight, element.DesiredSize.Height);
                        maxWidthOdd = Math.Max(maxWidthOdd, element.DesiredSize.Width);
                    }
                    lastVisilblePage = i ;
                }

                i++;
            }

            // find the total width of the panel
            if (maxWidthEven == 0)
            {
                //there was no even page, use two times the odd page width
                w = maxWidthOdd * 2;
            }
            else if (maxWidthOdd == 0)
            {
                //there was no odd page, use two times the even page width
                w = maxWidthEven * 2;
            }
            else
            {
                // total width is the sum of maximum page widths on the even and odd side
                w = maxWidthEven + maxWidthOdd;
            }

            // if the last page was even, add the page height to the total height
            // (since total height is only updated on odd pages in the for loop above)
            if (lastVisilblePage % 2 == 0)
            {
                h += rowHeight;
            }

            double width = double.IsPositiveInfinity(availableSize.Width) ?
                 w : availableSize.Width;
            double height = double.IsPositiveInfinity(availableSize.Height) ?
                 h : availableSize.Height;

            // Return our desired size
            return new Size(width, height);
        }

        // Place individual pages
        protected override Size ArrangeOverride(Size finalSize)
        {
            int i = 0;

            double lastW = 0;
            double lastH = 0;
            double prevH = 0;

            double maxWidthEven = 0, maxWidthOdd = 0;

            if (this.CoverFacing == true)
            {
                i = 1;
                if (this.Children.Count > 0)
                    lastW = this.Children[0].DesiredSize.Width;
            }
            else
                i = 0;

            int lastVisilblePage = i;

            double pw = 0, ph = 0;
            // Size and position the child elements
            foreach (FrameworkElement element in this.Children)
            {
                if (element.Visibility != Visibility.Collapsed)
                {
                    Rect r = new Rect();
                    r.Height = element.DesiredSize.Height;
                    r.Width = element.DesiredSize.Width;

                    if (i % 2 == 0)
                    {
                        r.X = 0;
                        lastW = element.DesiredSize.Width;
                        r.Y = lastH;
                        prevH = (element.DesiredSize.Height);
                        maxWidthEven = Math.Max(maxWidthEven, element.DesiredSize.Width);
                    }
                    else
                    {
                        r.X = lastW;
                        r.Y = lastH;
                        lastH += Math.Max(element.DesiredSize.Height, prevH);
                        maxWidthOdd = Math.Max(maxWidthOdd, element.DesiredSize.Width);
                    }

                    element.Arrange(r);

                    pw = element.DesiredSize.Width;
                    ph = element.DesiredSize.Height;

                    lastW = pw;
                    lastVisilblePage = i;
                }
                i++;
            }

            double w=0;

            // for comments, see MeasureOverride
            if (maxWidthEven == 0)
                w = maxWidthOdd * 2;
            else if (maxWidthOdd == 0)
                w = maxWidthEven * 2;
            else
                w = maxWidthEven + maxWidthOdd;

            // if the last page was even, add the page height to the total height
            // (since total height is only updated on odd pages in the for loop above)
            if (lastVisilblePage %  2 == 0)
            {
                lastH += prevH;
            }

            return new Size(w, lastH);
        }

    }

}