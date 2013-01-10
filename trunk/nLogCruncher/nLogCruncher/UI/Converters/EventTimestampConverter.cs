#region Copyright

// // The contents of this file are subject to the Mozilla Public License
// // Version 1.1 (the "License"); you may not use this file except in compliance
// // with the License. You may obtain a copy of the License at
// //   
// // http://www.mozilla.org/MPL/
// //   
// // Software distributed under the License is distributed on an "AS IS"
// // basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
// // License for the specific language governing rights and limitations under 
// // the License.
// //   
// // The Initial Developer of the Original Code is Robert Smyth.
// // Portions created by Robert Smyth are Copyright (C) 2008,2013.
// //   
// // All Rights Reserved.

#endregion

using System;
using System.Globalization;
using System.Windows.Data;


namespace NoeticTools.nLogCruncher.UI.Converters
{
    public class EventTimestampConverter : IValueConverter
    {
        private readonly IEventsFormatterData _formatterData;

        public EventTimestampConverter(IEventsFormatterData formatterData)
        {
            this._formatterData = formatterData;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timestamp = (DateTime) value;
            if (_formatterData.TimeFormat == TimeStampFormat.Relative)
            {
                var relativeTime = timestamp - _formatterData.ReferenceLogEvent.Time;
                var negative = relativeTime < TimeSpan.Zero;
                var duration = relativeTime.Duration();
                return string.Format("{0}{1:00}:{2:00}:{3:00}.{4:000}",
                                     negative ? "-" : "",
                                     duration.Hours, duration.Minutes, duration.Seconds, duration.Milliseconds);
            }
            return timestamp.ToString("HH:MM:ss.fff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}