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
        public ServerController(BaseScript baseScript) : base(baseScript) { }

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
                }
            }
            Debug.WriteLine($"[ServerController] Players registered: {GameInstance.Instance.PlayerCount}");
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

        public void RemoveVehiclesAndDrivers()
        {
            foreach (var vehicle in GameInstance.Instance.GetVehicles)
            {
                if (API.DoesEntityExist(vehicle.ServerVehicleId))
                {
                    API.DeleteEntity(vehicle.ServerVehicleId);
                    Debug.WriteLine($"[ServerController][{vehicle.ServerVehicleId}] vehicle removed.");
                }
                if (API.DoesEntityExist(vehicle.ServerDriverId))
                {
                    API.DeleteEntity(vehicle.ServerDriverId);
                    Debug.WriteLine($"[ServerController][{vehicle.ServerVehicleId}] ped removed.");
                }
            }
        }

        public bool CheckIfVehicleHasEntity(long id)
        {
            if (GameInstance.Instance.GetVehicle(id, out var model))
            {
                
            }
            return false;
        }
    }
}
