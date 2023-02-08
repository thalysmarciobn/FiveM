using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Core;
using static CitizenFX.Core.Native.API;

namespace FiveM.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            EventHandlers["playerSpawned"] += new Action(() => TriggerServerEvent(EventName.ProjectPlayerSpawned));
        }

        [Tick]
        public Task OnTick()
        {
            DrawRect(0.5f, 0.5f, 0.5f, 0.5f, 255, 255, 255, 150);

            return Task.FromResult(0);
        }
    }
}