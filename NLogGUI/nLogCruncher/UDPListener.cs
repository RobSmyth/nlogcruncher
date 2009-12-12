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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace NoeticTools.nLogCruncher
{
    public class UDPListener
    {
        private readonly TimeSpan timeReadingLimit = TimeSpan.FromSeconds(3);
        private string output = "";
        private bool stop;
        private Thread thread;

        public void Start(IMessageQueue messageQueue)
        {
            thread = new Thread(ThreadProc);
            thread.Start(messageQueue);
        }

        public void Stop()
        {
            stop = true;
            thread.Join(TimeSpan.FromSeconds(1));
            if (thread.IsAlive)
            {
                thread.Abort();
            }
        }

        private void ThreadProc(object data)
        {
            var messageQueue = (IMessageQueue) data;

            var receivingUdpClient = new UdpClient(4000);
            var RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (!stop)
                {
                    var timeReceiving = new Stopwatch();
                    timeReceiving.Start();
                    while (receivingUdpClient.Available > 0 && timeReceiving.Elapsed < timeReadingLimit)
                    {
                        var receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                        var message = Encoding.ASCII.GetString(receiveBytes);
                        messageQueue.Enqueue(message);
                    }
                    timeReceiving.Stop();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}