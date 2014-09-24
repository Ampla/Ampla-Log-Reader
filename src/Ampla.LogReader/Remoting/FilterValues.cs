using System;
using System.Collections.Generic;
using System.Linq;

namespace Ampla.LogReader.Remoting
{
    /// <summary>
    ///     Class used to represent the Filter Values from a remoting query
    /// </summary>
    public class FilterValues
    {
        private readonly string filterString;

        public FilterValues(string filterString)
        {
            this.filterString = filterString;
            List<FieldValue> filters = GetFilters(filterString);
            List<string> filterData = new List<string>();

            Location = filters.Count > 0 ? "Unknown" : null;
            
            foreach (FieldValue filterValue in filters)
            {
                if (filterValue.Name == "Location")
                {
                    Location = filterValue.Value;
                }
                else
                {
                    filterData.Add(filterValue.NameValue);
                }
            }

            FilterData = filterData.Count > 0 ? string.Join(", ", filterData) : null;
        }

        public string Location { get; private set; }
        public string FilterData { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return filterString;
        }

        private List<FieldValue> GetFilters(string filter)
        {
            if (filter != null)
            {
                string[] parsed = filter.Split(new[] {"}, "}, StringSplitOptions.None);
                int missing = parsed.Length - 1;
                List<FieldValue> filters = new List<FieldValue>();
                for (int i = 0; i < parsed.Length; i++)
                {
                    string parse = parsed[i] + (missing > 0 ? "}" : "");
                    FieldValue fieldValue = FieldValue.Parse(parse);
                    if (fieldValue != null)
                    {
                        filters.Add(fieldValue);
                    }
                    missing--;
                }
                return filters;
            }
            return new List<FieldValue>();
        }
        
    }
}