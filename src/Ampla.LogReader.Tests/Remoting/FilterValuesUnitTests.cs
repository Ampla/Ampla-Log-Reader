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
            Assert.That(filter.FilterData, Is.EqualTo(null));
        }

        [Test]
        public void WithRecurse()
        {
            FilterValues filter = new FilterValues("@GroupBy={Hour}, Location={\"Enterprise\" with recurse}, Sample Period={Current Day}");
            Assert.That(filter.Location, Is.EqualTo("\"Enterprise\" with recurse"));
            Assert.That(filter.FilterData, Is.EqualTo("@GroupBy={Hour}, Sample Period={Current Day}"));
        }

        [Test]
        public void LocationWithQuotes()
        {
            FilterValues filter = new FilterValues("Location={\"Enterprise.Site.Area\"}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
            Assert.That(filter.FilterData, Is.EqualTo(null));
        }

        [Test]
        public void LocationWithGroupBy()
        {
            FilterValues filter = new FilterValues("@GroupBy={Day}, Location={\"Enterprise.Site.Area\"}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
            Assert.That(filter.FilterData, Is.EqualTo("@GroupBy={Day}"));
        }

        [Test]
        public void LocationWithGroupByAndFilter()
        {
            FilterValues filter = new FilterValues("@GroupBy={Day}, Location={\"Enterprise.Site.Area\"}, Value={100}");
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
            Assert.That(filter.FilterData, Is.EqualTo("@GroupBy={Day}, Value={100}"));
        }

        [Test]
        public void NoLocation()
        {
            FilterValues filter = new FilterValues("Id={7265655}");
            Assert.That(filter.Location, Is.EqualTo("Unknown"));
            Assert.That(filter.FilterData, Is.EqualTo("Id={7265655}"));
        }

        [Test]
        public void EmptyFilter()
        {
            FilterValues filter = new FilterValues("");
            Assert.That(filter.Location, Is.EqualTo(null));
            Assert.That(filter.FilterData, Is.EqualTo(null));
        }

        [Test]
        public void NullFilter()
        {
            FilterValues filter = new FilterValues(null);
            Assert.That(filter.Location, Is.EqualTo(null));
            Assert.That(filter.FilterData, Is.EqualTo(null));
        }

        [Test]
        public void LongDateTime()
        {
            const string filterString = "@GroupBy={Day}, Location={\"Enterprise.Site.Area\"}, Value={100}, Sample Period={>= \"Saturday, 19 July 2014 00:00:00\" And < \"Monday, 21 July 2014 12:00:00\"}";
            FilterValues filter = new FilterValues(filterString);
            Assert.That(filter.Location, Is.EqualTo("Enterprise.Site.Area"));
            Assert.That(filter.FilterData, Is.EqualTo("@GroupBy={Day}, Value={100}, Sample Period={>= \"Saturday, 19 July 2014 00:00:00\" And < \"Monday, 21 July 2014 12:00:00\"}"));
        }

    }
}