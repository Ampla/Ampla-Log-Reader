using System;
using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.Xml
{
    [TestFixture]
    public class TruncatedTextReaderUnitTests : TestFixture
    {
        private StringReader JoinStringReader(params string[] lines)
        {
            return new StringReader(Join(lines));
        }

        private string Join(params string[] lines)
        {
            return string.Join(Environment.NewLine, lines);
        }

        [Test]
        public void Empty()
        {
            TruncatedTextReader reader = new TruncatedTextReader("Content", JoinStringReader(""));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(""));
        }

        [Test]
        public void TwoEmptyLines()
        {
            TruncatedTextReader reader = new TruncatedTextReader("Content", JoinStringReader("",""));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(""));
        }


        [Test]
        public void NoMatch()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("there is no marker"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(""));
        }

        [Test]
        public void String()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("text", "end"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("text", "end")));
        }

        [Test]
        public void TrailingText()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("this is a test", "end", "this is trailing"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("this is a test", "end", "")));
        }

        [Test]
        public void MultipleChunks()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("line 1", "end", "line 2", "end", "line 3", "end", "after", ""));
        
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("line 1", "end", "line 2", "end", "line 3", "end", "")));
        }

        [Test]
        public void TwoLines()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("line 1", "line 2", "end", "after"));

            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("line 1", "line 2", "end", "")));
        }

        [Test]
        public void MultipleMarkers()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("end", "end", "end"));

            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("end", "end", "end")));
        }

        [Test]
        public void FragmentAndContentReaders()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", new StringReader("end"));
            XmlFragmentTextReader fragment = new XmlFragmentTextReader("Xml", reader);
            string xml = fragment.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<Xml>end</Xml>"));
        }

        [Test]
        public void WithNodes()
        {
            TruncatedTextReader reader = new TruncatedTextReader("</One>", JoinStringReader("<One>", "<Two/>", "</One>", "blah"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("<One>", "<Two/>", "</One>", "")));
        }

        [Test]
        public void RootName()
        {
            TruncatedTextReader reader = new TruncatedTextReader("</One>", JoinStringReader("<One>", "Two", "</One>"));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo(Join("<One>", "Two", "</One>")));
        }

        [Test]
        public void Read2CharsAtATime()
        {
            TruncatedTextReader reader = new TruncatedTextReader("end", JoinStringReader("One", "Two", "end"));

            string result = "";
            int read = 1;
            while (read > 0)
            {
                char[] buffer = new char[2];
                read = reader.Read(buffer, 0, 2);
                result += new string(buffer, 0, read);
            }
            Assert.That(result, Is.EqualTo(Join("One", "Two", "end")));
        }

        [Test]
        public void WithSkipToContentReader()
        {
            StringReader stringReader = new StringReader(string.Join("\r\n", "<Blah>", "<One>", "<Two />", "</One>", "<Blah>"));
            TextReader reader = new TruncatedTextReader("</One>", new SkipToContentTextReader("<One>", stringReader));
            string xml = reader.ReadToEnd();
            Assert.That(xml, Is.EqualTo("<One>\r\n<Two />\r\n</One>\r\n"));
        }

    }
}