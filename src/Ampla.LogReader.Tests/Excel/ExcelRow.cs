using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ampla.LogReader.Excel.Reader;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Ampla.LogReader.Excel
{
    public class ExcelRow
    {
        private readonly List<ExcelCell> columns;
        private readonly int row;
        public ExcelRow(IWorksheetReader reader)
        {
            columns = new List<ExcelCell>();

            row = reader.GetCurrentCell().Row;

            ExcelCell cell = new ExcelCell(reader.GetCurrentCell());
            var address = cell.Address;
            while (!cell.IsEmpty)
            {
                columns.Add(cell);
                reader.Read();
                cell = new ExcelCell(reader.GetCurrentCell());
            }

            reader.MoveTo(address);
            reader.ReadRow();
        }

        public ExcelCell Column(int column)
        {
            if (column > 0 && column <= columns.Count)
            {
                ExcelCell cell = columns[column - 1];
                return cell;
            }
            Assert.Fail("Unable to find column {0} in {1}", column, this);
            return null;
        }

        public T GetValueAt<T>(int column)
        {
            if (column > 0 && column <= columns.Count)
            {
                ExcelCell cell = columns[column-1];
                return cell.GetValue<T>();
            }
            return default(T);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("Row {0:00}: {1}", row, '{');
            foreach (ExcelCell excelCell in columns)
            {
                builder.AppendFormat(" '{0}'",excelCell.GetValue<string>());
            }
            builder.Append(" }");
            return builder.ToString();
        }


        public void AssertValues<T>(IResolveConstraint resolveConstraint)
        {
            List<T> values = columns.Select(cell => cell.GetValue<T>()).ToList();
            Assert.That(values, resolveConstraint, ToString());
        }
    }
}