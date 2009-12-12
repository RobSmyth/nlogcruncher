using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI
{
    public class EventMessageConverter : IValueConverter
    {
        private readonly IEventsFormatterData formatterData;

        public EventMessageConverter(IEventsFormatterData formatterData)
        {
            this.formatterData = formatterData;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logEvent = (LogEvent) value;
            var message = logEvent.Message;

            if (formatterData.ShowContextDepth)
            {
                var messageFormat = new StringBuilder();
                for (int indexLevel = 0; indexLevel < logEvent.Context.Depth; indexLevel++)
                {
                    messageFormat.Append("  ");
                }
                messageFormat.Append("{0}");
                message = string.Format(messageFormat.ToString(), logEvent.Message);
            }

            return message;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}