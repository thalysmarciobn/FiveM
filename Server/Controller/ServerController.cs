using CitizenFX.Core;
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
            Debug.WriteLine($"Players registered: {GameInstance.Instance.PlayerCount}");
        }

        public void RegisterVehicles()
        {
            using (var context = DatabaseContextManager.Context)
            {
                foreach (var vehicle in context.ServerVehicleService.ToList())
                    GameInstance.Instance.AddVehicle(vehicle.Id, vehicle);
            }
            Debug.WriteLine($"Vehicles registered: {GameInstance.Instance.VehicleCount}");
        }
    }
}
