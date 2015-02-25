using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace Ampla.LogReader
{
    [TestFixture]
    public class ThreadCultureUnitTests : TestFixture
    {
        [Test]
        public void SetUICulture()
        {
            CultureInfo inside;
            CultureInfo before = Thread.CurrentThread.CurrentUICulture;
            
            using (ThreadCulture.SetUICulture("fr-FR"))
            {
                inside = Thread.CurrentThread.CurrentUICulture;
            }

            CultureInfo after = Thread.CurrentThread.CurrentUICulture;

            Assert.That(inside.Name, Is.EqualTo("fr-FR"), "Not set");
            Assert.That(before.Name, Is.EqualTo(after.Name), "Not reset back");
        }

        [Test]
        public void SetUICultureLayered()
        {
            CultureInfo inside;
            CultureInfo inner;
            CultureInfo outside;
            CultureInfo before = Thread.CurrentThread.CurrentUICulture;

            using (ThreadCulture.SetUICulture("fr-FR"))
            {
                inside = Thread.CurrentThread.CurrentUICulture;
                using (ThreadCulture.SetUICulture("de-DE"))
                {
                    inner = Thread.CurrentThread.CurrentUICulture;
                }
                outside = Thread.CurrentThread.CurrentUICulture;
            }

            CultureInfo after = Thread.CurrentThread.CurrentUICulture;

            Assert.That(inside.Name, Is.EqualTo("fr-FR"), "Not set");
            Assert.That(inner.Name, Is.EqualTo("de-DE"), "Inner not set");
            Assert.That(inside.Name, Is.EqualTo(outside.Name), "Not reset back");
            Assert.That(before.Name, Is.EqualTo(after.Name), "Not reset back");
        }
    }
}