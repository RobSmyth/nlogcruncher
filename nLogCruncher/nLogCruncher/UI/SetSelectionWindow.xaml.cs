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

using System.Windows;
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI
{
    public partial class SetSelectionWindow : Window
    {
        private readonly ILogSets logSets;

        public SetSelectionWindow()
        {
            InitializeComponent();
        }

        public SetSelectionWindow(ILogSets logSets) : this()
        {
            this.logSets = logSets;
        }

        public ILogSet SelectedSet { get; private set; }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedSet = logSets["A"];
            DialogResult = true;
        }
    }
}