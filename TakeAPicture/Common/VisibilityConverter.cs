using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TakeAPicture.Common
{
    public class VisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is Boolean)
            {
                Boolean negateValue;
                Boolean.TryParse(parameter as String, out negateValue);

                Boolean newValue;
                if (negateValue)
                    newValue = !(Boolean)value;
                else
                    newValue = (Boolean)value;
                return newValue ? Visibility.Visible : Visibility.Hidden;
            }

            return value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}