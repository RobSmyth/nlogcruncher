using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using NoeticTools.nLogCruncher.Domain;


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
            var logEvent = (ILogEvent) value;
            var text = new StringBuilder();
            foreach (var logSet in logEvent.Sets)
            {
                text.Append(logSet.Name);
            }
            return text.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}