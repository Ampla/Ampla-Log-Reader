using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class RemotingArgumentUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string fileName = @".\Remoting\Resources\SingleEntry.log";

        private RemotingArgument remotingArgument;

        protected override void OnSetUp()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            xmlNode = xmlDoc.SelectSingleNode("/Xml/RemotingEntry");

            Assert.That(xmlNode, Is.Not.Null);

            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry, Is.Not.Null);
            Assert.That(entry.Arguments, Is.Not.Null);
            Assert.That(entry.Arguments, Is.Not.Empty);
            remotingArgument = entry.Arguments[0];

            base.OnSetUp();
        }

        [Test]
        public void TypeName()
        {
            Assert.That(remotingArgument.TypeName, Is.EqualTo("System.Guid"));
        }

        [Test]
        public void Index()
        {
            Assert.That(remotingArgument.Index, Is.EqualTo(1));
        }

        [Test]
        public void Value()
        {
            Assert.That(remotingArgument.Value, Is.EqualTo("9f061bbe-a80d-4881-9a8c-1aa4cc4e84e3"));
        }

    }
}