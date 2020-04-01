using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    class OrderStateToTextConverter : IValueConverter
    {
        public string[] StateAliases { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateAliases[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (OrderStates)Array.IndexOf(StateAliases, value);
        }
    }
}
