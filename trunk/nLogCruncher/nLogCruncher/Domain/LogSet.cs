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

using System.Collections.Generic;


namespace NoeticTools.nLogCruncher.Domain
{
    public class LogSet : ILogSet
    {
        private readonly Dictionary<ILogEvent, bool> events = new Dictionary<ILogEvent, bool>();

        public LogSet(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public string Description { get { return string.Empty;} }

        public bool Includes(ILogEvent logEvent)
        {
            return events.ContainsKey(logEvent);
        }

        public void Add(LogEvent logEvent)
        {
            events.Add(logEvent, true);
        }
    }
}