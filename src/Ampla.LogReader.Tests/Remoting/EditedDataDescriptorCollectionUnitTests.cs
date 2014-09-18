using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class EditedDataDescriptorCollectionUnitTests : TestFixture
    {
        private const string xml =
            "<?xml version='1.0' encoding='utf-8'?>" +
            "<EditedDataDescriptorCollection Location='Enterprise.Site.Area.Quality' Comment='' SetId='5' UserName='System Configuration.Users.Administrator' SampleTimeUTC='1/01/0001 12:00:00 AM'>" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='-1' dataStatus='0' editedValue='11/06/2014 3:48:03 AM' isNewData='False' name='LastModified' setId='5' />" +
            "<EditedDataDescriptor comment='' baseTable='Set' dataId='5' dataStatus='0' editedValue='11/06/2014 3:47:09 AM' isNewData='False' name='SampleDateTime' setId='5' />" +
            "</EditedDataDescriptorCollection>";

        [Test]
        public void Location()
        {
            EditedDataDescriptorCollection collection = new EditedDataDescriptorCollection(xml);
            Assert.That(collection.Location, Is.EqualTo("Enterprise.Site.Area.Quality"));
        }

    }
}