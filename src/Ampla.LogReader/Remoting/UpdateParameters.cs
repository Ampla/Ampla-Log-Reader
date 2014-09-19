namespace Ampla.LogReader.Remoting
{
    public class UpdateParameters
    {
        public UpdateParameters(RemotingEntry remotingEntry)
        {
            if (remotingEntry.Method == "Update")
            {
                if (remotingEntry.Arguments.Length == 3)
                {
                    // ViewDescriptor (string)
                    RemotingArgument view = remotingEntry.Arguments[1];
                    if (view.TypeName == "Citect.Ampla.General.Common.ViewDescriptor")
                    {
                        ViewDescriptor viewDescriptor = new ViewDescriptor(view.Value);
                        Module = viewDescriptor.Module;
                    }
                    // EditedDataDescriptorCollection (xml)
                    RemotingArgument editDataDescriptor = remotingEntry.Arguments[2];
                    if (editDataDescriptor.TypeName == "Citect.Ampla.General.Common.EditedDataDescriptorCollection")
                    {
                        EditedDataDescriptorCollection filterValues = new EditedDataDescriptorCollection(editDataDescriptor.Value);
                        Location = filterValues.Location;
                    }
                }
            }
        }

        public string Location { get; private set; }

        public string Module { get; private set; } 
    }
}