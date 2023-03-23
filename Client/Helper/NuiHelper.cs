using Client.Core;
using Shared.Helper;
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
                SendNuiMessage(JsonHelper.SerializeObject(message));
            }
        }
    }
}