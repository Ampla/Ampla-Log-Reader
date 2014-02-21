using System;
using Ampla.LogReader.Excel.Reader;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Ampla.LogReader.Excel
{
    public class ExcelCell
    {
        private readonly ICellReader cellReader;

        public ExcelCell(ICellReader cellReader)
        {
            this.cellReader = cellReader;
        }

        public bool IsEmpty
        {
            get 
            { 
                string value = GetValue<string>();
                return string.IsNullOrEmpty(value);
            }
            
        }

        public string Address
        {
            get { return cellReader.Address; }
            
        }

        public void AssertValue<T>(IResolveConstraint resolveConstraint)
        {
            T value = GetValue<T>();
            Assert.That(value, resolveConstraint);
        }

        public T GetValue<T>()
        {
            return (T) Convert.ChangeType(cellReader.Value, typeof (T));
        }
    }
}