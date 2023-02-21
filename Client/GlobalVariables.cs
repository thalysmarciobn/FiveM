using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class GlobalVariables
    {
        public const bool S_Debug = true;

        public static JsonSerializer Serializer { get; } = new JsonSerializer
        {
            Culture = CultureInfo.CurrentCulture,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}
