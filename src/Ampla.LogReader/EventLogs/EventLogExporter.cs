using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public class EventLogExporter : IEventLogExporter
    {
        public string Export(string eventLog, string fileName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "wevtutil.exe",
                    UseShellExecute = true,
                    Arguments = "epl " + eventLog + " " + fileName,
                };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit(5000);
            }

            return fileName;
        }
    }
}