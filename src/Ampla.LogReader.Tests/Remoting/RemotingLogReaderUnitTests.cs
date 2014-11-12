using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class RemotingLogReaderUnitTests : TestFixture
    {
        [Test]
        public void LoadSingleEntry()
        {
            ILogReader<RemotingEntry> reader = new RemotingLogReader(@".\Remoting\Resources\SingleEntry.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
        }

        [Test]
        public void MultipleReads()
        {
            ILogReader<RemotingEntry> reader = new RemotingLogReader(@".\Remoting\Resources\SingleEntry.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.Read();
            int count = reader.Entries.Count;
            Assert.That(reader.Entries, Is.Not.Empty);

            reader.Read();
            Assert.That(reader.Entries.Count, Is.EqualTo(count));
        }

        [Test]
        public void LoadInvalidEntry()
        {
            ILogReader<RemotingEntry> reader = new RemotingLogReader(@".\Remoting\Resources\Incomplete.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
        }
    }
}