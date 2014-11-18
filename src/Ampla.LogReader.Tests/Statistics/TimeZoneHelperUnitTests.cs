using System;
using NUnit.Framework;

namespace Ampla.LogReader.Statistics
{
    [TestFixture]
    public class TimeZoneHelperUnitTests : TestFixture
    {
        [Test]
        public void LocalTimeZone()
        {
            Assert.That(TimeZoneHelper.GetTimeZone(), Is.EqualTo(TimeZoneInfo.Local));
        }

        [Test]
        public void GetSpecificTimeZone()
        {
            TimeZoneInfo ist = TimeZoneHelper.GetSpecificTimeZone("India Standard Time");
            Assert.That(ist, Is.Not.Null);
        }
    }
}