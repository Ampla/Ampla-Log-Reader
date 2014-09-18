using System;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class EditedDataDescriptorCollection
    {
        private readonly string editedData;

        public EditedDataDescriptorCollection(string editedData)
        {
            this.editedData = editedData;
            
            Location = GetLocation(editedData);
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
            return editedData;
        }

        private string GetLocation(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xml);
                return XmlHelper.GetText(xmlDoc, "/EditedDataDescriptorCollection/@Location");
            }
            catch (XmlException)
            {
            }
            return null;
        }
    }
}