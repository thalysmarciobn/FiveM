using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Helper
{
    public static class NuiHelper
    {
        public static void SendMessage(string action, string key, object[] parameters)
        {
            using (var message = new NuiMessage
            {
                Action = action,
                Key = key,
                Params = parameters
            })
            {
                SendNuiMessage(JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                }));
            }
        }
    }
}
