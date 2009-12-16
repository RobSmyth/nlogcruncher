using System;


namespace NoeticTools.nLogCruncher.Domain
{
    public class LoggerEvent : ILogEvent
    {
        public LoggerEvent(string eventDescription, IEventContext context)
        {
            Message = eventDescription;
            Context = context;
            Time = DateTime.Now;
        }

        public string Level { get { return "Info"; } }
        public DateTime Time { get; private set;}
        public string Message { get; private set;}
        public IEventContext Context  { get; private set;}

        public ILogEvent Self
        {
            get { return this; }
        }
    }
}