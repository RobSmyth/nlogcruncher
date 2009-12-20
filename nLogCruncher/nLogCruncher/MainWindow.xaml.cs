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
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using NoeticTools.nLogCruncher.Domain;
using NoeticTools.nLogCruncher.UI;
using NoeticTools.nLogCruncher.UI.Commands;


namespace NoeticTools.nLogCruncher
{
    public partial class MainWindow : Window, IStateListener<EventsLogChanged>, IEventListener<FormatChanged>
    {
        public static readonly ListCollectionView DisplayedLogEvents = new ListCollectionView(EventsLog.LogEvents);
        private readonly EventsFormatterData data;

        public MainWindow()
        {
            data = new EventsFormatterData(this);
            TimeStampConverter = new EventTimestampConverter(data);
            EventMessageConverter = new EventMessageConverter(data);
            SetReferenceEventCommand = new SetReferenceEventCommand(data);
            HideMessageCommand = new HideMessageCommand(data);
            HideEventsInContextCommand = new HideEventsInContextCommand(data);
            ShowAllEventsCommand = new ShowAllEventsCommand(data);
            ShowContextDepthCommand = new ShowContextDepthCommand(data);

            InitializeComponent();

            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;
        }

        public static IValueConverter TimeStampConverter { get; private set; }
        public static IValueConverter EventMessageConverter { get; private set; }
        public static ICommand SetReferenceEventCommand { get; private set; }
        public static ICommand HideMessageCommand { get; private set; }
        public static ICommand HideEventsInContextCommand { get; private set; }
        public static ICommand ShowAllEventsCommand { get; private set; }
        public static ICommand ShowContextDepthCommand { get; private set; }

        void IEventListener<FormatChanged>.OnChange()
        {
            Refresh();
        }

        void IStateListener<EventsLogChanged>.OnChange()
        {
            Refresh();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            eventsDataGrid.DataContext = DisplayedLogEvents;
            eventContextTreeView.SelectedItemChanged += eventContextTreeView_SelectedItemChanged;
            DisplayedLogEvents.Filter = new Predicate<object>(EventFilter);

            EventsLog.StartLogging();
            EventsLog.AddListener(this);
        }

        private static void eventContextTreeView_SelectedItemChanged(object sender,
                                                                  RoutedPropertyChangedEventArgs<object> e)
        {
            Refresh();
        }

        private static void Refresh()
        {
            DisplayedLogEvents.Refresh();
        }

        private bool EventFilter(object obj)
        {
            var logEvent = (ILogEvent) obj;

            if (!LevelFilter(logEvent)) return false;

            if (!ContextFilter(logEvent)) return false;

            return HiddenMessagesFilter(logEvent);
        }

        private bool HiddenMessagesFilter(ILogEvent logEvent)
        {
            return !data.EventIsHidden(logEvent);
        }

        private bool ContextFilter(ILogEvent logEvent)
        {
            var context = (IEventContext) eventContextTreeView.SelectedItem;
            if (context != null)
            {
                return logEvent.Context.FullName.StartsWith(context.FullName);
            }
            return true;
        }

        private bool LevelFilter(ILogEvent logEvent)
        {
            var level = EventsLog.Levels.First(thisLevel => thisLevel.Name.ToLower() == logEvent.Level.ToLower());
            if (!level.IsSelected)
            {
                return false;
            }
            return true;
        }

        private static void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            EventsLog.Stop();
        }

        private void HideContextTreeView_Click(object sender, RoutedEventArgs e)
        {
            var selectedContext = (IEventContext) eventContextTreeView.SelectedItem;
            if (selectedContext != null)
            {
                data.HideEventsInExactContext(selectedContext);
                Refresh();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private static void Clear()
        {
            EventsLog.ClearAll();
            Refresh();
        }

        private void FilterSettingsChanged(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventsLog.Running)
            {
                EventsLog.Stop();
            }
            else
            {
                EventsLog.StartLogging();
            }
        }
    }
}