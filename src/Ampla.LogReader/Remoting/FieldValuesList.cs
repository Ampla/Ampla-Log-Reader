using System.Collections.Generic;

namespace Ampla.LogReader.Remoting
{
    public class FieldValuesList : List<FieldValue>
    {
        public string this[string field]
        {
            get
            {
                FieldValue fieldValue = Find(f => f.Name == field);
                if (fieldValue != null)
                {
                    return fieldValue.Value;
                }
                return null;
            }
        }

        public override string ToString()
        {
            if (Count > 0)
            {
                return string.Join(", ", this.ConvertAll(f => f.NameValue));
            }
            return "FieldValuesList (Empty)";
        }
    }
}