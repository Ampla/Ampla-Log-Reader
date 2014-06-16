using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class RemotingArgumentUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string fileName = @".\Remoting\Resources\EntryWithArgs.log";

        private RemotingArgument remotingArgument;
        private RemotingEntry remotingEntry;

        protected override void OnSetUp()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            xmlNode = xmlDoc.SelectSingleNode("/Xml/RemotingEntry");

            Assert.That(xmlNode, Is.Not.Null);

            remotingEntry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(remotingEntry, Is.Not.Null);
            Assert.That(remotingEntry.Arguments, Is.Not.Null);
            Assert.That(remotingEntry.Arguments, Is.Not.Empty);
            remotingArgument = remotingEntry.Arguments[0];

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
            Assert.That(remotingArgument.Value, Is.EqualTo("d965fab7-f292-41c3-8a7a-2bdf2a369703"));
        }

        [Test]
        public void Arguments()
        {
            Assert.That(remotingEntry.Arguments.Length, Is.EqualTo(4));
            Assert.That(remotingEntry.Arguments[0].Index, Is.EqualTo(1));
            Assert.That(remotingEntry.Arguments[1].Index, Is.EqualTo(2));
            Assert.That(remotingEntry.Arguments[2].Index, Is.EqualTo(3));
            Assert.That(remotingEntry.Arguments[3].Index, Is.EqualTo(4));
            Assert.That(remotingEntry.Arguments[0].Value, Is.Not.Empty);
            Assert.That(remotingEntry.Arguments[1].Value, Is.Not.Empty);
            Assert.That(remotingEntry.Arguments[2].Value, Is.Not.Empty);
            Assert.That(remotingEntry.Arguments[3].Value, Is.Not.Empty);
        }
    }
}