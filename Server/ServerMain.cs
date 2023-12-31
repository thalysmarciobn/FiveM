﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Server.Configurations;
using Server.Controller;
using Server.Core;
using Server.Database;
using Server.Helper;
using Server.Instances;
using Shared.Helper;
using Shared.Models.Message;
using Shared.Models.Messages;
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class ServerMain : BaseServerScriptAbstract
    {
        protected override void OnStart()
        {
            //base.OnStart();

            Debug.WriteLine("[PROJECT] ServerMain Started.");
            
            DatabaseContextManager.Build(Settings.Database);
        }

        [Command("project_players")]
        public void ProjectPlayers()
        {
            Debug.WriteLine($"Players Connected: {GameInstance.Instance.PlayerCount} / {Players.Count()}");
        }

        [Command("weather")]
        public void Weather(int src, List<object> args, string raw)
        {
            TimeSyncController.Update(int.Parse(args[0].ToString()));
            var date = TimeSyncController.CurrentDate;
            Debug.WriteLine(
                $"[PROJECT] Time: {date.Hour}:{date.Minute}:{date.Second}\n - Weather: {TimeSyncController.CurrentWeather}\n - Last Weather: {TimeSyncController.LastWeatherType}\n - Rain Level: {TimeSyncController.RainLevel}\n - Wind Speed: {TimeSyncController.WindSpeed}\n - Wind Direction: {TimeSyncController.WindDirection}");
        }

        #region Connection

        [EventHandler(EventName.External.Server.PlayerConnecting)]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback,
            dynamic deferrals)
        {
            AuthenticatorController.PlayerConnecting(player, playerName, kickCallback, deferrals);
        }

        [EventHandler(EventName.External.Server.PlayerDropped)]
        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            AuthenticatorController.OnPlayerDropped(player, reason);
        }

        #endregion

        #region Resource

        [EventHandler(EventName.External.OnResourceStart)]
        public void OnResourceStart(string resourceName)
        {
            if (resourceName != GetCurrentResourceName()) return;

            ServerController.RegisterPlayers(Players.ToImmutableList());

            ServerController.RegisterVehicles();

            ServerController.RegisterBlips();
        }

        [EventHandler(EventName.External.OnResourceStop)]
        public void OnResourceStop(string resourceName)
        {
            if (resourceName != GetCurrentResourceName()) return;

            ServerController.RemoveSpawnVehicles();

            ThreadInstance.Instance.Shutdown();
        }

        #endregion

        #region PlayerData

        [EventHandler(EventName.Server.SetPassive)]
        public void SetPassive([FromSource] Player player, bool isPassive)
        {
            if (int.TryParse(player.Handle, out var playerServerId))
            {
                var serverPlayer = GameInstance.Instance.GetOrAddPlayerData(playerServerId);
                Debug.WriteLine($"[PROJECT][{playerServerId}] Passive updated: {isPassive}");
                serverPlayer.IsPassive = isPassive;
            }

            using (var message = new PlayerDataListMessage
            {
                List = GameInstance.Instance.GetPlayerDataList
            })
            {
                foreach (var entity in Players)
                    entity.TriggerEvent(EventName.Client.UpdatePlayerDataList, JsonHelper.SerializeObject(message));
            }
        }

        [EventHandler(EventName.Server.GetPassive)]
        public void GetPassive(int playerServerId, NetworkCallbackDelegate networkCallback)
        {
            networkCallback.Invoke(GameInstance.Instance.GetPlayerIsPassive(playerServerId));
        }

        [EventHandler(EventName.Server.GetPlayerDataList)]
        public void GetPlayerDataList(NetworkCallbackDelegate networkCallback)
        {
            using (var message = new PlayerDataListMessage
            {
                List = GameInstance.Instance.GetPlayerDataList
            })
            {
                networkCallback.Invoke(JsonHelper.SerializeObject(message));
            }
        }

        #endregion

        #region Map

        [EventHandler(EventName.Server.GetBlips)]
        public void GetBlips(NetworkCallbackDelegate networkCallback)
        {
            using (var message = new BlipListMessage
            {
                List = GameInstance.Instance.GetBlipList
            })
            {
                networkCallback.Invoke(JsonHelper.SerializeObject(message));
            }
        }

        [EventHandler(EventName.Server.GetTimeSync)]
        public void GetTimeSync(NetworkCallbackDelegate networkCallback)
        {
            using (var message = new ServerTimeSyncMessage
            {
                Weather = (uint)TimeSyncController.CurrentWeather,
                RainLevel = TimeSyncController.RainLevel,
                WindSpeed = TimeSyncController.WindSpeed,
                WindDirection = TimeSyncController.WindDirection,
                Ticks = TimeSyncController.CurrentDate.Ticks
            })
            {
                networkCallback.Invoke(JsonHelper.SerializeObject(message));
            }
        }

        #endregion

        #region Character

        [EventHandler(EventName.Server.AccountRequest)]
        public void AccountRequest([FromSource] Player player)
        {
            CharacterController.AccountRequest(player);
        }

        [EventHandler(EventName.Server.CharacterRequest)]
        public void CharacterRequest([FromSource] Player player, int slot, NetworkCallbackDelegate networkCallback)
        {
            CharacterController.CharacterRequest(player, slot, networkCallback);
        }

        [EventHandler(EventName.Server.RegisterCharacter)]
        public void RegisterCharacter([FromSource] Player player, string name, string lastName, int age, int slot,
            ExpandoObject appearance, NetworkCallbackDelegate networkCallback)
        {
            CharacterController.RegisterCharacter(player, name, lastName, age, slot, appearance, networkCallback);
        }

        #endregion

        #region Vehicles

        [EventHandler(EventName.Server.GetServiceVehicles)]
        public void GetServiceVehicles(NetworkCallbackDelegate networkCallback)
        {
            networkCallback.Invoke(JsonHelper.SerializeObject(GameInstance.Instance.GetVehicles));
        }

        [EventHandler(EventName.Server.GetVehicleData)]
        public void GetVehicleData([FromSource] Player player, int vehicleId, NetworkCallbackDelegate networkCallback)
        {
            using (var context = DatabaseContextManager.Context)
            {
                var vehicle = context.Vehicles.FirstOrDefault(x => x.Id == vehicleId);
                using (var message = new VehicleDataMessage())
                {
                    message.Status = vehicle == null ? 0 : 1;
                    message.Model = vehicle;
                    networkCallback.Invoke(JsonHelper.SerializeObject(message));
                }
            }
        }

        [EventHandler(EventName.Server.ForceVehicle)]
        public async void ForceVehicle([FromSource] Player player, uint model, NetworkCallbackDelegate networkCallback)
        {
            var license = player.Identifiers["license"];

            var heading = player.Character.Heading;
            var position = player.Character.Position;

            if (GameInstance.Instance.GetPlayer(license, out var gamePlayer))
                if (VehicleDataHelper.GetVehicleType(model, out var type))
                {
                    Debug.WriteLine($"{model}  {type}  {position.X}  {position.Y}  {position.Z}");
                    var serverVehicleId = CreateVehicle(model, position.X, position.Y + 8.0f, position.Z + 0.5f,
                        heading, true, false);

                    while (!DoesEntityExist(serverVehicleId))
                        await Task.Delay(10);

                    var networkId = NetworkGetNetworkIdFromEntity(serverVehicleId);

                    var data = VehicleHelper.VehicleToData(model, gamePlayer.Account.CurrentCharacter,
                        serverVehicleId);

                    var serverVehicle = new ServerVehicle
                    {
                        ServerId = serverVehicleId,
                        NetworkId = networkId,
                        Data = data
                    };

                    var json = JsonHelper.SerializeObject(serverVehicle);

                    using (var context = DatabaseContextManager.Context)
                    {
                        context.Vehicles.Add(data);
                        context.SaveChanges();
                    }

                    await networkCallback.Invoke(json);
                }
        }

        [EventHandler(EventName.Server.SpawnVehicleService)]
        public async void SpawnVehicleService([FromSource] Player player, int serviceId,
            NetworkCallbackDelegate networkCallback)
        {
            if (GameInstance.Instance.GetVehicle(serviceId, out var model))
            {
                //if (GameInstance.Instance.ContainsSpawnVehicle(model.Id))
                //    return;

                var serverVehicleId = CreateVehicleServerSetter(model.Model, "automobile", model.SpawnX, model.SpawnY,
                    model.SpawnZ, model.SpawnHeading);

                while (!DoesEntityExist(serverVehicleId))
                    await Task.Delay(10);

                // 2 - trancado mas o npc abre a porta
                // 3 - não da pra entrar
                SetVehicleDoorsLocked(serverVehicleId, 3);

                SetVehicleNumberPlateText(serverVehicleId, model.Id.ToString("D4"));

                var networkId = NetworkGetNetworkIdFromEntity(serverVehicleId);

                var spawnServer = new SpawnServerVehicle
                {
                    ServerId = serverVehicleId,
                    NetworkId = networkId,
                    Model = model
                };

                var json = JsonHelper.SerializeObject(spawnServer);

                GameInstance.Instance.AddSpawnVehicle(model.Id, spawnServer);

                await networkCallback.Invoke(json);
            }
        }

        #endregion
    }
}