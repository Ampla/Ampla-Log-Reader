using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class WcfCallUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string fileName = @".\Wcf\Resources\SingleEntry.log";

        protected override void OnSetUp()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", File.OpenRead(fileName));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            xmlNode = xmlDoc.SelectSingleNode("/Xml/WCFCall");

            Assert.That(xmlNode, Is.Not.Null);
            base.OnSetUp();
        }

        [Test]
        public void LoadCallTime()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            DateTime callTime = XmlHelper.GetDateTime(xmlNode, "CallTime", DateTime.MinValue);
            DateTime expected = DateTime.Parse("2013-04-29T02:45:06.4647667Z", null,
                                               DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

            Assert.That(callTime, Is.GreaterThan(DateTime.MinValue));
            Assert.That(callTime.Kind, Is.EqualTo(DateTimeKind.Utc), "CallTime: {0}", callTime);
            Assert.That(call.CallTime, Is.EqualTo(expected));
            Assert.That(callTime, Is.EqualTo(expected));
        }

        [Test]
        public void LoadUrl()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            string url = XmlHelper.GetValue(xmlNode, "Url", string.Empty);

            Assert.That(url, Is.Not.Empty);
            Assert.That(url, Is.StringContaining("localhost"));
            Assert.That(call.Url, Is.EqualTo(url));
        }

        [Test]
        public void LoadAction()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            string action = XmlHelper.GetValue(xmlNode, "Action", string.Empty);

            Assert.That(action, Is.Not.Empty);
            Assert.That(action, Is.StringContaining("Ampla"));
            Assert.That(call.Action, Is.EqualTo(action));
        }

        [Test]
        public void LoadMethod()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            string method = XmlHelper.GetValue(xmlNode, "Method", "No Method");

            Assert.That(method, Is.Empty);
            Assert.That(call.Method, Is.EqualTo(method));
        }

        [Test]
        public void LoadDuration()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            double duration = XmlHelper.GetValue(xmlNode, "Duration", 0D);

            Assert.That(duration, Is.EqualTo(7.0004D));
            Assert.That(call.Duration, Is.EqualTo(TimeSpan.FromMilliseconds(duration)));
        }

        [Test]
        public void LoadResponseMessageLength()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            double responseMessageLength = XmlHelper.GetValue(xmlNode, "ResponseMessageLength", 0D);

            Assert.That(responseMessageLength, Is.EqualTo(4.45D));
            Assert.That(call.ResponseMessageLength, Is.EqualTo(responseMessageLength));
        }

        [Test]
        public void LoadIsFault()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            bool isFault = XmlHelper.GetValue(xmlNode, "IsFault", true);

            Assert.That(isFault, Is.EqualTo(false));
            Assert.That(call.IsFault, Is.EqualTo(isFault));
        }

        [Test]
        public void LoadFaultMessage()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            string faultMessage = XmlHelper.GetValue(xmlNode, "FaultMessage", "No Message");

            Assert.That(faultMessage, Is.Empty);
            Assert.That(call.FaultMessage, Is.EqualTo(faultMessage));
        }

        [Test]
        public void LoadRequestMessage()
        {
            WcfCall call = WcfCall.LoadFromXml(xmlNode);

            string requestMessage = XmlHelper.GetOuterXml(xmlNode, "RequestMessage");

            Assert.That(requestMessage, Is.Not.Empty);
            Assert.That(requestMessage, Is.StringContaining("http://schemas.xmlsoap.org/soap/envelope/"));
            Assert.That(call.RequestMessage, Is.EqualTo(requestMessage));
        }
    }
}