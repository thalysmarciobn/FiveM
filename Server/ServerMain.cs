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

namespace Server
{
    public class ServerMain : BaseScript
    {
        private AuthenticatorController AuthenticatorController { get; }
        private CharacterController CharacterController { get; }
        public ServerMain()
        {
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
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            var license = player.Identifiers["license"];
            AuthenticatorController.PlayerConnecting(player, license, playerName, kickCallback, deferrals);
        }

        [EventHandler(EventName.External.Server.PlayerDropped)]
        private async void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var license = player.Identifiers["license"];
            await AuthenticatorController.OnPlayerDropped(player, license, reason);
        }

        [EventHandler(EventName.Server.SpawnRequest)]
        public void SpawnRequest([FromSource] Player player)
        {
            var license = player.Identifiers["license"];
            CharacterController.SpawnRequest(license);
        }

        [EventHandler(EventName.External.OnResourceStart)]
        public void OnResourceStart(string resourceName)
        {
            if (resourceName != "project")
                return;

            using (var context = DatabaseContextManager.Context)
            {
                foreach (var player in Players.ToImmutableList())
                {
                    var license = player.Identifiers["license"];

                    var account = context.Account.SingleOrDefault(m => m.License == license);
                    if (account == null)
                        continue;

                    GameInstance.Instance.AddPlayer(new GamePlayer(account.Id, player));
                }
                Debug.WriteLine($"Players added: {GameInstance.Instance.PlayerCount}");
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
