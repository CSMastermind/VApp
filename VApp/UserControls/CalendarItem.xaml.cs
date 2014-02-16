using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VApp.UserControls
{
    public sealed partial class CalendarItem : UserControl
    {
        public DateTime Value { get; set; }
        public string Text { get; set; }

        private Style _gridStyle;
        public Style GridStyle 
        {
            get { return _gridStyle; }
            set 
            { 
                _gridStyle = value;

                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs("GridStyle"));
                }
            }
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemBox.Style = this.GridStyle;
        }

        public CalendarItem()
        {
            this.InitializeComponent();
        }

        public CalendarItem(DateTime value, string text)
            : this()
        {
            this.Value = value;
            this.Text = text;
        }

        public CalendarItem(DateTime value, string text, Style style) : this(value, text)
        {
            this.GridStyle = style;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ItemValue.Text = this.Text;
            ItemBox.Style = this.GridStyle;

            if (this.Value.CompareTo(DateTime.Today) == 0)
                ItemBox.Background = new SolidColorBrush(Colors.Orange);
        }
    }
}
