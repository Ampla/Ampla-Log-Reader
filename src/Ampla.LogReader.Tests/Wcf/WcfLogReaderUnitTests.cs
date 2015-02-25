using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class WcfLogReaderUnitTests : TestFixture
    {
        [Test]
        public void LoadSingleEntry()
        {
            ILogReader<WcfCall> reader = new WcfLogReader(@".\Wcf\Resources\SingleEntry.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.ReadAll();

            Assert.That(reader.Entries, Is.Not.Empty);
        }

        [Test]
        public void LoadErrorEntry()
        {
            ILogReader<WcfCall> reader = new WcfLogReader(@".\Wcf\Resources\ErrorEntry.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.ReadAll();

            Assert.That(reader.Entries, Is.Not.Empty);
        }

        [Test]
        public void MultipleReads()
        {
            ILogReader<WcfCall> reader = new WcfLogReader(@".\Wcf\Resources\SingleEntry.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.ReadAll();
            int count = reader.Entries.Count;
            Assert.That(reader.Entries, Is.Not.Empty);

            reader.ReadAll();
            Assert.That(reader.Entries.Count, Is.EqualTo(count));
        }

        [Test]
        public void LoadInvalidEntry()
        {
            ILogReader<WcfCall> reader = new WcfLogReader(@".\Wcf\Resources\Incomplete.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.ReadAll();

            Assert.That(reader.Entries, Is.Not.Empty);
        }
    }
}