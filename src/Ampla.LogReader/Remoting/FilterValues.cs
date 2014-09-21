using System;

namespace Ampla.LogReader.Remoting
{
    /// <summary>
    ///     Class used to represent the Filter Values from a remoting query
    /// </summary>
    public class FilterValues
    {
        private readonly string filter;

        public FilterValues(string filter)
        {
            this.filter = filter;
            Location = GetLocation(filter);
        }

        public string Location { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return filter;
        }

        private string GetLocation(string filterValues)
        {
            if (string.IsNullOrEmpty(filterValues))
            {
                return null;
            }
            //Location={&quot;Enterprise&quot;}, Sample Period={Current Day}
            if (filterValues.Contains("Location={"))
            {
                int position = filterValues.IndexOf("Location={", StringComparison.InvariantCulture);
                if (position >= 0)
                {
                    string trim = filterValues.Substring(position + "Location={".Length);
                    string[] parts = trim.Split(new[] { "={", "}" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 0)
                    {
                        string location = parts[0];
                        return (location.StartsWith("\"") && location.EndsWith("\"")) ? location.Trim('\"') : location;
                    }
                }
            }
            return "Unknown";
        }

    }
}