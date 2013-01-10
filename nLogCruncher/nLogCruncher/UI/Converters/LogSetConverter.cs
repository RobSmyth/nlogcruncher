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
using System.Text;
using System.Windows.Data;
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI.Converters
{
    public class LogSetConverter : IValueConverter
    {
        private readonly ILogSets logSets;

        public LogSetConverter(ILogSets logSets)
        {
            this.logSets = logSets;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = new StringBuilder();
            var logEvent = (ILogEvent) value;
            foreach (var logSet in logSets.GetSetsFor(logEvent))
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