using System;
using System.Windows.Data;

namespace Model
{
    public class BoolToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "Yes";
                else
                    return "No";
            }
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value.ToString().ToLower())
            {
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return Binding.DoNothing;
            }
        }

    }
}
