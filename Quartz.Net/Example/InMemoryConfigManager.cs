namespace Example
{
    using System.Collections.Generic;

    public class InMemoryConfigManager : IConfigManager
    {
        private static readonly IDictionary<string, string> Values = new Dictionary<string, string>
        {
            {"type", "InMemmory"}
        };

        public string GetValue(string key)
        {
            string value;

            if (Values.TryGetValue(key, out value))
            {
                return value;
            }

            return "Key not found!";
        }
    }
}