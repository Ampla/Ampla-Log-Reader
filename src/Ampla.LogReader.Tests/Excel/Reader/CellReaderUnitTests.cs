using System;
using NUnit.Framework;

namespace Ampla.LogReader.Excel.Reader
{
    [TestFixture]
    public class CellReaderUnitTests : ExcelTestFixture
    {
        [Test]
        public void NullConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new CellReader(null));
        }
    }
}