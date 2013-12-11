using System.IO;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Render;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TextWriter writer = System.Console.Out;

            AmplaProjectDirectories projectDirectories = new AmplaProjectDirectories();
            foreach (AmplaProject project in projectDirectories.Projects)
            {
                writer.WriteLine("Project: {0}", project.ProjectName);
                writer.WriteLine("Directory: {0}", project.Directory);
                writer.WriteLine("======================");
                WcfLogDirectory directory = new WcfLogDirectory(project.ProjectName);
                directory.Execute();

                WcfErrorStatisticsReport statistics = new WcfErrorStatisticsReport(directory.WcfCalls, writer);
                //WcfErrorReport report = new WcfErrorReport(directory.WcfCalls, writer);
                statistics.Render();


            }

            System.Console.WriteLine("Press any key to continue:");
            System.Console.ReadLine();

        }
    }
}
