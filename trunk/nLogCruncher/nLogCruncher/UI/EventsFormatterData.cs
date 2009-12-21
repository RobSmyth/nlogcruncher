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
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI
{
    public class EventsFormatterData : IEventsFormatterData
    {
        private readonly IEventListener<FormatChanged> formatChangedListener;
        private readonly List<IEventContext> hiddenContextsCache = new List<IEventContext>();
        private bool showContextDepth;
        private TimeStampFormat timeFormat;
        private List<string> HiddenMessages { get; set; }
        private Dictionary<IEventContext, bool> HiddenEventsInContexts { get; set; }
        private readonly List<IEventContext> cachedContexts = new List<IEventContext>();

        public EventsFormatterData(IEventListener<FormatChanged> formatChangedListener)
        {
            this.formatChangedListener = formatChangedListener;
            timeFormat = TimeStampFormat.Absolute;
            HiddenMessages = new List<string>();
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
            OnFilterChanged();
            context.ShowEvents = ShowEvents.HideExact;
        }

        public void HideEventsInContext(IEventContext context)
        {
            if (!HiddenEventsInContexts.ContainsKey(context))
            {
                HiddenEventsInContexts.Add(context, true);
                OnFilterChanged();
                context.ShowEvents = ShowEvents.HideThisAndChildren;
            }
        }

        public void ShowAllEvents()
        {
            HiddenMessages.Clear();
            HiddenEventsInContexts.Clear();
            ClearCache();
        }

        private void ClearCache()
        {
            foreach (var context in cachedContexts)
            {
                context.ShowEvents = ShowEvents.Unknown;
            }
            foreach (var context in hiddenContextsCache)
            {
                context.ShowEvents = ShowEvents.Unknown;
            }
            hiddenContextsCache.Clear();
        }

        public bool EventIsHidden(ILogEvent logEvent)
        {
            var context = logEvent.Context;

            if (context.ShowEvents != ShowEvents.Unknown)
            {
                return context.ShowEvents == ShowEvents.Yes ? false : true;
            }

            var isHidden = (!ReferenceEquals(logEvent, ReferenceLogEvent)) && HiddenMessages.Contains(logEvent.Message);

            if (!isHidden)
            {
                foreach (var hiddenEventsContextPair in HiddenEventsInContexts)
                {
                    if (hiddenEventsContextPair.Key.ShowEvents == ShowEvents.HideThisAndChildren)
                    {
                        isHidden = true;
                        context.ShowEvents = ShowEvents.HideThisAndChildren;
                        break;
                    }
                }
            }

            if (!cachedContexts.Contains(context))
            {
                cachedContexts.Add(context);
            }

            if (!hiddenContextsCache.Contains(context))
            {
                hiddenContextsCache.Add(context);
            }

            return isHidden;
        }

        private void OnFilterChanged()
        {
            formatChangedListener.OnChange();
        }
    }
}