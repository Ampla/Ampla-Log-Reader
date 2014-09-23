
namespace Ampla.LogReader.Remoting
{
    public class QueryParameters : ILocationParameter
    {
        public QueryParameters(RemotingEntry remotingEntry)
        {
            if (remotingEntry.Method == "Query")
            {
                if (remotingEntry.Arguments.Length > 3)
                {
                    // ViewDescriptor
                    RemotingArgument view = remotingEntry.Arguments[1];
                    if (view.TypeName == "Citect.Ampla.General.Common.ViewDescriptor")
                    {
                        ViewDescriptor viewDescriptor = new ViewDescriptor(view.Value);
                        Module = viewDescriptor.Module;
                    }
                    // FilterValues
                    RemotingArgument filter = remotingEntry.Arguments[2];
                    if (filter.TypeName == "Citect.Ampla.General.Common.FilterValues")
                    {
                        FilterValues filterValues = new FilterValues(filter.Value);
                        Location = filterValues.Location;
                        MetaData = filterValues.FilterData;
                    }
                }
            }
        }

        public string Operation
        {
            get { return "Query"; }
        }

        public string Location { get; private set; }

        public string Module { get; private set; }

        public string MetaData { get; private set; }

    }
}