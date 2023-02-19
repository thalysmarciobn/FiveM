using CitizenFX.Core;
using CitizenFX.Core.Native;
using Server.Core.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Core
{
    public abstract class AbstractController
    {
        protected void TriggerClientEvent(Player player, string eventName, params object[] eventArgs) =>
            BaseScript.TriggerClientEvent(player, eventName, eventArgs);
    }
}
