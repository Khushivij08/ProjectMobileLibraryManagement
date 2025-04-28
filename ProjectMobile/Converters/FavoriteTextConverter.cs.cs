using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMobile.Converters
{
    public class FavoriteTextConverter : IValueConverter
    {
        // Converts the boolean IsFavorite to a string with emoji.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorite)
            {
                return isFavorite ? "💔 Unfavorite" : "❤️ Favorite"; // Change text based on IsFavorite
            }
            return "❤️ Favorite"; // Default text if IsFavorite is null or not a boolean
        }

        // The ConvertBack method is not implemented since we're only converting the value for display.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
