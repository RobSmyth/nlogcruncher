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
using System.Diagnostics;
using NLog;


namespace NoeticTools.Tests.NLogSender
{
    internal class PeriodicEvent
    {
        private readonly string context;
        private readonly Stopwatch eventStopwatch = new Stopwatch();
        private readonly Logger logger;
        private readonly LogLevel logLevel;
        private readonly string message;
        private readonly TimeSpan period;

        public PeriodicEvent(TimeSpan period, LogLevel logLevel, string context, string message)
        {
            this.period = period;
            this.logLevel = logLevel;
            this.context = context;
            this.message = message;

            logger = LogManager.GetLogger(context);
            eventStopwatch.Start();
        }

        public void Tick()
        {
            if (eventStopwatch.Elapsed >= period)
            {
                eventStopwatch.Reset();
                eventStopwatch.Start();
                Console.WriteLine(message);
                logger.Log(logLevel, message);
            }
        }
    }
}