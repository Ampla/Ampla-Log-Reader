using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class FilterValuesUnitTests : TestFixture
    {
        [Test]
        public void Location()
        {
            FilterValues filter = new FilterValues("Location={Enterprise.Site.Area}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
        }

        [Test]
        public void LocationWithQuotes()
        {
            FilterValues filter = new FilterValues("Location={\"Enterprise.Site.Area\"}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
        }

        [Test]
        public void LocationWithGroupBy()
        {
            FilterValues filter = new FilterValues("Group By={Day},Location={\"Enterprise.Site.Area\"}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
        }

        [Test]
        public void LocationWithGroupByAndFilter()
        {
            FilterValues filter = new FilterValues("Group By={Day},Location={\"Enterprise.Site.Area\"},Value={100}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
        }

    }
}