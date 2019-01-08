using System;
using Windows.UI.Xaml.Data;

namespace Client.WUP.Converters
{
    class ObjectToDateTimeOffsetConveerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (DateTimeOffset)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (DateTimeOffset)value;
        }
    }
}
