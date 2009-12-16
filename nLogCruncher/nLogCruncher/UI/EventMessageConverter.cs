#region Copyright

// The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in compliance
//  with the License. You may obtain a copy of the License at
//  
//  http://www.mozilla.org/MPL/
//  
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations under 
//  the License.
//  
//  The Initial Developer of the Original Code is Robert Smyth.
//  Portions created by Robert Smyth are Copyright (C) 2008.
//  
//  All Rights Reserved.

#endregion

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
            var logEvent = (ILogEvent) value;
            var message = logEvent.Message;

            if (formatterData.ShowContextDepth)
            {
                var messageFormat = new StringBuilder();
                for (var indexLevel = 0; indexLevel < logEvent.Context.Depth; indexLevel++)
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