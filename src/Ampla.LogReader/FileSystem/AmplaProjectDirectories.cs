using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ampla.LogReader.FileSystem
{
    public class AmplaProjectDirectories
    {
        private readonly AmplaProject[] projects;

        public AmplaProjectDirectories(string directory)
        {
            List<AmplaProject> projectList = new List<AmplaProject>();
            if (Directory.Exists(directory))
            {
                string projectsDir = Path.Combine(directory, "Citect", "Ampla", "Projects");

                if (Directory.Exists(projectsDir))
                {
                    IEnumerable<DirectoryInfo> amplaDirectories = new DirectoryInfo(projectsDir).EnumerateDirectories();
                    projectList.AddRange(amplaDirectories.Select(amplaDirectory => new AmplaProject
                        {
                            ProjectName = amplaDirectory.Name, Directory = amplaDirectory.FullName
                        }));
                }
            }

            projects = projectList.ToArray();
        }

        public AmplaProjectDirectories() : this(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))
        {
        }

        public AmplaProject[] Projects
        {
            get { return projects; }
        }
    }
}