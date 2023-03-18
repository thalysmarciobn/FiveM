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
using Shared.Helper;
using Client.Helper;

namespace Server
{
    public class ServerMain : BaseScript
    {
        private ServerController ServerController { get; }
        private AuthenticatorController AuthenticatorController { get; }
        private CharacterController CharacterController { get; }
        private TimeSyncController TimeSyncController { get; }
        public ServerMain()
        {
            ServerController = new ServerController();
            AuthenticatorController = new AuthenticatorController();
            CharacterController = new CharacterController();
            TimeSyncController = new TimeSyncController();
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

            TimeSyncController.Initialize();

            //ThreadInstance.Instance.CreateThread(async () =>
            //{
            //    while (TimeSyncController.IsRunning)
            //    {
            //        if (DateTime.Now.Ticks < TimeSyncController.CanUpdate)
            //        {
            //            await Task.Delay(100);
            //            continue;
            //        }
            //        var date = TimeSyncController.CurrentDate;
            //        TimeSyncController.Next();
            //        Debug.WriteLine($"[PROJECT] Time: {date.Hour}:{date.Minute}:{date.Second}\n - Weather: {TimeSyncController.CurrentWeather}\n - Last Weather: {TimeSyncController.LastWeatherType}\n - Rain Level: {TimeSyncController.RainLevel}\n - Wind Speed: {TimeSyncController.WindSpeed}\n - Wind Direction: {TimeSyncController.WindDirection}");
            //    }
            //}).Start();
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

            ThreadInstance.Instance.Shutdown();
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

        [EventHandler(EventName.Server.GetTimeSync)]
        public void GetTimeSync(NetworkCallbackDelegate networkCallback) =>
            networkCallback.Invoke(JsonConvert.SerializeObject(new ServerTimeSync
            {
                Weather = (uint)TimeSyncController.CurrentWeather,
                RainLevel = TimeSyncController.RainLevel,
                WindSpeed = TimeSyncController.WindSpeed,
                WindDirection = TimeSyncController.WindDirection,
                Ticks = TimeSyncController.CurrentDate.Ticks,
            }));
        #endregion

        #region Character

        [EventHandler(EventName.Server.AccountRequest)]
        public void AccountRequest([FromSource] Player player) =>
            CharacterController.AccountRequest(player);

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

        [EventHandler(EventName.Server.ForceVehicle)]
        public async void ForceVehicle([FromSource] Player player, uint model, NetworkCallbackDelegate networkCallback)
        {
            var license = player.Identifiers["license"];

            var heading = player.Character.Heading;
            var position = player.Character.Position;

            if (GameInstance.Instance.GetPlayer(license, out var gamePlayer))
            {

                if (VehicleDataHelper.GetVehicleType(model, out var type))
                {
                    Debug.WriteLine($"{model}  {type}  {position.X}  {position.Y}  {position.Z}");
                    var serverVehicleId = CreateVehicleServerSetter(model, type, position.X, position.Y + 8.0f, position.Z + 0.5f, heading);

                    while (!DoesEntityExist(serverVehicleId))
                        await Task.Delay(0);

                    var networkId = NetworkGetNetworkIdFromEntity(serverVehicleId);

                    var serverVehicle = new ServerVehicle
                    {
                        ServerId = serverVehicleId,
                        NetworkId = networkId,
                    };

                    var json = JsonConvert.SerializeObject(serverVehicle);

                    using (var context = DatabaseContextManager.Context)
                    {
                        var data = VehicleHelper.VehicleToData(model, gamePlayer.Account.CurrentCharacter, serverVehicleId);
                        context.Vehicles.Add(data);
                        context.SaveChanges();
                    }

                    await networkCallback.Invoke(json);
                }
            }
        }

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

        [Command("weather")]
        public void Weather(int src, List<object> args, string raw)
        {
            TimeSyncController.Update(int.Parse(args[0].ToString()));
            var date = TimeSyncController.CurrentDate;
            Debug.WriteLine($"[PROJECT] Time: {date.Hour}:{date.Minute}:{date.Second}\n - Weather: {TimeSyncController.CurrentWeather}\n - Last Weather: {TimeSyncController.LastWeatherType}\n - Rain Level: {TimeSyncController.RainLevel}\n - Wind Speed: {TimeSyncController.WindSpeed}\n - Wind Direction: {TimeSyncController.WindDirection}");
        }
    }
}
