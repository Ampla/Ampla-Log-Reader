namespace Ampla.LogReader.Remoting
{
    public interface ILocationParameter
    {
        string Operation { get; }
        string Location { get; }
        string Module { get; }
        string MetaData { get; }
    }
}