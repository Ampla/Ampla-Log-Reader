namespace Ampla.LogReader.FileSystem
{
    public static class AmplaTestProjects
    {
         public static AmplaProject GetAmplaProject()
         {
             return new AmplaProject()
                 {
                     ProjectName = "AmplaProject",
                     Directory = @".\Resources\AmplaProjects\AmplaProject"
                 };
         }

         public static AmplaProject GetWcfOnlyProject()
         {
             return new AmplaProject()
             {
                 ProjectName = "AmplaProject",
                 Directory = @".\Resources\AmplaProjects\WCFOnly"
             };
         }
    }
}