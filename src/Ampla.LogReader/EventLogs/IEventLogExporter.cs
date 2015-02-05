namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogExporter
    {
        string Export(string eventLog, string fileName);
    }
}