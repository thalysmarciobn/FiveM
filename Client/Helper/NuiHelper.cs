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
        public static void SendMessage(NuiMessage message)
        {
            string data = JsonConvert.SerializeObject(message);

            SendNuiMessage(data);
        }
    }
}
