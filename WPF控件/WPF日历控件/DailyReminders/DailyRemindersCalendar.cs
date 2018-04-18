// DailyRemindersCalendar.cs by Charles Petzold, March 2009
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;

namespace DailyReminders
{
    public class DailyRemindersCalendar : Calendar
    {
        static DailyRemindersStorage storage = new DailyRemindersStorage();

        static DailyRemindersCalendar()
        {
            SelectionModeProperty.OverrideMetadata(
                typeof(DailyRemindersCalendar), 
                new FrameworkPropertyMetadata(CalendarSelectionMode.None));
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs args)
        {
            if (args.ChangedButton != MouseButton.Left)
                return;

            if (args.OriginalSource is FrameworkElement &&
                (args.OriginalSource as FrameworkElement).DataContext is DateTime)
            {
                DateTime dateTime = (DateTime)(args.OriginalSource as FrameworkElement).DataContext;

                // Check to see if this one is already loaded
                WindowCollection windows = Application.Current.Windows;

                foreach (Window window in windows)
                {
                    if (window is DailyRemindersDialog && (DateTime)window.DataContext == dateTime)
                    {
                        window.Focus();
                        return;
                    }
                }

                // Otherwise, create a new dialog
                DailyRemindersDialog dlg =
                    new DailyRemindersDialog(Application.Current.MainWindow, storage, dateTime);
                dlg.DataContext = dateTime;
                dlg.Show();
            }

            base.OnMouseDoubleClick(args);
        }
    }
}
