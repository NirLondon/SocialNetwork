using System;
using Windows.UI.Xaml.Data;

namespace Client.WUP.Converters
{
    class NullableToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTime = (DateTime?)value;

            return dateTime.HasValue ?
                DateTimeOffset.FromUnixTimeMilliseconds(dateTime.Value.Ticks / (int)Math.Pow(10, 7))
                : default(DateTimeOffset);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.FromFileTimeUtc(((DateTimeOffset)value).ToFileTime());
        }
    }
}
