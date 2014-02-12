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
    }
}