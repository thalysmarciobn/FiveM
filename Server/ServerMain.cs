using CitizenFX.Core;
using FiveM.Server.Database;
using FluentScheduler;
using Server.Configurations;
using Server.Core.Game;
using Server.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using Microsoft.EntityFrameworkCore.Storage;
using Server.Database;
using System.Collections.Immutable;
using Server.Controller;
using Server.Core.Server;
using System.ComponentModel;
using static CitizenFX.Core.Native.API;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Server.Extensions;
using Newtonsoft.Json;
using Shared.Models.Database;
using CitizenFX.Core.Native;
using Shared.Models.Server;
using System.Threading;
using System.Dynamic;
using Shared.Enumerations;

namespace Server
{
    public class ServerMain : BaseScript
    {
        private ServerController ServerController { get; }
        private AuthenticatorController AuthenticatorController { get; }
        private CharacterController CharacterController { get; }
        public ServerMain()
        {
            ServerController = new ServerController();
            AuthenticatorController = new AuthenticatorController();
            CharacterController = new CharacterController();
            Debug.WriteLine("[PROJECT] ServerMain Started.");
            var directory = Directory.GetCurrentDirectory();
            var location = $"{directory}/server.yml";
            if (!File.Exists(location))
            {
                var yaml = YamlInstance.Instance.SerializerBuilder.Serialize(new ServerSettings
                {
                    Database = new Configurations.Database
                    {
                        Server = "127.0.0.1",
                        Schema = "fivem",
                        Login = "root",
                        Password = "123",
                        Port = 3306
                    }
                });
                File.WriteAllText(location, yaml);
            }

            var configuration = File.ReadAllText(location);
            var settings = YamlInstance.Instance.DeserializerBuilder.Deserialize<ServerSettings>(configuration);
            DatabaseContextManager.Build(settings.Database);
        }

        #region Connection

        [EventHandler(EventName.External.Server.PlayerConnecting)]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals) =>
            AuthenticatorController.PlayerConnecting(player, playerName, kickCallback, deferrals);

        [EventHandler(EventName.External.Server.PlayerDropped)]
        private void OnPlayerDropped([FromSource] Player player, string reason) =>
            AuthenticatorController.OnPlayerDropped(player, reason);

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
        }

        #endregion

        #region Passive

        [EventHandler(EventName.Server.SetPassive)]
        public void SetPassive([FromSource] Player player, bool isPassive)
        {
            if (int.TryParse(player.Handle, out var playerServerId))
            {
                GameInstance.Instance.SetPassive(playerServerId, isPassive);
                Debug.WriteLine($"[PROJECT][{playerServerId}] Passive updated: {isPassive}");
            }
            var data = JsonConvert.SerializeObject(GameInstance.Instance.GetPassiveList);
            foreach (var entity in Players)
                entity.TriggerEvent(EventName.Client.UpdatePassiveList, data);
        }

        [EventHandler(EventName.Server.GetPassive)]
        public void GetPassive(int playerServerId, NetworkCallbackDelegate networkCallback) =>
            networkCallback.Invoke(GameInstance.Instance.GetPlayerIsPassive(playerServerId));

        [EventHandler(EventName.Server.GetPassiveList)]
        public void GetPassiveList(NetworkCallbackDelegate networkCallback) =>
            networkCallback.Invoke(JsonConvert.SerializeObject(GameInstance.Instance.GetPassiveList));

        #endregion

        #region Map
        [EventHandler(EventName.Server.GetBlips)]
        public void GetBlips(NetworkCallbackDelegate networkCallback) =>
            networkCallback.Invoke(JsonConvert.SerializeObject(GameInstance.Instance.GetBlipList));
        #endregion

        #region Character

        [EventHandler(EventName.Server.SpawnRequest)]
        public void SpawnRequest([FromSource] Player player) =>
            CharacterController.SpawnRequest(player);

        [EventHandler(EventName.Server.CharacterRequest)]
        public void CharacterRequest([FromSource] Player player, int slot, NetworkCallbackDelegate networkCallback) =>
            CharacterController.CharacterRequest(player, slot, networkCallback);

        [EventHandler(EventName.Server.RegisterCharacter)]
        public void RegisterCharacter([FromSource] Player player, string name, string lastName, int age, int slot, ExpandoObject appearance, NetworkCallbackDelegate networkCallback) =>
            CharacterController.RegisterCharacter(player, name, lastName, age, slot, appearance, networkCallback);

        #endregion

        #region Vehicles

        [EventHandler(EventName.Server.GetServiceVehicles)]
        public void GetServiceVehicles(NetworkCallbackDelegate networkCallback) =>
            networkCallback.Invoke(JsonConvert.SerializeObject(GameInstance.Instance.GetVehicles));

        [EventHandler(EventName.Server.SpawnVehicleService)]
        public async void SpawnVehicleService([FromSource] Player player, int serviceId, NetworkCallbackDelegate networkCallback)
        {
            if (GameInstance.Instance.GetVehicle(serviceId, out var model))
            {
                //if (GameInstance.Instance.ContainsSpawnVehicle(model.Id))
                //    return;

                var serverVehicleId = CreateVehicleServerSetter(model.Model, "automobile", model.SpawnX, model.SpawnY, model.SpawnZ, model.SpawnHeading);
                
                while (!DoesEntityExist(serverVehicleId))
                    await Task.Delay(0);

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

                var json = JsonConvert.SerializeObject(spawnServer);

                GameInstance.Instance.AddSpawnVehicle(model.Id, spawnServer);

                await networkCallback.Invoke(json);
            }
        }

        #endregion

        [Command("project_players")]
        public void ProjectPlayers() => 
            Debug.WriteLine($"Players Connected: {GameInstance.Instance.PlayerCount} / {Players.Count()}");

        [Command("project_sync")]
        public void ProjectSync()
        {

        }
    }
}
