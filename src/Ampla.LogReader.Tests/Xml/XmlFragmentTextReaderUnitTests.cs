using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.Xml
{
    [TestFixture]
    public class XmlFragmentTextReaderUnitTests : TestFixture
    {
        [Test]
        public void Empty()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", new StringReader(""));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<Xml></Xml>"));
        }

        [Test]
        public void StringNode()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", new StringReader("text"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<Xml>text</Xml>"));
        }

        [Test]
        public void WithNodes()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", new StringReader("<One>Two</One>"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<Xml><One>Two</One></Xml>"));
        }

        [Test]
        public void RootName()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("root", new StringReader("<One>Two</One>"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<root><One>Two</One></root>"));
        }

        [Test]
        public void Read2CharsAtATime()
        {
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", new StringReader("One"));

            string result = "";
            int read = 1;
            while (read > 0)
            {
                char[] buffer = new char[2];
                read = reader.Read(buffer, 0, 2);
                result += new string(buffer, 0, read);
            }
            Assert.That(result, Is.EqualTo("<Xml>One</Xml>"));
        }
    }
}