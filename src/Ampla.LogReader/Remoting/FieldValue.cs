using System;

namespace Ampla.LogReader.Remoting
{
    public class FieldValue
    {
        public FieldValue(string name, string value)
        {
            Name = name;
            Value = value;  
        }

        public string Name { get; private set; }
        public string Value { get; private set; }

        public static FieldValue Parse(string filter)
        {
            string[] parts = filter.Split(new[] {"={", "}"}, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                string name = parts[0];
                string value = parts[1];

                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Trim('\"');
                }

                return new FieldValue(name, value);
            }
            return null;
        }

        public string NameValue
        {
            get { return Name + "={" + Value + "}"; }
        }
    }
}