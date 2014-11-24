namespace Ampla.LogReader.Wcf
{
    public interface IWcfLocationParameter
    {
        string Credentials { get; }
        string Operation { get; }
        string Location { get; }
        string Module { get; }
        string MetaData { get; }
    }
}