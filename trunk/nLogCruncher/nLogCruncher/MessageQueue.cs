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


namespace NoeticTools.nLogCruncher
{
    internal class MessageQueue : IMessageQueue
    {
        private readonly Queue<string> messageQueue = new Queue<string>();

        public bool HasMessages
        {
            get
            {
                bool hasMesssages;
                lock (messageQueue)
                {
                    hasMesssages = messageQueue.Count > 0;
                }
                return hasMesssages;
            }
        }

        public void Enqueue(string message)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }
        }

        public string[] Dequeue()
        {
            string[] messages;
            lock (messageQueue)
            {
                messages = messageQueue.ToArray();
                messageQueue.Clear();
            }
            return messages;
        }
    }
}