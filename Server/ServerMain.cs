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
using Shared.Models.Game;
using Newtonsoft.Json;
using Shared.Models.Database;
using CitizenFX.Core.Native;

namespace Server
{
    public class ServerMain : BaseScript
    {
        private ServerController ServerController { get; }
        private AuthenticatorController AuthenticatorController { get; }
        private CharacterController CharacterController { get; }
        public ServerMain()
        {
            ServerController = new ServerController(this);
            AuthenticatorController = new AuthenticatorController(this);
            CharacterController = new CharacterController(this);
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

        [EventHandler(EventName.External.Server.PlayerConnecting)]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals) =>
            AuthenticatorController.PlayerConnecting(player, playerName, kickCallback, deferrals);

        [EventHandler(EventName.External.Server.PlayerDropped)]
        private void OnPlayerDropped([FromSource] Player player, string reason) =>
            AuthenticatorController.OnPlayerDropped(player, reason);

        [EventHandler(EventName.Server.SpawnRequest)]
        public void SpawnRequest([FromSource] Player player) =>
            CharacterController.SpawnRequest(player);

        [EventHandler(EventName.External.OnResourceStart)]
        public void OnResourceStart(string resourceName)
        {
            if (resourceName != GetCurrentResourceName()) return;

            ServerController.RegisterPlayers(Players.ToImmutableList());

            ServerController.RegisterVehicles();
        }

        [EventHandler(EventName.External.OnResourceStop)]
        public void OnResourceStop(string resourceName)
        {
            if (resourceName != GetCurrentResourceName()) return;

            ServerController.RemoveVehiclesAndDrivers();
        }

        [EventHandler(EventName.Server.GetServiceVehicles)]
        public void GetServiceVehicles(NetworkCallbackDelegate networkCallback)
        {
            var json = JsonConvert.SerializeObject(GameInstance.Instance.GetVehicles);
            networkCallback.Invoke(json);
        }

        [EventHandler(EventName.Server.SpawnVehicleService)]
        public void SpawnVehicleService([FromSource] Player player, int serviceId, NetworkCallbackDelegate networkCallback)
        {
            if (GameInstance.Instance.GetVehicle(serviceId, out var model))
            {
                if (model.IsSpawned)
                {
                    return;
                }

                model.ServerVehicleId = CreateVehicle(model.Model, model.SpawnX, model.SpawnY, model.SpawnZ, model.SpawnHeading, true, true);
                // 2 - trancado mas o npc abre a porta
                // 3 - não da pra entrar
                //SetVehicleDoorsLocked(model.ServerVehicleId, 3);

                while (!DoesEntityExist(model.ServerVehicleId))
                    Task.Delay(0);

                SetVehicleNumberPlateText(model.ServerVehicleId, model.Id.ToString("D4"));

                model.ServerDriverId = CreatePedInsideVehicle(model.ServerVehicleId, 1, model.Driver, -1, true, true);

                while (!DoesEntityExist(model.ServerDriverId))
                    Task.Delay(0);

                SetPedRandomProps(model.ServerDriverId);
                SetPedRandomComponentVariation(model.ServerDriverId, true);

                TaskEnterVehicle(player.Character.Handle, model.ServerVehicleId, -1, 0, 1.5f, 1, 0);

                model.ServerVehicleNetworkId = NetworkGetNetworkIdFromEntity(model.ServerVehicleId);
                model.ServerDriverNetworkId = NetworkGetNetworkIdFromEntity(model.ServerDriverId);

                var json = JsonConvert.SerializeObject(model);

                networkCallback.Invoke(json);

                model.IsSpawned = true;
            }
        }

        [Command("project_players")]
        public void ProjectPlayers() => 
            Debug.WriteLine($"Players Connected: {GameInstance.Instance.PlayerCount} / {Players.Count()}");

        [Command("project_sync")]
        public void ProjectSync()
        {

        }
    }
}
