using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Shared.Helper
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };

        public static string SerializeObject<T>(T obj)
        {
            var stringBuilder = new StringBuilder();
            using (var jsonWriter = new JsonTextWriter(new StringWriter(stringBuilder)))
            {
                var serializer = JsonSerializer.Create(_serializerSettings);
                serializer.Serialize(jsonWriter, obj);
            }

            return stringBuilder.ToString();
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}