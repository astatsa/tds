using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    class NullToBoolConverter : IValueConverter
    {
        public bool NullIsTrue { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ^ NullIsTrue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
