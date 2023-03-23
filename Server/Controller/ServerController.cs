using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Server.Core;
using Server.Core.Game;
using Server.Database;
using Server.Extensions;
using Server.Instances;
using Shared.Models.Server;

namespace Server.Controller
{
    public class ServerController : AbstractController
    {
        public void RegisterPlayers(ICollection<Player> players)
        {
            using (var context = DatabaseContextManager.Context)
            {
                foreach (var player in players)
                {
                    var license = player.Identifiers["license"];

                    var account = context.GetAccount(license);
                    if (account == null)
                        continue;

                    if (GameInstance.Instance.AddPlayer(license, new GamePlayer(player, account)))
                        if (int.TryParse(player.Handle, out var playerServerId))
                            GameInstance.Instance.SetPlayerData(playerServerId, new ServerPlayer
                            {
                                IsPassive = false
                            });
                }
            }

            Debug.WriteLine($"[ServerController] Players registered: {GameInstance.Instance.PlayerCount}");
            Debug.WriteLine($"[ServerController] Player Data registered: {GameInstance.Instance.PlayerDataCount}");
        }

        public void RegisterVehicles()
        {
            using (var context = DatabaseContextManager.Context)
            {
                foreach (var vehicle in context.ServerVehicleService.ToList())
                    GameInstance.Instance.AddVehicle(vehicle.Id, vehicle);
            }

            Debug.WriteLine($"[ServerController] Vehicles registered: {GameInstance.Instance.VehicleCount}");
        }

        public void RegisterBlips()
        {
            using (var context = DatabaseContextManager.Context)
            {
                foreach (var blip in context.Blip.ToList())
                    GameInstance.Instance.AddBlip(blip.Id, blip);
            }

            Debug.WriteLine($"[ServerController] Blips registered: {GameInstance.Instance.BlipCount}");
        }

        public void RemoveSpawnVehicles()
        {
            foreach (var vehicle in GameInstance.Instance.GetSpawnVehicles)
                if (API.DoesEntityExist(vehicle.ServerId))
                {
                    API.DeleteEntity(vehicle.ServerId);
                    Debug.WriteLine($"[ServerController][{vehicle.ServerId}] vehicle removed.");
                }
        }
    }
}