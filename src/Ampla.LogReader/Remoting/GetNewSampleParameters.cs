namespace Ampla.LogReader.Remoting
{
    public class GetNewSampleParameters : ILocationParameter
    {
        public GetNewSampleParameters(RemotingEntry remotingEntry)
        {
            if (remotingEntry.Method == "GetNewSample")
            {
                if (remotingEntry.Arguments.Length == 3)
                {
                    Module = "Unknown";

                    // Location (string) 
                    Location = remotingEntry.Arguments[1].Value;
                    MetaData = null;
                }
            }
        }

        public string Operation
        {
            get { return "New Record"; }
        }

        public string Location { get; private set; }

        public string Module { get; private set; }

        public string MetaData { get; private set; }

    }
}