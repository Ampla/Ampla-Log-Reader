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
            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
        }

        [Test]
        public void LoadErrorEntry()
        {
            ILogReader<WcfCall> reader = new WcfLogReader(@".\Wcf\Resources\ErrorEntry.log");
            Assert.That(reader.Entries, Is.Empty);
            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
        }
    }
}