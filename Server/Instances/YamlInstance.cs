using Server.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Server.Instances
{
    public class YamlInstance : AbstractInstance<YamlInstance>
    {
        public ISerializer SerializerBuilder =>
            new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

        public IDeserializer DeserializerBuilder => new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
    }
}