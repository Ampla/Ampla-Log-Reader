using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class ViewDescriptorUnitTests : TestFixture
    {

        [Test]
        public void Downtime()
        {
            ViewDescriptor view = new ViewDescriptor("View Descriptor [Downtime.StandardView]");
            Assert.That(view.Module, Is.EqualTo("Downtime"));
        }

        [Test]
        public void Custom()
        {
            ViewDescriptor view = new ViewDescriptor("View Descriptor [Enterprise.Site.Area.View]");
            Assert.That(view.Module, Is.EqualTo("Unknown"));
        }

        [Test]
        public void Metrics()
        {
            ViewDescriptor view = new ViewDescriptor("View Descriptor [Metrics.AnalysisView]");
            Assert.That(view.Module, Is.EqualTo("Metrics"));
        }

        [Test]
        public void Null()
        {
            ViewDescriptor view = new ViewDescriptor(null);
            Assert.That(view.Module, Is.Null);
        }

        [Test]
        public void Empty()
        {
            ViewDescriptor view = new ViewDescriptor(string.Empty);
            Assert.That(view.Module, Is.Null);
        }
    }
}