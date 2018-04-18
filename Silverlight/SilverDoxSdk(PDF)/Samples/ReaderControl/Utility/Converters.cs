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
using System.Windows.Data;
using PDFTron.SilverDox.Controls;
using System.Globalization;

namespace PDFTron.SilverDox.Samples.Utility
{

    /// <summary>
    /// Represents a class that that converts between DocumentViewer enum types  and integers for index binding
    /// </summary>
    public class ModeConverter : IValueConverter
    {
        /// <summary>
        /// Convert DocumentViewer enum to integer
        /// </summary>
        /// <param name="value">DocumentViewer enum type</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">type of enum, tool or pageview</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(((string)parameter).Equals("tool"))
            {
                switch ((DocumentViewer.ToolModes)value)
                {
                    case DocumentViewer.ToolModes.Pan:
                        return 0;
                    case DocumentViewer.ToolModes.TextSelect:
                        return 1;
                }
                return -1;
            }
            else if(((string)parameter).Equals("annotatetool"))
            {
                switch ((DocumentViewer.ToolModes)value)
                {
                    case DocumentViewer.ToolModes.Pan:
                        //var a = DocumentViewer.ToolModes.Pan.ToString("g");
                        return DocumentViewer.ToolModes.Pan.ToString("g");
                    case DocumentViewer.ToolModes.TextSelect:
                        return DocumentViewer.ToolModes.TextSelect.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateEllipse:
                        return DocumentViewer.ToolModes.AnnotationCreateEllipse.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateRectangle:
                        return DocumentViewer.ToolModes.AnnotationCreateRectangle.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateFreeHand:
                        return DocumentViewer.ToolModes.AnnotationCreateFreeHand.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateTextHighlight:
                        return DocumentViewer.ToolModes.AnnotationCreateTextHighlight.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateTextUnderline:
                        return DocumentViewer.ToolModes.AnnotationCreateTextUnderline.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateTextStrikeout:
                        return DocumentViewer.ToolModes.AnnotationCreateTextStrikeout.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateLine:
                        return DocumentViewer.ToolModes.AnnotationCreateLine.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationCreateSticky:
                        return DocumentViewer.ToolModes.AnnotationCreateSticky.ToString("g");
                    case DocumentViewer.ToolModes.AnnotationEdit:
                        return DocumentViewer.ToolModes.AnnotationEdit.ToString("g");
                    default:
                        return DocumentViewer.ToolModes.Pan.ToString("g");
                }
            }
            else if (((string)parameter).Equals("pageview"))
            {

                switch ((ReaderControl.PageViewModes)value)
                {
                    case ReaderControl.PageViewModes.Zoom:
                        return -1;
                    case ReaderControl.PageViewModes.FitWidth:
                        return 0;
                    case ReaderControl.PageViewModes.FitPage:
                        return 1;
                }
            
            }
            else if (((string)parameter).Equals("fit"))
            {
                
                switch ((ReaderControl.PageViewModes)value)
                {
                    case ReaderControl.PageViewModes.Zoom:
                        return -1;
                    case ReaderControl.PageViewModes.FitWidth:
                        return 0;
                    case ReaderControl.PageViewModes.FitPage:
                        return 1;
                }
            }
            
            return -1;
        }

        /// <summary>
        /// Converts integer to DocumentViewer enum
        /// </summary>
        /// <param name="value">integer index</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">type of enum, tool or pageview</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (((string)parameter).Equals("tool"))
            {
                switch ((int)value)
                {
                    case -1:
                        return DocumentViewer.ToolModes.Pan;
                    case 0:
                        return DocumentViewer.ToolModes.Pan;
                    case 1:
                        return DocumentViewer.ToolModes.TextSelect;
                }

            }
            else if(((string)parameter).Equals("annotatetool"))
            {
                var enumVal = (DocumentViewer.ToolModes)Enum.Parse(typeof(DocumentViewer.ToolModes), (string)value, true);

                switch (enumVal)
                {
                    case DocumentViewer.ToolModes.Pan:
                        return DocumentViewer.ToolModes.Pan;
                    case DocumentViewer.ToolModes.TextSelect:
                        return DocumentViewer.ToolModes.TextSelect;
                    case DocumentViewer.ToolModes.AnnotationCreateEllipse:
                        return DocumentViewer.ToolModes.AnnotationCreateEllipse;
                    case DocumentViewer.ToolModes.AnnotationCreateRectangle:
                        return DocumentViewer.ToolModes.AnnotationCreateRectangle;
                    case DocumentViewer.ToolModes.AnnotationCreateFreeHand:
                        return DocumentViewer.ToolModes.AnnotationCreateFreeHand;
                    case DocumentViewer.ToolModes.AnnotationCreateTextHighlight:
                        return DocumentViewer.ToolModes.AnnotationCreateTextHighlight;
                    case DocumentViewer.ToolModes.AnnotationCreateTextUnderline:
                        return DocumentViewer.ToolModes.AnnotationCreateTextUnderline;
                    case DocumentViewer.ToolModes.AnnotationCreateTextStrikeout:
                        return DocumentViewer.ToolModes.AnnotationCreateTextStrikeout;
                    case DocumentViewer.ToolModes.AnnotationCreateLine:
                        return DocumentViewer.ToolModes.AnnotationCreateLine;
                    case DocumentViewer.ToolModes.AnnotationCreateSticky:
                        return DocumentViewer.ToolModes.AnnotationCreateSticky;
                    case DocumentViewer.ToolModes.AnnotationEdit:
                        return DocumentViewer.ToolModes.AnnotationEdit;
                    default:
                        return DocumentViewer.ToolModes.Pan;
                }
            }
            else if (((string)parameter).Equals("pageview"))
            {

                switch ((int)value)
                {
                    case -1:
                        return ReaderControl.PageViewModes.Zoom;
                    case 0:
                        return ReaderControl.PageViewModes.FitWidth;
                    case 1:
                        return ReaderControl.PageViewModes.FitPage;
                }
            }
            else if (((string)parameter).Equals("fit"))
            {
                switch ((ReaderControl.PageViewModes)value)
                {
                    case ReaderControl.PageViewModes.Zoom:
                        return -1;
                    case ReaderControl.PageViewModes.FitWidth:
                        return 0;
                    case ReaderControl.PageViewModes.FitPage:
                        return 1;
                }
            }

            return null;            
        }

    }

    /// <summary>
    /// A converter class that represents a double value as a string in percentages
    /// </summary>
    public class ZoomLevelConverter : IValueConverter
    {
        /// <summary>
        /// Converts a double value to a percentage string
        /// </summary>
        /// <param name="value">the value in double format</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int zoomPercent = (int)(Math.Round((double)value, 2) * 100);
            return (zoomPercent.ToString());            
        }

        /// <summary>
        /// Converts a percentage string back to a double value
        /// </summary>
        /// <param name="value">the value in percentage as string</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (((string)value) == string.Empty)
            {
                return 1;
            }
            else
            {
                string zoomString = ((string)value);
                char[] charsToTrim = { ' ', '%' };
                string trimmedString = zoomString.TrimEnd(charsToTrim);
                double result;

                if (double.TryParse((string)trimmedString, out result) && result != 0)
                    return result * 0.01;
                else
                    return 1;
            }
        }
    }

    /// <summary>  
    /// A type converter for visibility and boolean values.  
    /// </summary>  
    public class VisibilityConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((parameter as string) == "notnull")
            {
                return (value != null) ? Visibility.Visible : Visibility.Collapsed;
            }
            if ((parameter as string) == "!notnull")
            {
                return (value != null && !(bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            bool visibility;
            if ((parameter as string) == "!")
                visibility = !(bool)value;
            else
                visibility = (bool)value;

            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if ((parameter as string) == "!")
                return (visibility != Visibility.Visible);
            else
                return (visibility == Visibility.Visible);
        }
    }

    /// <summary>  
    /// A type converter to change the alpha of a Color
    /// </summary>  
    public class ColorAlphaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Color)(value as Color?);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as Color?;

            var alpha = parameter as byte?;

            if (color != null && alpha != null)
            {
                //Don't bother adding alpha if color is already transparent
                if (color == Colors.Transparent) return color;

                var c = (Color)color;
                return Color.FromArgb((byte)alpha, c.R, c.G, c.B);
            }
            else
            {
                return Colors.Transparent;
            }
        }
    }
    /// <summary>  
    /// A type converter that returns Visibility.Visible if value is not null; returns Visibility.Collapsed otherwise.
    /// For string objects, whitespace values are also considered as being null.
    /// </summary>  
    public class HideIfNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as string) != null)
            {
                return String.IsNullOrWhiteSpace(value as string) ?
                       (Visibility.Collapsed) : (Visibility.Visible);
            }
            else if( value != null) 
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}