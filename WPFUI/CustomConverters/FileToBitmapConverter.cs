using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPFUI.CustomConverters
{
    public class FileToBitmapConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> _locations =
            new Dictionary<string, BitmapImage>();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string filename)
            {
                return null;
            }
            if (!_locations.ContainsKey(filename))
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}{filename}",
                                                       UriKind.Absolute));
                _locations.Add(filename, bitmapImage);
            }
            return _locations[filename];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return null;
        }
    }
}