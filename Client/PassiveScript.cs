using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class PassiveScript : BaseScript
    {
        private readonly object LockObject = new object();
        private ConcurrentDictionary<int, bool> PassiveList { get; } = new ConcurrentDictionary<int, bool>();
        public PassiveScript()
        {
            Debug.WriteLine("[PROJECT] Script: PassiveScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
            EventHandlers[EventName.Client.UpdatePassiveList] += new Action<string>(UpdatePassiveList);
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            TriggerServerEvent(EventName.Server.GetPassiveList, new Action<string>((arg) =>
            {
                lock (LockObject)
                {
                    var data = JsonConvert.DeserializeObject<ICollection<KeyValuePair<int, bool>>>(arg);
                    foreach (var kvp in data)
                        PassiveList.TryAdd(kvp.Key, kvp.Value);
                }
            }));
        }

        private void UpdatePassiveList(string arg)
        {
            lock (LockObject)
            {
                var data = JsonConvert.DeserializeObject<ICollection<KeyValuePair<int, bool>>>(arg);
                foreach (var kvp in data)
                    PassiveList.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => kvp.Value);
            }
        }

        [Tick]
        public Task Collisions()
        {
            lock (LockObject)
            {
                var localPlayer = Game.Player;

                var localPed = localPlayer.Character;
                var localVehicle = localPed?.CurrentVehicle;
                var localHooked = localVehicle?.GetHookedVehicle();

                var peds = World.GetAllPeds();
                var vehicles = World.GetAllVehicles();

                foreach (var player in Players)
                {
                    if (PassiveList.ContainsKey(player.ServerId))
                    {
                        var passive = PassiveList[player.ServerId];

                        Debug.WriteLine($"{player.ServerId} {passive}");

                        var pedCharacter = player.Character;
                        var pedVehicle = pedCharacter?.CurrentVehicle;
                        var pedHooked = pedVehicle?.GetHookedVehicle();

                        int alpha = passive ? 200 : 255;

                        pedCharacter?.SetAlpha(alpha);
                        pedVehicle?.SetAlpha(alpha);
                        pedHooked?.SetAlpha(alpha);

                        if (!passive) continue;

                        pedCharacter?.SetEntityNoCollision(localPed, passive);
                        pedVehicle?.SetEntityNoCollision(localPed, passive);
                        pedHooked?.SetEntityNoCollision(localPed, passive);

                        pedCharacter?.SetEntityNoCollision(localVehicle, passive);
                        pedVehicle?.SetEntityNoCollision(localVehicle, passive);
                        pedHooked?.SetEntityNoCollision(localVehicle, passive);

                        pedCharacter?.SetEntityNoCollision(localHooked, passive);
                        pedVehicle?.SetEntityNoCollision(localHooked, passive);
                        pedHooked?.SetEntityNoCollision(localHooked, passive);

                        foreach (var ped in peds)
                        {
                            pedCharacter?.SetEntityNoCollision(ped, passive);
                            pedVehicle?.SetEntityNoCollision(ped, passive);
                            pedHooked?.SetEntityNoCollision(ped, passive);
                        }

                        foreach (var vehicle in vehicles)
                        {
                            pedCharacter?.SetEntityNoCollision(vehicle, passive);
                            pedVehicle?.SetEntityNoCollision(vehicle, passive);
                            pedHooked?.SetEntityNoCollision(vehicle, passive);
                        }
                    }
                }
            }
            return Task.FromResult(0);
        }
    }
}
