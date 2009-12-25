namespace NoeticTools.nLogCruncher.Domain
{
    public interface ILogSets
    {
        ILogSet[] GetSetsFor(ILogEvent logEvent);
    }
}