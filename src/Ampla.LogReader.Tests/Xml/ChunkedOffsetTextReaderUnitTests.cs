using System;
using NUnit.Framework;

namespace Ampla.LogReader.Xml
{
    [TestFixture]
    public class ChunkedOffsetTextReaderUnitTests : TestFixture
    {
        [Test]
        public void ZeroOffset()
        {
            ChunkedOffsetTextReader reader = new ChunkedOffsetTextReader(0, new [] {"ABC", "DEF", "GHI"});
            string read = reader.ReadToEnd();
            Assert.That(read, Is.EqualTo("ABC\r\nDEF\r\nGHI"));

            Assert.That(reader.Offset, Is.EqualTo(0));
            Assert.That(reader.Length, Is.EqualTo(13));
        }

        [Test]
        public void PastOffset()
        {
            ChunkedOffsetTextReader reader = new ChunkedOffsetTextReader(100, new[] { "ABC", "DEF", "GHI" });
            string read = reader.ReadToEnd();
            Assert.That(read, Is.EqualTo("ABC\r\nDEF\r\nGHI"));
            Assert.That(reader.Offset, Is.EqualTo(100));
            Assert.That(reader.Length, Is.EqualTo(13));
        }

        [Test]
        public void OneOffset()
        {
            ChunkedOffsetTextReader reader = new ChunkedOffsetTextReader(1, new[] { "ABC", "DEF", "GHI" });
            string read = reader.ReadToEnd();
            Assert.That(read, Is.EqualTo("ABC\r\nDEF\r\nGHI"));
            Assert.That(reader.Offset, Is.EqualTo(1));
            Assert.That(reader.Length, Is.EqualTo(13));
        }

        [Test]
        public void TwoOffset()
        {
            ChunkedOffsetTextReader reader = new ChunkedOffsetTextReader(2, new[] { "ABC", "DEF", "GHI" });
            string read = reader.ReadToEnd();
            Assert.That(read, Is.EqualTo("ABC\r\nDEF\r\nGHI"));
            Assert.That(reader.Offset, Is.EqualTo(2));
            Assert.That(reader.Length, Is.EqualTo(13));
        }

        [Test]
        public void FourOffset()
        {
            ChunkedOffsetTextReader reader = new ChunkedOffsetTextReader(4, new[] { "ABC", "DEF", "GHI" });
            string read = reader.ReadToEnd();
            Assert.That(read, Is.EqualTo("ABC\r\nDEF\r\nGHI"));
            Assert.That(reader.Offset, Is.EqualTo(4));
            Assert.That(reader.Length, Is.EqualTo(13));
        }

    }
}