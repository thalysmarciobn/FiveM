using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Core
{
    public abstract class BaseScriptAbstract : BaseScript
    {
        private Delegate OnClientResourceStartDelegate { get; }
        private Delegate OnClientResourceStopDelegate { get; }

        public BaseScriptAbstract()
        {
            OnClientResourceStartDelegate = new Action<string>(OnClientResourceStart);
            OnClientResourceStopDelegate = new Action<string>(OnClientResourceStop);
            RegisterHandler(EventName.External.Client.OnClientResourceStart, OnClientResourceStartDelegate);
            RegisterHandler(EventName.External.Client.OnClientResourceStop, OnClientResourceStopDelegate);
        }

        protected virtual void OnClientResourceStart(string resourceName)
        {
        }

        protected virtual void OnClientResourceStop(string resourceName)
        {
        }

        protected void RegisterHandler(string key, Delegate @delegate)
        {
            EventHandlers[key] += @delegate;
        }

        protected void UnregisterHandler(string key, Delegate @delegate)
        {
            EventHandlers[key] -= @delegate;
        }

        protected void RegisterNui(string key, Delegate @delegate)
        {
            RegisterNuiCallbackType(key);
            EventHandlers[$"__cfx_nui:{key}"] += @delegate;
        }

        public new void TriggerServerEvent(string name, params object[] args)
        {
            BaseScript.TriggerServerEvent(name, args);
        }

        public new Task Delay(int delay)
        {
            return BaseScript.Delay(delay);
        }

        public PlayerList PlayerList()
        {
            return Players;
        }
    }
}
