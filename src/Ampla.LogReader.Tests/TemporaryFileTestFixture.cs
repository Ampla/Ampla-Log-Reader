namespace Ampla.LogReader
{
    public abstract class TemporaryFilesTestFixture : TestFixture
    {
        private readonly string fileExtension;

        protected TemporaryFilesTestFixture(string fileExtension)
        {
            this.fileExtension = fileExtension;
        }

        private TempDirectory TempDirectory { get; set; }
        
        protected override void OnFixtureSetUp()
        {
            string directory = @"Files\" + GetType().Name;
            string pattern = "UnitTest_{0:00}." + fileExtension;
            TempDirectory = new TempDirectory(directory, pattern);
            TempDirectory.DeleteAllFiles();
        
            base.OnFixtureSetUp();
        }

        protected string GetNextTemporaryFile()
        {
            return TempDirectory.GetNextTemporaryFile();
        }

        protected string GetSpecificFile(string fileName)
        {
            return TempDirectory.GetSpecificFileName(fileName);
        }

    }
}