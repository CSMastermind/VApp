using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VApp.UserControls
{
    public sealed partial class Calendar : UserControl
    {
        public DateTime CurrentDate { get; set; }
        public DateTime SelectedDate { get; set; }

        private event EventHandler<TappedRoutedEventArgs> _selectionChange;
        public event EventHandler<TappedRoutedEventArgs> SelectionChange
        {
            add
            {
                _selectionChange += value;
            }
            remove
            {
                _selectionChange -= value;
            }
        }

        public void OnSelectionChange(object sender, TappedRoutedEventArgs e)
        {
            if (_selectionChange != null)
                _selectionChange(sender, e);
        }

        public Calendar()
        {
            this.InitializeComponent();
            this.CurrentDate = DateTime.Today;
            this.SelectedDate = this.CurrentDate;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCalendar();
        }

        private void InitializeCalendar()
        {
            //Clear the grid whenever the user load the first time or change the date
            CalendarGrid.RowDefinitions.Clear();
            CalendarGrid.Children.Clear();

            InitializeHeader();
            InitializeDayLabelBoxes();
            InitializePreviousMonthBoxes();
            InitializeCurrentMonthBoxes();
            InitializeNextMonthBoxes();
        }

        private void InitializeHeader()
        {
            DateTime date = new DateTime(this.CurrentDate.Year, this.CurrentDate.Month, 1);
            CurrentDateText.Text = date.ToString("MMMM, yyyy");
        }

        private void InitializeDayLabelBoxes()
        {
            int column = 0;
            CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
            {
                //Runtime generate controls and setting style
                Rectangle box = new Rectangle();
                box.Style = Application.Current.Resources["CalendarLabelBox"] as Style;
                box.SetValue(Grid.RowProperty, 0);
                box.SetValue(Grid.ColumnProperty, column);

                TextBlock textBlock = new TextBlock();
                textBlock.Style = Application.Current.Resources["CalendarLabel"] as Style;
                textBlock.Text = day.ToString();

                //Runtime setting the control Grid.Row and Grid.Column XAML property value
                textBlock.SetValue(Grid.RowProperty, 0);
                textBlock.SetValue(Grid.ColumnProperty, column);

                //Adding the box and the textblock control to the Grid during runtime
                CalendarGrid.Children.Add(box);
                CalendarGrid.Children.Add(textBlock);

                column++;
            }
        }

        private void InitializeCurrentMonthBoxes()
        {
            int row = 1;
            int maxDay = DateTime.DaysInMonth(this.CurrentDate.Year, this.CurrentDate.Month);
            for (int day = 1; day <= maxDay; day++)
            {
                DateTime dateIteration = new DateTime(this.CurrentDate.Year, this.CurrentDate.Month, day);
                int dayOfWeek = (int)dateIteration.DayOfWeek;

                CalendarItem item = new CalendarItem(dateIteration, day.ToString(), Application.Current.Resources["CalendarItemBox"] as Style);
                item.PointerEntered += (sender, args) => 
                { 
                    ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarMouseOverItemBox"] as Style; 
                };

                item.PointerExited += (sender, args) => 
                { 
                    if (((CalendarItem)sender).Value == this.SelectedDate)
                        ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarSelectedItemBox"] as Style; 
                    else
                        ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarItemBox"] as Style; 
                };

                //delegate the tapped event to selection change event
                item.Tapped += (sender, args) =>
                {
                    //get the box day value
                    ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarSelectedItemBox"] as Style;

                    //get the box before selected value and reset the color
                    var selectedItem = (CalendarItem)CalendarGrid.Children.Single(x => 
                                            x.GetType() == typeof(CalendarItem) && 
                                            ((CalendarItem)x).Value == this.SelectedDate);

                    selectedItem.GridStyle = Application.Current.Resources["CalendarItemBox"] as Style;

                    //update the selected date value
                    this.SelectedDate = ((CalendarItem)sender).Value;

                    OnSelectionChange(sender, args);
                };

                //highlight selected date
                if (this.SelectedDate.CompareTo(dateIteration) == 0)
                    item.GridStyle = Application.Current.Resources["CalendarSelectedItemBox"] as Style;

                item.SetValue(Grid.RowProperty, row);
                item.SetValue(Grid.ColumnProperty, dayOfWeek);

                CalendarGrid.Children.Add(item);

                if (dayOfWeek == 6 && day != maxDay)
                {
                    row++;
                    CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
            }
        }

        private void InitializePreviousMonthBoxes()
        {
            DateTime previousMonthDate = this.CurrentDate.AddMonths(-1);
            DateTime previousMonthDateIteration = new DateTime(previousMonthDate.Year, previousMonthDate.Month, DateTime.DaysInMonth(previousMonthDate.Year, previousMonthDate.Month));
            CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            for (int dayOfWeek = (int)previousMonthDateIteration.DayOfWeek; dayOfWeek >= 0; dayOfWeek--)
            {

                CalendarItem item = new CalendarItem(previousMonthDateIteration, previousMonthDateIteration.Day.ToString(), Application.Current.Resources["CalendarOtherMonthItemBox"] as Style);
                item.PointerEntered += (sender, args) =>
                {
                    ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarMouseOverItemBox"] as Style;
                };

                item.PointerExited += (sender, args) =>
                {
                    if (((CalendarItem)sender).Value == this.SelectedDate)
                        ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarSelectedItemBox"] as Style;
                    else
                        ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarOtherMonthItemBox"] as Style;
                };

                //delegate the tapped event to selection change event
                item.Tapped += (sender, args) =>
                {
                    //update the selected date value
                    this.SelectedDate = ((CalendarItem)sender).Value;
                    this.CurrentDate = this.CurrentDate.AddMonths(-1);
                    InitializeCalendar();

                    OnSelectionChange(sender, args);
                };

                item.SetValue(Grid.RowProperty, 1);
                item.SetValue(Grid.ColumnProperty, dayOfWeek);

                CalendarGrid.Children.Add(item);

                previousMonthDateIteration = previousMonthDateIteration.AddDays(-1);
            }
        }

        private void InitializeNextMonthBoxes()
        {
            DateTime nextMonthDate = this.CurrentDate.AddMonths(1);
            DateTime nextMonthDateIteration = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1);

            int lastRow = CalendarGrid.RowDefinitions.Count - 1;

            if (nextMonthDateIteration.DayOfWeek != DayOfWeek.Sunday)
                for (int dayOfWeek = (int)nextMonthDateIteration.DayOfWeek; dayOfWeek < 7; dayOfWeek++)
                {
                    CalendarItem item = new CalendarItem(nextMonthDateIteration, nextMonthDateIteration.Day.ToString(), Application.Current.Resources["CalendarOtherMonthItemBox"] as Style);
                    item.PointerEntered += (sender, args) =>
                    {
                        ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarMouseOverItemBox"] as Style;
                    };

                    item.PointerExited += (sender, args) =>
                    {
                        if (((CalendarItem)sender).Value == this.SelectedDate)
                            ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarSelectedItemBox"] as Style;
                        else
                            ((CalendarItem)sender).GridStyle = Application.Current.Resources["CalendarOtherMonthItemBox"] as Style;
                    };

                    //delegate the tapped event to selection change event
                    item.Tapped += (sender, args) =>
                    {
                        //update the selected date value
                        this.SelectedDate = ((CalendarItem)sender).Value;
                        this.CurrentDate = this.CurrentDate.AddMonths(1);
                        InitializeCalendar();

                        OnSelectionChange(sender, args);
                    };

                    item.SetValue(Grid.RowProperty, lastRow);
                    item.SetValue(Grid.ColumnProperty, dayOfWeek);

                    CalendarGrid.Children.Add(item);

                    nextMonthDateIteration = nextMonthDateIteration.AddDays(1);
                }
        }

        private void PreviousButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurrentDate = this.CurrentDate.AddMonths(-1);
            InitializeCalendar();
        }

        private void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurrentDate = this.CurrentDate.AddMonths(1);
            InitializeCalendar();
        }
    }
}
