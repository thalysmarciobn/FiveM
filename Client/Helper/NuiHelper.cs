using Client.Core;
using Client.Core.Message;
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
        public static void SendMessage(string action, string key, string parameter)
        {
            using (var message = new NuiMessage
            {
                Action = action,
                Key = key,
                Params = new[] { parameter }
            })
            {
                SendNuiMessage(JsonHelper.SerializeObject(message));
            }
        }
    }
}