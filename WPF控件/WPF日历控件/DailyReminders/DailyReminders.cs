// DailyReminders.cs by Charles Petzold, March 2009
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace DailyReminders
{
    public partial class DailyReminders : Window
    {
        [STAThread]
        public static void Main()
        {
            // Can't have two instances of DailyRemindersCalendar runnning at same time 
            //  because of possible storage conflict
            Process thisProcess = Process.GetCurrentProcess();

            foreach (Process process in Process.GetProcessesByName(thisProcess.ProcessName))
                if (process.Id != thisProcess.Id)
                {
                    MessageBox.Show("Only one instance of DailyReminders can be running at any time", "Daily Reminders");
                    return;
                }

            Application app = new Application();
            app.Run(new DailyReminders());
        }

        public DailyReminders()
        {
            Title = "Daily Reminders";
            ResizeMode = ResizeMode.CanMinimize;
            WindowStyle = WindowStyle.SingleBorderWindow;
            SizeToContent = SizeToContent.WidthAndHeight;

            DailyRemindersCalendar cal = new DailyRemindersCalendar();
            cal.LayoutTransform = new ScaleTransform(1.5, 1.5);
            Content = cal;
        }
    }
}
