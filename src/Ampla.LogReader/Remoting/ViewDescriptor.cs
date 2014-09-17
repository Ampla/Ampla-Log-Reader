using System;

namespace Ampla.LogReader.Remoting
{
    /// <summary>
    ///     Class used to represent the View Descriptor from a Remoting Query
    /// </summary>
    public class ViewDescriptor
    {
        private readonly string viewDescriptor;

        public ViewDescriptor(string viewDescriptor)
        {
            this.viewDescriptor = viewDescriptor;
            Module = GetModule(viewDescriptor);
        }

        public string Module { get; private set; }

        /// <summary>
        ///     Gets the module from the ViewDescriptor
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        private string GetModule(string view)
        {
            if (!string.IsNullOrEmpty(view))
            {
                //View Descriptor [Downtime.StandardView]
                string[] parts = view.Split(new[] {"[", "."}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    return parts[1];
                }
                return "Unknown";
            }
            return null;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return viewDescriptor;
        }
    }
}