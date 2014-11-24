using System;
using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class WcfResponseMessageUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string getDataFileName = @".\Wcf\Resources\GetData.log";

        private void LoadXmlFile(string fileName)
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            xmlNode = xmlDoc.SelectSingleNode("/Xml/WCFCall");
            Assert.That(xmlNode, Is.Not.Null, "Unable to find node\r\n{0}", xmlDoc.OuterXml);
        }

        protected override void OnSetUp()
        {
            LoadXmlFile(getDataFileName);
            base.OnSetUp();
        }

        [Test]
        public void GetNames()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);
            WcfResponseMessage responseMessage = new WcfResponseMessage(call.RequestMessage);

            Assert.That(responseMessage.GetNames(), Is.EquivalentTo(new[] { "RequestMessage" }));
            Assert.That(responseMessage.MoveToNode("//*[name()='GetDataRequestMessage']"), Is.True);
            Assert.That(responseMessage.GetNames(), Is.EquivalentTo(new[] { "GetDataRequest" }));
            Assert.That(responseMessage.MoveToNode("*[name()='GetDataRequest']"), Is.True);
            Assert.That(responseMessage.GetNames(), Is.EquivalentTo(new[] { "Credentials", "View", "Filter", "OutputOptions" }));
        }

        [Test]
        public void MoveToNode()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);
            WcfResponseMessage responseMessage = new WcfResponseMessage(call.RequestMessage);

            Assert.That(responseMessage.GetNames(), Is.Not.Empty);

            Assert.That(responseMessage.GetXmlValue("//*[name()='GetDataRequest']"), Is.Not.Null, "Any text");

            Assert.That(responseMessage.MoveToNode("Invalid/Path"), Is.False);
            Assert.That(responseMessage.MoveToNode("//*[name()='Session']"), Is.True);
            Assert.That(responseMessage.GetXmlValue("."), Is.EqualTo("8bca9a70-627b-4a14-a2f6-5ec051a94953"));
        }
    }
}