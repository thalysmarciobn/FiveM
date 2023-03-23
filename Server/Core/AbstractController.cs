using CitizenFX.Core;

namespace Server.Core
{
    public abstract class AbstractController
    {
        protected void TriggerClientEvent(Player player, string eventName, params object[] eventArgs)
        {
            BaseScript.TriggerClientEvent(player, eventName, eventArgs);
        }
    }
}