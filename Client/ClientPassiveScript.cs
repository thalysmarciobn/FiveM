using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client.Extensions;
using Newtonsoft.Json;
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class ClientPassiveScript : BaseScript
    {
        public ClientPassiveScript()
        {
            Debug.WriteLine("[PROJECT] Script: PassiveScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
            EventHandlers[EventName.Client.UpdatePlayerDataList] += new Action<string>(UpdatePlayerDataList);
        }

        private ConcurrentDictionary<int, ServerPlayer> PlayerDataList { get; } = new ConcurrentDictionary<int, ServerPlayer>();

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Task.Factory.StartNew(() =>
            {
                TriggerServerEvent(EventName.Server.GetPlayerDataList, new Action<string>(arg =>
                {
                    var data = JsonConvert.DeserializeObject<ICollection<KeyValuePair<int, ServerPlayer>>>(arg);
                    foreach (var kvp in data)
                        PlayerDataList.TryAdd(kvp.Key, kvp.Value);
                }));
            });
        }

        private void UpdatePlayerDataList(string arg)
        {
            Debug.WriteLine(arg);
            var data = JsonConvert.DeserializeObject<ICollection<KeyValuePair<int, ServerPlayer>>>(arg);
            foreach (var kvp in data)
                PlayerDataList.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => kvp.Value);
        }

        [Tick]
        public Task Collisions()
        {
            var localPlayer = Game.Player;

            var localPed = localPlayer.Character;
            var localVehicle = localPed?.CurrentVehicle;
            var localHooked = localVehicle?.GetHookedVehicle();

            var localPassive = false;

            if (PlayerDataList.TryGetValue(localPlayer.ServerId, out var isLocalPassive))
                localPassive = isLocalPassive.IsPassive;

            foreach (var player in Players)
                if (PlayerDataList.ContainsKey(player.ServerId))
                {
                    var data = PlayerDataList[player.ServerId];

                    var disableCollisions = data.IsPassive || localPassive;

                    if (player.Handle == localPlayer.Handle) continue;

                    var otherPed = player.Character;
                    var otherVehicle = otherPed?.CurrentVehicle;
                    var otherHooked = otherVehicle?.GetHookedVehicle();

                    var alpha = disableCollisions && !GetIsTaskActive(otherPed.Handle, 2) &&
                                localVehicle?.Handle != otherVehicle?.Handle
                        ? 200
                        : 255;
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

            foreach (var vehicle in World.GetAllVehicles())
            {
                var alpha = localPassive && localVehicle?.Handle != vehicle.Handle ? 200 : 255;
                vehicle.SetAlpha(alpha);

                if (!localPassive || localVehicle?.Handle == vehicle.Handle) continue;

                vehicle.SetEntityNoCollision(localPed);
                vehicle.SetEntityNoCollision(localVehicle);
                vehicle.SetEntityNoCollision(localHooked);
            }

            foreach (var ped in World.GetAllPeds())
            {
                const int passiveAlpha = 200;
                const int activeAlpha = 255;
                int alpha = localPassive ? passiveAlpha : activeAlpha;

                if (localVehicle?.Handle != ped.CurrentVehicle?.Handle)
                    alpha = passiveAlpha;

                ped.SetAlpha(alpha);

                if (!localPassive || localVehicle?.Handle == ped.CurrentVehicle?.Handle) continue;

                ped.SetEntityNoCollision(localPed);
                ped.SetEntityNoCollision(localVehicle);
                ped.SetEntityNoCollision(localHooked);
            }

            return Task.FromResult(0);
        }
    }
}