using CitizenFX.Core;
using CitizenFX.Core.Native;
using Microsoft.EntityFrameworkCore;
using Server.Core;
using Server.Core.Game;
using Server.Core.Server;
using Server.Database;
using Server.Extensions;
using Server.Instances;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

                    GameInstance.Instance.AddPlayer(license, new GamePlayer(player, account));

                    if (int.TryParse(player.Handle, out var playerServerId))
                    {
                        GameInstance.Instance.SetPassive(playerServerId, false);
                        Debug.WriteLine($"[PROJECT][{playerServerId}] Registered passive.");
                    }
                }
            }
            Debug.WriteLine($"[ServerController] Players registered: {GameInstance.Instance.PlayerCount}");
            Debug.WriteLine($"[ServerController] Passives registered: {GameInstance.Instance.PassivesCount}");
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

        public void RemoveSpawnVehicles()
        {
            foreach (var vehicle in GameInstance.Instance.GetSpawnVehicles)
            {
                if (API.DoesEntityExist(vehicle.ServerId))
                {
                    API.DeleteEntity(vehicle.ServerId);
                    Debug.WriteLine($"[ServerController][{vehicle.ServerId}] vehicle removed.");
                }
            }
        }
    }
}
