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
        private bool ForceSpawn = true;

        public SpawnScript()
        {
            Debug.WriteLine("[PROJECT] Script: SpawnScript");
            EventHandlers[EventName.External.Client.OnResourceStart] += new Action<string>(OnResourceStart);
        }

        public void OnResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            Debug.WriteLine("OnResourceStart");
            TriggerServerEvent(EventName.Server.SpawnRequest);
        }
    }
}
