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

using System;
using System.Collections.Generic;
using System.Threading;
using NLog;

namespace NoeticTools.nLogCruncher.Tests.NLogSender
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var events = new List<PeriodicEvent>
                {
                    new PeriodicEvent(TimeSpan.FromSeconds(5), LogLevel.Debug,
                                      "NoeticTools.Tests1.PeriodEvents",
                                      "5 second message."),
                    new PeriodicEvent(TimeSpan.FromSeconds(3), LogLevel.Trace,
                                      "NoeticTools.Tests2.PeriodEvents", "3 second message."),
                    new PeriodicEvent(TimeSpan.FromSeconds(4), LogLevel.Trace,
                                      "NoeticTools.Tests2", "4 second message.")
                };

            while (true)
            {
                Thread.Sleep(300);

                foreach (var periodicEvent in events)
                {
                    periodicEvent.Tick();
                }

                LogManager.Flush();
            }
        }
    }
}