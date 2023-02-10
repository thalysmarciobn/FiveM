using Server.Core;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Server.Instances
{
    public class YamlInstance : AbstractInstance<YamlInstance>
    {
        public ISerializer SerializerBuilder => new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        public IDeserializer DeserializerBuilder => new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
    }
}
