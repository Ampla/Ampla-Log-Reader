using System.IO;
using System.Xml;
using Ampla.LogReader.Xml;
using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class GetDataParameterUnitTests : TestFixture
    {
        private XmlNode xmlNode;
        private const string getDataFileName = @".\Wcf\Resources\GetData.log";
        private const string getDataUserFileName = @".\Wcf\Resources\GetDataUser.log";

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
        public void GetDataSession()
        {
            LoadXmlFile(getDataFileName);
            WcfCall wcfCall = WcfCall.LoadFromXml(xmlNode);
            Assert.That(wcfCall.Method, Is.EqualTo("GetData"));

            IWcfLocationParameter parameter = new GetDataParameter(wcfCall);
            Assert.That(parameter, Is.Not.Null);
            Assert.That(parameter.Module, Is.EqualTo("Production"));
            Assert.That(parameter.Location, Is.EqualTo("Enterprise with recurse"));
            Assert.That(parameter.Operation, Is.EqualTo("GetData"));
            Assert.That(parameter.MetaData, Is.EqualTo("Id={2}"));
            Assert.That(parameter.Credentials, Is.EqualTo("Session: 8bca9a70-627b-4a14-a2f6-5ec051a94953"));
        }

        [Test]
        public void GetDataUser()
        {
            LoadXmlFile(getDataUserFileName);
            WcfCall wcfCall = WcfCall.LoadFromXml(xmlNode);
            Assert.That(wcfCall.Method, Is.EqualTo("GetData"));

            IWcfLocationParameter parameter = new GetDataParameter(wcfCall);
            Assert.That(parameter, Is.Not.Null);
            Assert.That(parameter.Module, Is.EqualTo("Quality"));
            Assert.That(parameter.Location, Is.EqualTo("Enterprise.Site.Area.Quality"));
            Assert.That(parameter.Operation, Is.EqualTo("GetData"));
            Assert.That(parameter.MetaData, Is.EqualTo("Deleted={False}"));
            Assert.That(parameter.Credentials, Is.EqualTo("User: User"));
        }
    }
}