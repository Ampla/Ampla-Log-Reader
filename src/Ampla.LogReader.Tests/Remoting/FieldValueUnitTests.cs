using System;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class FieldValueUnitTests : TestFixture
    {
        [Test]
        public void Constructor()
        {
            FieldValue fieldValue = new FieldValue("Field", "100");
            Assert.That(fieldValue.Name, Is.EqualTo("Field"));
            Assert.That(fieldValue.Value, Is.EqualTo("100"));
            Assert.That(fieldValue.NameValue, Is.EqualTo("Field={100}"));
        }

        [Test]
        public void Parse()
        {
            FieldValue fieldValue = FieldValue.Parse("Field={100}");
            Assert.That(fieldValue.Name, Is.EqualTo("Field"));
            Assert.That(fieldValue.Value, Is.EqualTo("100"));
            Assert.That(fieldValue.NameValue, Is.EqualTo("Field={100}"));
        }
    }
}