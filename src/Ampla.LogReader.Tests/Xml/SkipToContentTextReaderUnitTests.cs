using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.Xml
{
    [TestFixture]
    public class SkipToContentTextReaderUnitTests : TestFixture
    {
        [Test]
        public void Empty()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("Content", new StringReader(""));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(""));
        }

        [Test]
        public void String()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("text", new StringReader("blah\r\ntext"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("text"));
        }
        
        [Test]
        public void TwoLines()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("text", new StringReader("blah\r\ntext\r\nend\r\n"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("text\r\nend\r\n"));
        }


        [Test]
        public void FragmentAndContentReaders()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("text", new StringReader("blah\r\ntext"));
            XmlFragmentTextReader fragment = new XmlFragmentTextReader("Xml", reader);
            string xml = fragment.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<Xml>text</Xml>"));
        }

        [Test]
        public void WithNodes()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("<One>", new StringReader("blah\r\n<One>\r\n<Two/>\r\n</One>\r\n"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<One>\r\n<Two/>\r\n</One>\r\n"));
        }

        [Test]
        public void ReadLine()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("<One>", new StringReader("blah\r\n<One>\r\n<Two/>\r\n</One>\r\n"));

            List<string> lines = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                lines.Add(reader.ReadLine());
            }

            Assert.That(string.Join("|", lines), Is.EqualTo(string.Join("|", "<One>", "<Two/>", "</One>", "")));
        }

        [Test]
        public void RootName()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("<One>", new StringReader("<One>\r\nTwo\r\n</One>\r\n"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<One>\r\nTwo\r\n</One>\r\n"));
        }

        [Test]
        public void Read2CharsAtATime()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("One", new StringReader("blah\r\nOne\r\nTwo"));

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

        [Test]
        public void Read1CharsAtATime()
        {
            SkipToContentTextReader reader = new SkipToContentTextReader("One", new StringReader("blah\r\nOne\r\nTwo"));

            string result = "";
            int read = 1;
            while (read > 0)
            {
                char[] buffer = new char[1];
                read = reader.Read(buffer, 0, 1);
                result += new string(buffer, 0, read);
            }
            Assert.That(result, Is.EqualTo("One\r\nTwo"));
        }

    }
}