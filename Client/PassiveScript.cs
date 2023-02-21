﻿using CitizenFX.Core;
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

                var localPassive = false;

                if (PassiveList.TryGetValue(Game.Player.ServerId, out var isLocalPassive))
                    localPassive = isLocalPassive;

                foreach (var player in Players)
                {
                    if (PassiveList.ContainsKey(player.ServerId))
                    {
                        var passive = PassiveList[player.ServerId];

                        var disableCollisions = passive || localPassive;

                        if (player.Handle == localPlayer.Handle) continue;

                        var otherPed = player.Character;
                        var otherVehicle = otherPed?.CurrentVehicle;
                        var otherHooked = otherVehicle?.GetHookedVehicle();

                        var alpha = disableCollisions && !GetIsTaskActive(otherPed.Handle, 2) && localVehicle?.Handle != otherVehicle?.Handle ? 200 : 255;
                        otherPed.SetAlpha(alpha);
                        otherVehicle?.SetAlpha(alpha);
                        otherHooked?.SetAlpha(alpha);

                        if (disableCollisions)
                        {
                            otherPed.SetEntityNoCollision(localPed);
                            otherPed?.SetEntityNoCollision(localVehicle);
                            otherPed?.SetEntityNoCollision(localHooked);

                            otherVehicle.SetEntityNoCollision(localPed);
                            otherVehicle?.SetEntityNoCollision(localVehicle);
                            otherVehicle?.SetEntityNoCollision(localHooked);

                            otherHooked.SetEntityNoCollision(localPed);
                            otherHooked?.SetEntityNoCollision(localVehicle);
                            otherHooked?.SetEntityNoCollision(localHooked);
                        }
                    }
                }

                foreach (var vehicle in vehicles)
                {
                    var alpha = localPassive && vehicle.Handle != localVehicle?.Handle ? 200 : 255;
                    vehicle.SetAlpha(alpha);

                    vehicle.SetEntityNoCollision(localPed);
                    vehicle.SetEntityNoCollision(localVehicle);
                    vehicle.SetEntityNoCollision(localHooked);
                }

                foreach (var ped in peds)
                {
                    var alpha = localPassive && ped?.CurrentVehicle.Handle != localVehicle?.Handle ? 200 : 255;
                    ped.SetAlpha(alpha);

                    ped.SetEntityNoCollision(localPed);
                    ped.SetEntityNoCollision(localVehicle);
                    ped.SetEntityNoCollision(localHooked);
                }

            }
            return Task.FromResult(0);
        }
    }
}
