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
using System.Windows.Data;

namespace PDFTron.SilverDox.Samples.Controls
{
    /// <summary>
    /// A control used to select a color
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        /// <summary>
        /// Dependency Property for Value
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Color),
                    typeof(ColorPicker),
                    new PropertyMetadata(
                        (sender, e) =>
                        {
                            var source = sender as ColorPicker;

                            if (source.ValueChanged != null)
                            {
                                var oldColor = (Color)e.OldValue;
                                var newColor = (Color)e.NewValue;
                                source.ValueChanged.Invoke(sender,
                                new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor));
                            }
                        }
                    )
        );

        /// <summary>
        /// The selected color
        /// </summary>
        public Color Value
        {
            get { return (Color)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of ColorPicker
        /// </summary>
        public ColorPicker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the selected color is changed
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Color> ValueChanged;

    }


    /// <summary>
    /// A class that converts a <c>Windows.Media.Color</c> object to a string representation and vice versa.
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <c>Windows.Media.Color?</c> object into a its string representation: ARGB hex values (i.e #FF000000).
        /// </summary>
        /// <param name="value">A nullable color object</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>A string representation of the color's ARGB hex values</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var color = (Color)(value as Color?);

            //If Alpha is 00, return Transparent
            if (color.A.Equals(0)) return "Transparent";

            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        /// <summary>
        /// A ARGB hex color string to a nullable Windows.Media.Color object
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var colorString = value as string;

            if (String.IsNullOrWhiteSpace(colorString)) return Colors.Transparent;
            if (colorString.Equals("Transparent")) return Colors.Transparent;

            byte a = 255;
            byte r = (byte)(System.Convert.ToUInt32(colorString.Substring(1, 2), 16));
            byte g = (byte)(System.Convert.ToUInt32(colorString.Substring(3, 2), 16));
            byte b = (byte)(System.Convert.ToUInt32(colorString.Substring(5, 2), 16));

            return Color.FromArgb(a, r, g, b);
        }
    }
}
