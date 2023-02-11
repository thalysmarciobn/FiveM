using CitizenFX.Core;
using CitizenFX.Core.Native;
using Mono.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class SpawnScript : BaseScript
    {
        public SpawnScript()
        {
            Debug.WriteLine("[PROJECT] Script: SpawnScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            TriggerServerEvent(EventName.Server.SpawnRequest);
        }
    }
}
