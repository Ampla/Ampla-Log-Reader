using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class QueryParametersUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string availableReportsFileName = @".\Remoting\Resources\SingleEntry.log";
        private const string loginFileName = @".\Remoting\Resources\EntryWithNoArgs.log";
        private const string queryFileName = @".\Remoting\Resources\QueryRecord.log";
        private const string updateFileName = @".\Remoting\Resources\UpdateRecord.log";

        private void LoadXmlFile(string fileName)
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            xmlNode = xmlDoc.SelectSingleNode("/Xml/RemotingEntry");
        }

        protected override void OnSetUp()
        {
            LoadXmlFile(queryFileName);

            Assert.That(xmlNode, Is.Not.Null);
            base.OnSetUp();
        }

        [Test]
        public void QueryDetails()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Query"));

            QueryParameters query = new QueryParameters(entry);
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Module, Is.EqualTo("Downtime"));
            Assert.That(query.Location, Is.Not.Null);
            Assert.That(query.Location, Is.EqualTo("Enterprise"));
            Assert.That(query.MetaData, Is.EqualTo("Sample Period={Current Day}"));
        }

        [Test]
        public void UpdateRecord()
        {
            LoadXmlFile(updateFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Update"));

            QueryParameters query = new QueryParameters(entry);
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Module, Is.Null);
            Assert.That(query.Location, Is.Null);
            Assert.That(query.MetaData, Is.Null);
        }

        [Test]
        public void LoginEntry()
        {
            LoadXmlFile(loginFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Login"));

            QueryParameters query = new QueryParameters(entry);
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Location, Is.Null);
            Assert.That(query.Module, Is.Null);
            Assert.That(query.MetaData, Is.Null);
        }

        [Test]
        public void GetAvailableReports()
        {
            LoadXmlFile(availableReportsFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("GetAvailableReports"));

            QueryParameters query = new QueryParameters(entry);
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Location, Is.Null);
            Assert.That(query.Module, Is.Null);
            Assert.That(query.MetaData, Is.Null);
        }

    }
}