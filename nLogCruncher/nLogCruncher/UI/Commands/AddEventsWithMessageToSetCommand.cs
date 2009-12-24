using System;
using System.Windows.Input;


namespace NoeticTools.nLogCruncher.UI.Commands
{
    public class AddEventsWithMessageToSetCommand : ICommand
    {
        private readonly IEventsFormatterData formatterData;

        public AddEventsWithMessageToSetCommand(IEventsFormatterData formatterData)
        {
            this.formatterData = formatterData;
        }

        public void Execute(object parameter)
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}