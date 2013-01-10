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

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NoeticTools.nLogCruncher.Domain
{
    public class LogSets : ILogSets
    {
        public static readonly ObservableCollection<ILogSet> Sets = new ObservableCollection<ILogSet>();
        private readonly List<ILogSet> sets = new List<ILogSet>();

        public LogSets()
        {
            var defaultLogSet = new LogSet("A");
            sets.Add(defaultLogSet);
            Sets.Add(defaultLogSet);
        }

        public ILogSet this[string setName]
        {
            get { return sets[0]; }
        }

        public ILogSet[] GetSetsFor(ILogEvent logEvent)
        {
            return sets.FindAll(thisSet => thisSet.Includes(logEvent)).ToArray();
        }
    }
}