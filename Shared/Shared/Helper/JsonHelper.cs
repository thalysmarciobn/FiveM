using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Enumerations;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared.Helper
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
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

        public static T DeserializeObject<T>(string data)
        {
            return (T) JsonConvert.DeserializeObject(data);
        }
    }
}
