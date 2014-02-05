namespace Ampla.LogReader.Statistics
{
    public class Result
    {
        private Result(string name, string topic, object data)
        {
            Name = name;
            Topic = topic;
            Data = data;
        }

        public string Name { get; private set; }
        public string Topic { get; private set; }
        public object Data { get; private set; }

        public static Result New<T>(string name, string topic, T data)
        {
            return new Result(name, topic, data);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}: {2}", Name, Topic, Data);
        }
    }
}