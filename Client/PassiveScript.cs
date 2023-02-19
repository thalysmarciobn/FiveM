using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Extensions;
using Newtonsoft.Json;
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
        private object LockObject = new object();
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
                var localVehicle = localPed.CurrentVehicle;
                var localHooked = localVehicle?.GetHookedVehicle();

                foreach (Player player in Players)
                {
                    if (player == localPlayer)
                        continue;

                    bool isPassive = PassiveList.FirstOrDefault(x => x.Key == player.ServerId).Value;

                    var otherPed = player.Character;
                    var otherVehicle = otherPed.CurrentVehicle;
                    var otherHooked = otherVehicle?.GetHookedVehicle();

                    var alpha = isPassive ? 220 : 255;

                    otherPed.SetAlpha(alpha);
                    otherVehicle?.SetAlpha(alpha);
                    otherHooked?.SetAlpha(alpha);

                    if (!isPassive)
                        continue;

                    if (otherVehicle != null
                        && IsPedInVehicle(otherVehicle.Handle, localPed.Handle, false)
                        && otherVehicle.GetPedOnSeat(VehicleSeat.Driver) != localPed)
                    {
                        continue;
                    }

                    localPed.DisableCollisionsThisFrame(otherPed);
                    localPed.DisableCollisionsThisFrame(otherVehicle);
                    localPed.DisableCollisionsThisFrame(otherHooked);

                    localVehicle?.DisableCollisionsThisFrame(otherPed);
                    localVehicle?.DisableCollisionsThisFrame(otherVehicle);
                    localVehicle?.DisableCollisionsThisFrame(otherHooked);

                    localHooked?.DisableCollisionsThisFrame(otherPed);
                    localHooked?.DisableCollisionsThisFrame(otherVehicle);
                    localHooked?.DisableCollisionsThisFrame(otherHooked);

                    otherPed.DisableCollisionsThisFrame(localPed);
                    otherPed.DisableCollisionsThisFrame(localVehicle);
                    otherPed.DisableCollisionsThisFrame(localHooked);
                    DisableCamCollisionForEntity(otherPed.Handle);

                    otherVehicle?.DisableCollisionsThisFrame(localPed);
                    otherVehicle?.DisableCollisionsThisFrame(localVehicle);
                    otherVehicle?.DisableCollisionsThisFrame(localHooked);
                    if (otherVehicle != null)
                        DisableCamCollisionForEntity(otherVehicle.Handle);

                    otherHooked?.DisableCollisionsThisFrame(localPed);
                    otherHooked?.DisableCollisionsThisFrame(localVehicle);
                    otherHooked?.DisableCollisionsThisFrame(localHooked);
                    if (otherHooked != null)
                        DisableCamCollisionForEntity(otherHooked.Handle);
                }
            }
            return Task.FromResult(0);
        }
    }
}
