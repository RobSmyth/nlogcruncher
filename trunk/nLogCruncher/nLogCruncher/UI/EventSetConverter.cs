using System;
using System.Globalization;
using System.Windows.Data;


namespace NoeticTools.nLogCruncher.UI
{
    public class EventSetConverter : IValueConverter
    {
        private readonly IEventsFormatterData formatterData;

        public EventSetConverter(IEventsFormatterData formatterData)
        {
            this.formatterData = formatterData;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}