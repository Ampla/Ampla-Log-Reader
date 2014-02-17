using NUnit.Framework;

namespace Ampla.LogReader.Excel
{
    [TestFixture]
    public abstract class ExcelTestFixture : TestFixture
    {
        protected override void OnFixtureSetUp()
        {
            base.OnFixtureSetUp();
            string directory = @"Files\" + GetType().Name;
            TempDirectory = new TempDirectory(directory, "UnitTest_{0:00}.xlsx");
            TempDirectory.DeleteAllFiles();
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Filename = TempDirectory.GetNextTemporaryFile();
        }

        protected string Filename { get; private set; }

        private TempDirectory TempDirectory { get; set; }

    }
}