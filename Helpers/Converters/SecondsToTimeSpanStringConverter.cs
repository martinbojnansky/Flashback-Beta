using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Helpers.Converters
{
    public sealed class SecondsToTimeSpanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var timespan = TimeSpan.FromSeconds((double)value);
                return timespan.ToString(@"mm\:ss\.ff");
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Converting from string is not supported.");
        }
    }
}
