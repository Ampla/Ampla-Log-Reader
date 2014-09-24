using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class UpdateParametersUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string availableReportsFileName = @".\Remoting\Resources\SingleEntry.log";
        private const string loginFileName = @".\Remoting\Resources\EntryWithNoArgs.log";
        private const string queryFileName = @".\Remoting\Resources\QueryRecord.log";
        private const string updateFileName = @".\Remoting\Resources\UpdateRecord.log";
        private const string confirmFileName = @".\Remoting\Resources\ConfirmRecord.log";
        private const string newFileName = @".\Remoting\Resources\NewRecord.log";

        private void LoadXmlFile(string fileName)
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            xmlNode = xmlDoc.SelectSingleNode("/Xml/RemotingEntry");
        }

        protected override void OnSetUp()
        {
            LoadXmlFile(updateFileName);

            Assert.That(xmlNode, Is.Not.Null);
            base.OnSetUp();
        }

        [Test]
        public void QueryRecord()
        {
            LoadXmlFile(queryFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Query"));

            UpdateParameters update = new UpdateParameters(entry);
            Assert.That(update, Is.Not.Null);
            Assert.That(update.Module, Is.Null);
            Assert.That(update.Location, Is.Null);
            Assert.That(update.MetaData, Is.Null);
        }

        [Test]
        public void UpdateRecord()
        {
            LoadXmlFile(updateFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Update"));

            UpdateParameters parameters = new UpdateParameters(entry);
            Assert.That(parameters, Is.Not.Null);
            Assert.That(parameters.Module, Is.EqualTo("Quality"));
            Assert.That(parameters.Location, Is.Not.Null);
            Assert.That(parameters.Location, Is.EqualTo("Enterprise.Site.Area.Quality"));
            Assert.That(parameters.MetaData, Is.EqualTo("Id={5}, LastModified={11/06/2014 3:48:03 AM}, SampleDateTime={11/06/2014 3:47:09 AM}"));
        }

        [Test]
        public void LoginEntry()
        {
            LoadXmlFile(loginFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Login"));

            UpdateParameters parameters = new UpdateParameters(entry);
            Assert.That(parameters, Is.Not.Null);
            Assert.That(parameters.Location, Is.Null);
            Assert.That(parameters.Module, Is.Null);
            Assert.That(parameters.MetaData, Is.Null);
        }

        [Test]
        public void GetAvailableReports()
        {
            LoadXmlFile(availableReportsFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("GetAvailableReports"));

            UpdateParameters parameters = new UpdateParameters(entry);
            Assert.That(parameters, Is.Not.Null);
            Assert.That(parameters.Location, Is.Null);
            Assert.That(parameters.Module, Is.Null);
            Assert.That(parameters.MetaData, Is.Null);
        }

        [Test]
        public void NewRecord()
        {
            LoadXmlFile(newFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("GetNewSample"));

            UpdateParameters parameters = new UpdateParameters(entry);
            Assert.That(parameters, Is.Not.Null);
            Assert.That(parameters.Location, Is.Null);
            Assert.That(parameters.Module, Is.Null);
            Assert.That(parameters.MetaData, Is.Null);
            Assert.That(parameters.MetaData, Is.Null);
        }

        [Test]
        public void ConfirmRecord()
        {
            LoadXmlFile(confirmFileName);
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            Assert.That(entry.Method, Is.EqualTo("Update"));

            UpdateParameters query = new UpdateParameters(entry);
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Location, Is.EqualTo("Enterprise.Site.Area.Downtime"));
            Assert.That(query.Module, Is.EqualTo("Downtime"));
            Assert.That(query.MetaData, Is.EqualTo("Id={11}, ConfirmedBy={System Configuration.Users.Administrator}, IsConfirmed={True}, ConfirmedDateTime={7/19/2014 7:46:06 AM}, LastModified={7/19/2014 7:46:17 AM}"));
        }

    }
}