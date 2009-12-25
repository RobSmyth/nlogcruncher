using System;
using System.Collections.Generic;


namespace NoeticTools.nLogCruncher.Domain
{
    public class LogSets : ILogSets
    {
        private readonly List<ILogSet> sets = new List<ILogSet>();

        public LogSets()
        {
            sets.Add(new LogSet("A"));
        }

        public ILogSet[] GetSetsFor(ILogEvent logEvent)
        {
            var matchingSets = new List<ILogSet>();
            return matchingSets.ToArray();
        }
    }
}