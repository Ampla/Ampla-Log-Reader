using System;
using System.Globalization;
using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class RemotingEntryUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string singleFileName = @".\Remoting\Resources\SingleEntry.log";
        private const string differentTimeZone = @".\Remoting\Resources\DifferentTimeZone.log";

        private void LoadXmlFile(string fileName)
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            xmlNode = xmlDoc.SelectSingleNode("/Xml/RemotingEntry");
        }

        protected override void OnSetUp()
        {
            LoadXmlFile(singleFileName);

            Assert.That(xmlNode, Is.Not.Null);
            base.OnSetUp();
        }

        [Test]
        public void UtcDateTime()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            DateTime callTime = XmlHelper.GetDateTimeUtc(xmlNode, "UTCDateTime", DateTime.MinValue);
            DateTime expected = DateTime.Parse("2014-01-28T01:43:03", null,
                                               DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

            Assert.That(callTime, Is.GreaterThan(DateTime.MinValue));
            Assert.That(callTime.Kind, Is.EqualTo(DateTimeKind.Utc), "UTCDateTime: {0}", callTime);
            Assert.That(entry.CallTimeUtc, Is.EqualTo(expected));
            Assert.That(callTime, Is.EqualTo(expected));

            //Assert.That(entry.CallTimeLocal, Is.EqualTo(entry.CallTimeUtc.ToLocalTime()), "Local time");
        }

        [Test]
        public void LocalDateTime()
        {
            LoadXmlFile(differentTimeZone);

            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            DateTime localTime = XmlHelper.GetDateTimeLocal(xmlNode, "LocalDateTime", DateTime.MinValue);
            DateTime expected = DateTime.Parse("2014-01-29T07:43:03", null,
                                               DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal).ToLocalTime();

            Assert.That(localTime, Is.GreaterThan(DateTime.MinValue));
            Assert.That(localTime.Kind, Is.EqualTo(DateTimeKind.Local), "LocalDateTime: {0}", localTime);
            Assert.That(entry.CallTimeLocal, Is.EqualTo(expected));
            Assert.That(localTime, Is.EqualTo(expected));

            Assert.That(entry.CallTimeLocal, Is.Not.EqualTo(entry.CallTimeUtc.ToLocalTime()), "Local time");
        }
        
        [Test]
        public void Identity()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);
            string identity = XmlHelper.GetValue(xmlNode, "Identity", string.Empty);

            Assert.That(identity, Is.Not.Empty);
            Assert.That(identity, Is.StringContaining("TESTDOMAIN\\sysadmin"));
            Assert.That(entry.Identity, Is.EqualTo(identity));
        }

        [Test]
        public void MethodName()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            string method = XmlHelper.GetValue(xmlNode, "__MethodName", string.Empty);

            Assert.That(method, Is.Not.Empty);
            Assert.That(method, Is.EqualTo("GetAvailableReports"));
            Assert.That(entry.Method, Is.EqualTo(method));
        }

        [Test]
        public void TypeName()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            string typeName = XmlHelper.GetValue(xmlNode, "__TypeName", string.Empty);

            const string truncatedName = "Citect.Ampla.General.Common.IReportingService";

            Assert.That(typeName, Is.Not.Empty);
            Assert.That(typeName, Is.StringStarting(truncatedName));
            Assert.That(entry.TypeName, Is.EqualTo(truncatedName));
        }


        [Test]
        public void Duration()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            double duration = XmlHelper.GetValue(xmlNode, "__MessageResponseTime", 0D);

            Assert.That(duration, Is.EqualTo(93.5856D));
            Assert.That(entry.Duration, Is.EqualTo(TimeSpan.FromMilliseconds(duration)));
        }

        [Test]
        public void ArgumentXml()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            string arguments = XmlHelper.GetInnerXml(xmlNode, "__Args");

            Assert.That(arguments, Is.Not.Empty);
            Assert.That(arguments, Is.StringContaining("System.Guid"));
            Assert.That(entry.ArgumentXml, Is.EqualTo(arguments));
        }

        [Test]
        public void Arguments()
        {
            RemotingEntry entry = RemotingEntry.LoadFromXml(xmlNode);

            string arguments = XmlHelper.GetOuterXml(xmlNode, "__Args");

            Assert.That(arguments, Is.Not.Empty);
            Assert.That(arguments, Is.StringContaining("System.Guid"));
            Assert.That(entry.Arguments.Length, Is.EqualTo(1));

            Assert.That(entry.Arguments[0].TypeName, Is.EqualTo("System.Guid"));
            Assert.That(entry.Arguments[0].Index, Is.EqualTo(1));
        }
    }
}