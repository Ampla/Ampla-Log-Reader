using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class WcfLogReaderUnitTests : TestFixture
    {
        [Test]
        public void LoadSingleEntry()
        {
            WcfLogReader reader = new WcfLogReader(@".\Wcf\Resources\SingleEntry.log");
            Assert.That(reader.WcfCalls, Is.Empty);
            reader.Read();

            Assert.That(reader.WcfCalls, Is.Not.Empty);
        }

        [Test]
        public void LoadErrorEntry()
        {
            WcfLogReader reader = new WcfLogReader(@".\Wcf\Resources\ErrorEntry.log");
            Assert.That(reader.WcfCalls, Is.Empty);
            reader.Read();

            Assert.That(reader.WcfCalls, Is.Not.Empty);
        }
    }
}