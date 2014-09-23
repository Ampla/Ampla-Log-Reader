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

        private class FilterValue
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public static FilterValue Parse(string filter)
            {
                string[] parts = filter.Split(new[] { "={", "}" }, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    string name = parts[0];
                    string value = parts[1];

                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Trim('\"');
                    }

                    return new FilterValue
                        {
                            Name = name,
                            Value = value,
                        };
                }
                return null;
            }

            public string NameValue
            {
                get
                {
                    return Name + "={" + Value + "}";
                }
            }
        }

        public FilterValues(string filterString)
        {
            this.filterString = filterString;
            List<FilterValue> filters = GetFilters(filterString);
            List<string> filterData = new List<string>();

            Location = filters.Count > 0 ? "Unknown" : null;
            
            foreach (FilterValue filterValue in filters)
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

        private List<FilterValue> GetFilters(string filter)
        {
            if (filter != null)
            {
                string[] parsed = filter.Split(new[] {", "}, StringSplitOptions.None);
                List<FilterValue> filters = parsed.Select(FilterValue.Parse).Where(fv => fv != null).ToList();
                return filters;
            }
            return new List<FilterValue>();
        }
        
    }
}