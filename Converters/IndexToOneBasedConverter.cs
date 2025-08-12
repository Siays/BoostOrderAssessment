using System;
using System.Globalization;
using System.Windows.Data;

namespace BoostOrderAssessment.Converters
{
    public class IndexToOneBasedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
                return (index + 1).ToString() + ". ";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
