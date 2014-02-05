using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.Xml
{
    [TestFixture]
    public class XmlContentTextReaderUnitTests : TestFixture
    {
        [Test]
        public void Empty()
        {
            XmlContentTextReader reader = new XmlContentTextReader("Content", new StringReader(""));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(""));
        }

        [Test]
        public void String()
        {
            XmlContentTextReader reader = new XmlContentTextReader("text", new StringReader("blah\r\ntext"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("text"));
        }
        
        [Test]
        public void TwoLines()
        {
            XmlContentTextReader reader = new XmlContentTextReader("text", new StringReader("blah\r\ntext\r\nend\r\n"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("text\r\nend\r\n"));
        }


        [Test]
        public void FragmentAndContentReaders()
        {
            XmlContentTextReader reader = new XmlContentTextReader("text", new StringReader("blah\r\ntext"));
            XmlFragmentTextReader fragment = new XmlFragmentTextReader("Xml", reader);
            string xml = fragment.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<Xml>text</Xml>"));
        }

        [Test]
        public void WithNodes()
        {
            XmlContentTextReader reader = new XmlContentTextReader("<One>", new StringReader("blah\r\n<One>\r\n<Two/>\r\n</One>\r\n"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<One>\r\n<Two/>\r\n</One>\r\n"));
        }

        [Test]
        public void RootName()
        {
            XmlContentTextReader reader = new XmlContentTextReader("<One>", new StringReader("<One>\r\nTwo\r\n</One>\r\n"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<One>\r\nTwo\r\n</One>\r\n"));
        }

        [Test]
        public void Read2CharsAtATime()
        {
            XmlContentTextReader reader = new XmlContentTextReader("One", new StringReader("blah\r\nOne\r\nTwo"));

            string result = "";
            int read = 1;
            while (read > 0)
            {
                char[] buffer = new char[2];
                read = reader.Read(buffer, 0, 2);
                result += new string(buffer, 0, read);
            }
            Assert.That(result, Is.EqualTo("One\r\nTwo"));
        }
    }
}