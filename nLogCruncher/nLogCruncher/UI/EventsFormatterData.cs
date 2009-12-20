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
using System.Collections;
using System.Collections.Generic;
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI
{
    public class EventsFormatterData : IEventsFormatterData
    {
        private readonly IEventListener<FormatChanged> formatChangedListener;
        private bool showContextDepth;
        private TimeStampFormat timeFormat;
        private readonly Dictionary<IEventContext, bool> hiddenContextsCache = new Dictionary<IEventContext, bool>();

        public EventsFormatterData(IEventListener<FormatChanged> formatChangedListener)
        {
            this.formatChangedListener = formatChangedListener;
            timeFormat = TimeStampFormat.Absolute;
            HiddenMessages = new List<string>();
            HiddenEventsInExactContexts = new Dictionary<IEventContext, bool>();
            HiddenEventsInContexts = new Dictionary<IEventContext, bool>();
        }

        public ILogEvent ReferenceLogEvent { get; set; }

        public TimeStampFormat TimeFormat
        {
            get { return timeFormat; }
            set
            {
                timeFormat = value;
                formatChangedListener.OnChange();
            }
        }

        public bool ShowContextDepth
        {
            set
            {
                showContextDepth = true;
                formatChangedListener.OnChange();
            }
            get { return showContextDepth; }
        }

        public void HideMessages(string message)
        {
            HiddenMessages.Add(message);
            formatChangedListener.OnChange();
        }

        public void HideEventsInExactContext(IEventContext context)
        {
            if (!HiddenEventsInExactContexts.ContainsKey(context))
            {
                HiddenEventsInExactContexts.Add(context, true);
                OnFilterChanged();
            }
        }

        private void OnFilterChanged()
        {
            hiddenContextsCache.Clear();
            formatChangedListener.OnChange();
        }

        public void HideEventsInContext(IEventContext context)
        {
            if (!HiddenEventsInContexts.ContainsKey(context))
            {
                HiddenEventsInContexts.Add(context, true);
                OnFilterChanged();
            }
        }

        public void ShowAllEvents()
        {
            HiddenMessages.Clear();
            HiddenEventsInExactContexts.Clear();
            HiddenEventsInContexts.Clear();
            hiddenContextsCache.Clear();
        }

        public bool EventIsHidden(ILogEvent logEvent)
        {
            var context = logEvent.Context;
            if (hiddenContextsCache.ContainsKey(context))
            {
                return hiddenContextsCache[context];
            }

            var isHidden = (!ReferenceEquals(logEvent, ReferenceLogEvent)) && HiddenMessages.Contains(logEvent.Message) ||
                   HiddenEventsInExactContexts.ContainsKey(context);

            if (!isHidden)
            {
                foreach (var hiddenEventsContextPair in HiddenEventsInContexts)
                {
                    if (hiddenEventsContextPair.Key.IsEqualOrParentOf(context))
                    {
                        isHidden = true;
                        break;
                    }
                }
            }

            hiddenContextsCache.Add(context, isHidden);

            return isHidden;
        }

        private List<string> HiddenMessages { get; set; }
        private Dictionary<IEventContext, bool> HiddenEventsInExactContexts { get; set; }
        private Dictionary<IEventContext, bool> HiddenEventsInContexts { get; set; }
    }
}