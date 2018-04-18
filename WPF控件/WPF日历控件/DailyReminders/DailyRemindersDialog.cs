// DailyRemindersDialog.cs by Charles Petzold, March 2009
using System;
using System.Windows;
using System.Windows.Controls;

namespace DailyReminders
{
    public class DailyRemindersDialog : Window
    {
        DailyRemindersStorage storage;
        Grid grid;

        public DailyRemindersDialog(Window owner, DailyRemindersStorage storage, DateTime dateTime)
        {
            this.storage = storage;

            ShowInTaskbar = false;
            WindowStyle = WindowStyle.ToolWindow;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Title = dateTime.ToLongDateString();
            Owner = owner;
            Width = 400;
            Height = 500;

            // Create the visual tree in code due to the repetition
            grid = new Grid();
            ColumnDefinition coldef = new ColumnDefinition();
            coldef.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(coldef);

            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(coldef);

            // Prepare for the creation loop
            int yr = dateTime.Year;
            int mn = dateTime.Month;
            int dy = dateTime.Day;
            DateTime dtBeg = new DateTime(yr, mn, dy, 8, 0, 0);
            DateTime dtEnd = new DateTime(yr, mn, dy, 23, 0, 0);
            TimeSpan tsInc = TimeSpan.FromMinutes(30);
            int row = 0;

            for (DateTime dt = dtBeg; dt < dtEnd; dt += tsInc, row++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                grid.RowDefinitions.Add(rowdef);

                TextBlock txtblk = new TextBlock();
                txtblk.Text = dt.ToShortTimeString();
                txtblk.Margin = new Thickness(6, 2, 6, 2);
                txtblk.HorizontalAlignment = HorizontalAlignment.Right;
                txtblk.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(txtblk, row);
                Grid.SetColumn(txtblk, 0);
                grid.Children.Add(txtblk);

                TextBox txtbox = new TextBox();
                txtbox.AcceptsReturn = true;
                txtbox.DataContext = dt;
                txtbox.Text = storage.GetReminderText(dt);
                Grid.SetRow(txtbox, row);
                Grid.SetColumn(txtbox, 1);
                grid.Children.Add(txtbox);
            }
            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            viewer.Content = grid;
            Content = viewer;

            grid.Children[1].Focus();
        }

        protected override void OnClosed(EventArgs args)
        {
            SaveAllReminders();
            base.OnClosed(args);
        }

        public void SaveAllReminders()
        {
            foreach (UIElement child in grid.Children)
            {
                if (child is TextBox)
                {
                    TextBox txtbox = child as TextBox;
                    storage.Update((DateTime)txtbox.DataContext, txtbox.Text);
                }
            }
            storage.Save();
        }
    }
}
