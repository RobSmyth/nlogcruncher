using System;
using System.Windows.Input;


namespace NoeticTools.nLogCruncher.UI.Commands
{
    public class ShowContextDepthCommand : ICommand
    {
        private readonly IEventsFormatterData data;

        public ShowContextDepthCommand(IEventsFormatterData data)
        {
            this.data = data;
        }

        public void Execute(object parameter)
        {
            data.ShowContextDepth = true;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}