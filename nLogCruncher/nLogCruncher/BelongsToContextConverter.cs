﻿#region Copyright

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
using System.Windows;
using System.Windows.Data;


namespace NoeticTools.nLogCruncher
{
    public class BelongsToContextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}