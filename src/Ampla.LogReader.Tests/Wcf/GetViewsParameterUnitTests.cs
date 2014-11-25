using System.IO;
using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class GetViewsParameterUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string getDataFileName = @".\Wcf\Resources\GetData.log";
        private const string getViewsFileName = @".\Wcf\Resources\GetViews.log";
        private const string submitDataUpdateFileName = @".\Wcf\Resources\SubmitDataUpdate.log";

        private void LoadXmlFile(string fileName)
        {
            Assert.That(File.Exists(fileName), Is.True, fileName);
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
        public void GetViews()
        {
            LoadXmlFile(getViewsFileName);
            WcfCall wcfCall = WcfCall.LoadFromXml(xmlNode);
            Assert.That(wcfCall.Method, Is.EqualTo("GetViews"));

            IWcfLocationParameter parameter = new GetViewsParameter(wcfCall);
            Assert.That(parameter, Is.Not.Null);
            Assert.That(parameter.Module, Is.EqualTo("Quality"));
            Assert.That(parameter.Location, Is.EqualTo("Enterprise"));
            Assert.That(parameter.Operation, Is.EqualTo("GetViews"));
            Assert.That(parameter.MetaData, Is.EqualTo(""));
            Assert.That(parameter.Credentials, Is.EqualTo("Session: 8bca9a70-627b-4a14-a2f6-5ec051a94953"));
        }

        [Test]
        public void GetData()
        {
            LoadXmlFile(getDataFileName);
            WcfCall wcfCall = WcfCall.LoadFromXml(xmlNode);
            Assert.That(wcfCall.Method, Is.EqualTo("GetData"));

            IWcfLocationParameter parameter = new GetViewsParameter(wcfCall);
            Assert.That(parameter, Is.Not.Null);
            Assert.That(parameter.Module, Is.Null);
            Assert.That(parameter.Location, Is.Null);
            Assert.That(parameter.Operation, Is.Null);
            Assert.That(parameter.MetaData, Is.Null);
            Assert.That(parameter.Credentials, Is.Null);
        }

        [Test]
        public void SubmitData()
        {
            LoadXmlFile(submitDataUpdateFileName);
            WcfCall wcfCall = WcfCall.LoadFromXml(xmlNode);
            Assert.That(wcfCall.Method, Is.EqualTo("SubmitData"));

            IWcfLocationParameter parameter = new GetViewsParameter(wcfCall);
            Assert.That(parameter, Is.Not.Null);
            Assert.That(parameter.Module, Is.Null);
            Assert.That(parameter.Location, Is.Null);
            Assert.That(parameter.Operation, Is.Null);
            Assert.That(parameter.MetaData, Is.Null);
            Assert.That(parameter.Credentials, Is.Null);
        }

    }
}