﻿using CitizenFX.Core;
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

            ServerController.RemoveSpawnVehicles();
        }

        [EventHandler(EventName.Server.GetServiceVehicles)]
        public void GetServiceVehicles(NetworkCallbackDelegate networkCallback)
        {
            var json = JsonConvert.SerializeObject(GameInstance.Instance.GetVehicles);
            networkCallback.Invoke(json);
        }

        [EventHandler(EventName.Server.SpawnVehicleService)]
        public async void SpawnVehicleService([FromSource] Player player, int serviceId, NetworkCallbackDelegate networkCallback)
        {
            Debug.WriteLine("SpawnVehicleService");
            if (GameInstance.Instance.GetVehicle(serviceId, out var model))
            {
                if (GameInstance.Instance.ContainsSpawnVehicle(model.Id))
                    return;

                Debug.WriteLine("GET  SpawnVehicleService");

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

        [Command("project_players")]
        public void ProjectPlayers() => 
            Debug.WriteLine($"Players Connected: {GameInstance.Instance.PlayerCount} / {Players.Count()}");

        [Command("project_sync")]
        public void ProjectSync()
        {

        }
    }
}
