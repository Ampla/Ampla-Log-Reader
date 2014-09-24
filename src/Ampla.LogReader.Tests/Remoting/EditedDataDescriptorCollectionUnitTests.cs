using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class EditedDataDescriptorCollectionUnitTests : TestFixture
    {
        private const string qualityXml =
            "<?xml version='1.0' encoding='utf-8'?>" +
            "<EditedDataDescriptorCollection Location='Enterprise.Site.Area.Quality' Comment='' SetId='5' UserName='System Configuration.Users.Administrator' SampleTimeUTC='1/01/0001 12:00:00 AM'>" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='-1' dataStatus='0' editedValue='11/06/2014 3:48:03 AM' isNewData='False' name='LastModified' setId='5' />" +
            "<EditedDataDescriptor comment='' baseTable='Set' dataId='5' dataStatus='0' editedValue='11/06/2014 3:47:09 AM' isNewData='False' name='SampleDateTime' setId='5' />" +
            "</EditedDataDescriptorCollection>";

        private const string downtimeXml =
            "<?xml version='1.0' encoding='utf-8'?>" +
            "<EditedDataDescriptorCollection Location='Enterprise.Site.Area.Downtime' Comment='' SetId='2' UserName='System Configuration.Users.Administrator' SampleTimeUTC='1/01/0001 12:00:00 AM'>" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='-1' dataStatus='0' editedValue='10/10/2013 1:57:45 AM' isNewData='False' name='LastModified' setId='2' />" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='2' dataStatus='0' editedValue='9020' isNewData='False' name='Cause' setId='2' />" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='2' dataStatus='0' editedValue='Enterprise.Site.Area' isNewData='False' name='Cause Location' setId='2' />" +
            "</EditedDataDescriptorCollection>";

        private const string deleteXml =
            "<?xml version='1.0' encoding='utf-8'?>" +
            "<EditedDataDescriptorCollection Location='Enterprise.Site.Area.Maintenance' Comment='Grid delete' SetId='3' UserName='System Configuration.Users.Administrator' SampleTimeUTC='1/01/0001 12:00:00 AM'>" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='-1' dataStatus='0' editedValue='14/10/2013 5:26:08 AM' isNewData='False' name='LastModified' setId='3' />" +
            "<EditedDataDescriptor comment='' baseTable='Set' dataId='3' dataStatus='0' editedValue='True' isNewData='False' name='IsDeleted' setId='3' />" +
            "</EditedDataDescriptorCollection>";

        private const string confirmXml =
            "<?xml version='1.0' encoding='utf-8'?>" +
            "<EditedDataDescriptorCollection Location='Enterprise.Site.Area.Production' Comment='Grid confirmation' SetId='2' UserName='System Configuration.Users.Administrator' SampleTimeUTC='1/01/0001 12:00:00 AM'>" +
            "<EditedDataDescriptor comment='' baseTable='Set' dataId='2' dataStatus='0' editedValue='True' isNewData='False' name='IsConfirmed' setId='2' />" +
            "<EditedDataDescriptor comment='' baseTable='Field' dataId='-1' dataStatus='0' editedValue='15/10/2013 3:51:45 AM' isNewData='False' name='LastModified' setId='2' />" +
            "<EditedDataDescriptor comment='' baseTable='Set' dataId='2' dataStatus='0' editedValue='System Configuration.Users.Administrator' isNewData='False' name='ConfirmedBy' setId='2' />" +
            "<EditedDataDescriptor comment='' baseTable='Set' dataId='2' dataStatus='0' editedValue='15/10/2013 3:51:45 AM' isNewData='False' name='ConfirmedDateTime' setId='2' />" +
            "</EditedDataDescriptorCollection>";

        [Test]
        public void QualityLocation()
        {
            EditedDataDescriptorCollection collection = new EditedDataDescriptorCollection(qualityXml);
            Assert.That(collection.Location, Is.EqualTo("Enterprise.Site.Area.Quality"));
            Assert.That(collection.SetId, Is.EqualTo(5));
            Assert.That(collection.FieldValues, Is.EqualTo("Id={5}, LastModified={11/06/2014 3:48:03 AM}, SampleDateTime={11/06/2014 3:47:09 AM}"));
            Assert.That(collection.Operation, Is.EqualTo("Update Record"));
        }

        [Test]
        public void DowntimeLocation()
        {
            EditedDataDescriptorCollection collection = new EditedDataDescriptorCollection(downtimeXml);
            Assert.That(collection.Location, Is.EqualTo("Enterprise.Site.Area.Downtime"));
            Assert.That(collection.SetId, Is.EqualTo(2));
            Assert.That(collection.FieldValues, Is.EqualTo("Id={2}, LastModified={10/10/2013 1:57:45 AM}, Cause={9020}, Cause Location={Enterprise.Site.Area}"));
            Assert.That(collection.Operation, Is.EqualTo("Update Record"));
        }

        [Test]
        public void DeleteRecord()
        {
            EditedDataDescriptorCollection collection = new EditedDataDescriptorCollection(deleteXml);
            Assert.That(collection.Location, Is.EqualTo("Enterprise.Site.Area.Maintenance"));
            Assert.That(collection.SetId, Is.EqualTo(3));
            Assert.That(collection.FieldValues, Is.EqualTo("Id={3}, LastModified={14/10/2013 5:26:08 AM}, IsDeleted={True}"));
            Assert.That(collection.Operation, Is.EqualTo("Delete Record"));
        }

        [Test]
        public void ConfirmRecord()
        {
            EditedDataDescriptorCollection collection = new EditedDataDescriptorCollection(confirmXml);
            Assert.That(collection.Location, Is.EqualTo("Enterprise.Site.Area.Production"));
            Assert.That(collection.SetId, Is.EqualTo(2));
            Assert.That(collection.FieldValues, Is.EqualTo("Id={2}, IsConfirmed={True}, LastModified={15/10/2013 3:51:45 AM}, ConfirmedBy={System Configuration.Users.Administrator}, ConfirmedDateTime={15/10/2013 3:51:45 AM}"));
            Assert.That(collection.Operation, Is.EqualTo("Confirm Record"));
        }


    }
}