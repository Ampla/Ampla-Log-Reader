namespace Ampla.LogReader.EventLogs
{
    public interface ISecurityResolver
    {
        string GetName(string sid);
    }
}