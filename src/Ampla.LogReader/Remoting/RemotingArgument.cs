using System.Collections.Generic;
using System.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingArgument
    {
        public static RemotingArgument[] Parse(XmlNodeList nodeList)
        {
            List<RemotingArgument> arguments = new List<RemotingArgument>();

            foreach (XmlNode node in nodeList)
            {
                RemotingArgument argument = new RemotingArgument
                    {
                        Index = arguments.Count + 1,
                        TypeName = node.Name,
                        Value = node.InnerText
                    };
                arguments.Add(argument);
            }
            return arguments.ToArray();
        }

        public string TypeName { get; private set; }
        public string Value { get; private set; }
        public int Index { get; private set; }
    }
}