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

using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using NoeticTools.nLogCruncher.Domain;
using NoeticTools.nLogCruncher.UI;
using NoeticTools.nLogCruncher.UI.Commands;
using NoeticTools.nLogCruncher.UI.Converters;
using NoeticTools.nLogCruncher.Views;


namespace NoeticTools.nLogCruncher
{
    public partial class MainWindow : Window, IStateListener<EventsLogChanged>, IEventListener<FormatChanged>
    {
        public static readonly ListCollectionView DisplayedLogEvents = new ListCollectionView(EventsLog.LogEvents);
        public static readonly ListCollectionView DisplayedLogSets = new ListCollectionView(Domain.LogSets.Sets);
        public static readonly ILogSets LogSets = new LogSets();
        private readonly EventsFormatterData _eventsFormatterData;

        public MainWindow()
        {
            _eventsFormatterData = new EventsFormatterData(this);
            LogSetConverter = new LogSetConverter(LogSets);
            TimeStampConverter = new EventTimestampConverter(_eventsFormatterData);
            EventMessageConverter = new EventMessageConverter(_eventsFormatterData);

            AddEventToSetCommand = new AddEventToSetCommand(LogSets);
            AddEventsWithMessageToSetCommand = new AddEventsWithMessageToSetCommand(_eventsFormatterData);

            SetReferenceEventCommand = new SetReferenceEventCommand(_eventsFormatterData);
            HideEventsWithMessageCommand = new HideEventsWithMessageCommand(_eventsFormatterData);
            HideEventsInContextCommand = new HideEventsInContextCommand(_eventsFormatterData);
            ShowAllEventsCommand = new ShowAllEventsCommand(_eventsFormatterData);
            ShowContextDepthCommand = new ShowContextDepthCommand(_eventsFormatterData);

            InitializeComponent();

            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;
        }

        public static IValueConverter LogSetConverter { get; private set; }
        public static IValueConverter TimeStampConverter { get; private set; }
        public static IValueConverter EventMessageConverter { get; private set; }

        public static ICommand AddEventToSetCommand { get; private set; }
        public static ICommand AddEventsWithMessageToSetCommand { get; private set; }
        public static ICommand SetReferenceEventCommand { get; private set; }
        public static ICommand HideEventsWithMessageCommand { get; private set; }
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
            DisplayedLogEvents.Filter = EventFilter;

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
            return !_eventsFormatterData.EventIsHidden(logEvent);
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
                _eventsFormatterData.HideEventsInContext(selectedContext);
                Refresh();
            }
        }

        private void HideExactContextTreeView_Click(object sender, RoutedEventArgs e)
        {
            var selectedContext = (IEventContext) eventContextTreeView.SelectedItem;
            if (selectedContext != null)
            {
                _eventsFormatterData.HideEventsInExactContext(selectedContext);
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

        private void ShowHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }
    }
}