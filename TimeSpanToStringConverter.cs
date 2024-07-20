namespace MauiApp1;
using System;
using System.Globalization;

public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Convert TimeSpan to string
        if (value is TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Convert string to TimeSpan
        if (value is string timeString && TimeSpan.TryParse(timeString, out TimeSpan timeSpan))
        {
            return timeSpan;
        }
        return TimeSpan.Zero;
    }
}
