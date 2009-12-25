using System;
using System.Globalization;
using System.Windows.Data;


namespace NoeticTools.nLogCruncher.UI
{
    public class LogSetConverter : IValueConverter
    {
        private readonly IEventsFormatterData formatterData;

        public LogSetConverter(IEventsFormatterData formatterData)
        {
            this.formatterData = formatterData;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}