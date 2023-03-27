using CitizenFX.Core;
using System;
using static CitizenFX.Core.Native.API;

namespace Client.Core
{
    public abstract class BaseScriptAbstract : BaseScript
    {
        public BaseScriptAbstract()
        {
            RegisterHandler(EventName.External.Client.OnClientResourceStart, new Action<string>(OnClientResourceStart));
            RegisterHandler(EventName.External.Client.OnClientResourceStop, new Action<string>(OnClientResourceStop));
        }

        protected virtual void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
        }

        protected virtual void OnClientResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
        }

        protected void RegisterHandler(string key, Delegate @delegate)
        {
            EventHandlers[key] += @delegate;
        }

        protected void RegisterNui(string key, Delegate @delegate)
        {
            RegisterNuiCallbackType(key);
            EventHandlers[$"__cfx_nui:{key}"] += @delegate;
        }
    }
}
