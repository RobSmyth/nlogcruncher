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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace NoeticTools.nLogCruncher.Domain
{
    public class LogEvent : ILogEvent
    {
        private readonly string eventText;

        public LogEvent(string eventText, IEventContext rootContext, ICollection<IEventLevel> levels)
        {
            this.eventText = eventText;

            string contextName;
            if (eventText.StartsWith("<log4j:event"))
            {
                var regex =
                    new Regex(
                        "logger=\"(?<context>.*)\" level=\"(?<level>.*)\" timestamp=\"(?<timeDate>.*)\" .*log4j:message>(?<message>.*)</log4j:message>",
                        RegexOptions.Multiline);

                var matches = regex.Matches(eventText);

                contextName = matches[0].Groups["context"].Value.Trim();
                var milliseconds = long.Parse(matches[0].Groups["timeDate"].Value.Trim());
                Time = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds);
                Time = Time.ToUniversalTime();
                Level = matches[0].Groups["level"].Value.Trim();
                Message = matches[0].Groups["message"].Value.Trim();
            }
            else
            {
                contextName = GetContextName();
            }

            IsControlMessage = (contextName.ToLower() == "nLogCruncher.Command".ToLower());

            Context = rootContext;
            foreach (var name in contextName.Split('.'))
            {
                Context = Context.GetContext(name);
            }

            if (levels.Count(thisLevel => thisLevel.Name.ToLower() == Level.ToLower()) == 0)
            {
                levels.Add(new EventLevel(Level));
            }
        }

        public string Level { get; set; }
        public DateTime Time { get; private set; }
        public string Message { get; private set; }
        public IEventContext Context { get; private set; }
        public ILogEvent Self { get { return this; } }
        public bool IsControlMessage { get; private set; }

        private string GetContextName()
        {
            var regex = new Regex(@"^(?<timeDate>.*)\|(?<context>.*)\|(?<level>.*)\|(?<message>.*)$",
                                  RegexOptions.Multiline);
            var matches = regex.Matches(eventText);

            string context;
            if (matches.Count == 1 && matches[0].Groups.Count > 4)
            {
                Time = DateTime.Parse(matches[0].Groups["timeDate"].Value.Trim());
                context = matches[0].Groups["context"].Value.Trim();
                Level = matches[0].Groups["level"].Value.Trim();
                Message = matches[0].Groups["message"].Value.Trim();
            }
            else
            {
                Time = DateTime.MinValue; // "Unknown";
                Level = "Unkown";
                Message = eventText;
                context = "Unknown";
            }

            return context;
        }
    }
}