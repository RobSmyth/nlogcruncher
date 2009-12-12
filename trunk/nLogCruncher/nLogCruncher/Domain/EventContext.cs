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
using System.Collections.ObjectModel;
using System.Linq;


namespace NoeticTools.nLogCruncher.Domain
{
    public class EventContext : IEventContext, IEquatable<IEventContext>
    {
        private readonly IEventContext parent;

        public EventContext(string name, IEventContext parent)
        {
            this.parent = parent;
            Children = new ObservableCollection<IEventContext>();
            Name = name;
        }

        public string Name { get; private set; }
        public ObservableCollection<IEventContext> Children { get; private set; }

        public string FullName
        {
            get
            {
                var prefix = parent != null ? parent.FullName + "." : string.Empty;
                return (prefix + Name).Trim('.');
            }
        }

        public IEventContext GetContext(string name)
        {
            name = name.Trim();

            if (name == string.Empty)
            {
                return this;
            }

            IEventContext context;
            if (Children.Count(thisEvent => thisEvent.Name == name) == 0)
            {
                context = new EventContext(name.Trim(), this);
                Children.Add(context);
            }
            else
            {
                context = Children.First(thisEvent => thisEvent.Name == name);
            }

            return context;
        }

        public void Clear()
        {
            Children.Clear();
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is IEventContext)
            {
                return Equals((IEventContext) obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(IEventContext other)
        {
            return Name == other.Name;
        }
    }
}