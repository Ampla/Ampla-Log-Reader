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
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(editedData);

            Location = XmlHelper.GetText(xmlDoc, "/EditedDataDescriptorCollection/@Location");
            SetId = XmlHelper.GetValue(xmlDoc, "/EditedDataDescriptorCollection/@SetId", -1);

            FieldValuesList fields = new FieldValuesList();

            if (SetId > 0)
            {
                fields.Add(new FieldValue("Id", SetId.ToString("0")));
            }

            foreach (XmlNode xmlField in XmlHelper.GetNodes(xmlDoc, "/EditedDataDescriptorCollection/EditedDataDescriptor"))
            {
                string field = XmlHelper.GetText(xmlField, "@name");
                string value = XmlHelper.GetText(xmlField, "@editedValue");
                fields.Add(new FieldValue(field, value));
            }

            FieldValues = fields.ToString();

            Operation = "Update Record";
            switch (fields.Count)
            {
                case 3:
                    {
                        if (fields["IsDeleted"] == "True"
                            && fields["Id"] != null
                            && fields["LastModified"] != null)
                        {
                            Operation = "Delete Record";
                        }
                        break;
                    }
                case 5:
                    {
                        if (fields["IsConfirmed"] == "True"
                            && fields["Id"] != null
                            && fields["LastModified"] != null
                            && fields["ConfirmedBy"] != null
                            && fields["ConfirmedDateTime"] != null)
                        {
                            Operation = "Confirm Record";
                        }
                        break;
                    }
            }
        }

        public string Location { get; private set; }
        public int SetId { get; private set; }
        public string FieldValues { get; private set; }
        public string Operation { get; private set; }

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
    }
}