using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Statistics
{
    [TestFixture]
    public class DateTimeExtensionsUnitTests : TestFixture
    {
        [Test]
        public void LocalTime()
        {
            DateTime now = DateTime.Now;
            Assert.That(now.Kind, Is.EqualTo(DateTimeKind.Local));
            Assert.That(now.TimeOfDay, Is.GreaterThan(TimeSpan.Zero));

            DateTime result = now.TruncateToLocalDay();
            Assert.That(result, Is.EqualTo(now.Subtract(now.TimeOfDay)));
            Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Local));
        }

        [Test]
        public void UtcTime()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime local = utcNow.ToLocalTime();

            Assert.That(utcNow.Kind, Is.EqualTo(DateTimeKind.Utc));
            Assert.That(local.TimeOfDay, Is.GreaterThan(TimeSpan.Zero));

            DateTime result = utcNow.TruncateToLocalDay();
            Assert.That(result, Is.EqualTo(utcNow.Subtract(local.TimeOfDay)));
            Assert.That(result.Kind, Is.EqualTo(DateTimeKind.Utc));
        }
    }
}