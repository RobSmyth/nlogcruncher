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
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI
{
    public class EventsFormatterData : IEventsFormatterData
    {
        private readonly List<IEventContext> _cachedContexts = new List<IEventContext>();
        private readonly IEventListener<FormatChanged> _formatChangedListener;
        private readonly List<IEventContext> _hiddenContextsCache = new List<IEventContext>();
        private bool _showContextDepth;
        private TimeStampFormat _timeFormat;

        public EventsFormatterData(IEventListener<FormatChanged> formatChangedListener)
        {
            _formatChangedListener = formatChangedListener;
            _timeFormat = TimeStampFormat.Absolute;
            HiddenMessages = new List<string>();
        }

        private List<string> HiddenMessages { get; set; }

        public ILogEvent ReferenceLogEvent { get; set; }

        public TimeStampFormat TimeFormat
        {
            get { return _timeFormat; }
            set
            {
                _timeFormat = value;
                _formatChangedListener.OnChange();
            }
        }

        public bool ShowContextDepth
        {
            set
            {
                _showContextDepth = true;
                _formatChangedListener.OnChange();
            }
            get { return _showContextDepth; }
        }

        public void HideMessages(string message)
        {
            HiddenMessages.Add(message);
            _formatChangedListener.OnChange();
        }

        public void HideEventsInExactContext(IEventContext context)
        {
            OnFilterChanged();
            context.ShowEvents = ShowEvents.HideExact;
            _cachedContexts.Add(context);
        }

        public void HideEventsInContext(IEventContext context)
        {
            OnFilterChanged();
            context.ShowEvents = ShowEvents.HideThisAndChildren;
            _cachedContexts.Add(context);
        }

        public void ShowAllEvents()
        {
            HiddenMessages.Clear();
            ClearCache();
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
                foreach (var eventContext in _cachedContexts)
                {
                    if (eventContext.ShowEvents == ShowEvents.HideThisAndChildren)
                    {
                        if (eventContext.IsEqualOrParentOf(context))
                        {
                            isHidden = true;
                            context.ShowEvents = ShowEvents.HideThisAndChildren;
                            break;
                        }
                    }
                }
            }

            if (context.ShowEvents != ShowEvents.Yes)
            {
                if (!_cachedContexts.Contains(context))
                {
                    _cachedContexts.Add(context);
                }

                if (!_hiddenContextsCache.Contains(context))
                {
                    _hiddenContextsCache.Add(context);
                }
            }

            return isHidden;
        }

        private void ClearCache()
        {
            foreach (var context in _cachedContexts)
            {
                context.ShowEvents = ShowEvents.Unknown;
            }
            foreach (var context in _hiddenContextsCache)
            {
                context.ShowEvents = ShowEvents.Unknown;
            }
            _hiddenContextsCache.Clear();
        }

        private void OnFilterChanged()
        {
            _formatChangedListener.OnChange();
        }
    }
}