// RedLetterDayConverter.cs by Charles Petzold, March 2009
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RedLetterDays
{
    public class RedLetterDayConverter : IValueConverter
    {
        static Dictionary<DateTime, string> dict = 
                    new Dictionary<DateTime, string>();

        static RedLetterDayConverter()
        {
            dict.Add(new DateTime(2009, 3, 17), "St. Patrick's Day");
            dict.Add(new DateTime(2009, 3, 20), "First day of spring");
            dict.Add(new DateTime(2009, 4, 1), "April Fools");
            dict.Add(new DateTime(2009, 4, 22), "Earth Day");
            dict.Add(new DateTime(2009, 5, 1), "May Day");
            dict.Add(new DateTime(2009, 5, 10), "Mother's Day");
            dict.Add(new DateTime(2009, 6, 21), "First Day of Summer");
        }

        public object Convert(object value, Type targetType, 
                              object parameter, CultureInfo culture)
        {
            string text;
            if (!dict.TryGetValue((DateTime)value, out text))
                text = null;
            return text;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return null;
        }
    }























/*
    public class RedLetterDayLookup : ToolTip
    {

        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime",
                typeof(DateTime),
                typeof(RedLetterDayLookup),
                new PropertyMetadata(new DateTime(), OnDateTimeChanged));

        protected static readonly DependencyPropertyKey TextKey =
            DependencyProperty.RegisterReadOnly("Text",
                typeof(string),
                typeof(RedLetterDayLookup),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            TextKey.DependencyProperty;

        public DateTime DateTime
        {
            set { SetValue(DateTimeProperty, value); }
            get { return (DateTime)GetValue(DateTimeProperty); }
        }

        public string Text
        {
            protected set { SetValue(TextKey, value); }
            get { return (string)GetValue(TextProperty); }
        }

        static RedLetterDayLookup()
        {
            dict.Add(new DateTime(2009, 3, 17), "St. Patrick's Day");
            dict.Add(new DateTime(2009, 3, 20), "First day of spring");

        }

        static void OnDateTimeChanged(DependencyObject obj,
                        DependencyPropertyChangedEventArgs args)
        {
            string text;
            dict.TryGetValue((DateTime)args.NewValue, out text);
            (obj as RedLetterDayLookup).Content = text;                         //  Text = text;
        }
    }
 */



}
