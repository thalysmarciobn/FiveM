using CitizenFX.Core;
using Server.Core.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Core
{
    public abstract class AbstractController
    {
        private BaseScript BaseScript { get; }

        public AbstractController(BaseScript baseScript)
        {
            BaseScript = baseScript;
        }

        protected void TriggerClientEvent(Player player, string eventName, params object[] eventArgs) =>
            BaseScript.TriggerClientEvent(player, eventName, eventArgs);
    }
}
