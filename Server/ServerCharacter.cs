using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using FiveM.Server.Database;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using Newtonsoft.Json;
using Server.Instances;
using Server.Core.Game;

namespace FiveM.Server
{
    public class ServerCharacter : BaseScript
    {
        public static bool s_Debug = true;
        public ServerCharacter()
        {
            Debug.WriteLine("FiveM Project!");
        }

        [EventHandler(EventName.Server.ProjectPlayerSpawned)]
        public void ProjectPlayerSpawned([FromSource] Player player)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.GetPlayer(license, out GamePlayer gamePlayer))
            {
                var json = JsonConvert.SerializeObject(gamePlayer.CurrentCharacter);

                TriggerClientEvent(player, EventName.Client.ProjectInitCharacter, json);
            }
        }

        [EventHandler(EventName.Server.ProjectPlayerPositionUpdate)]
        public void ProjectPlayerPositionUpdate([FromSource] Player player, float x, float y, float z)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.GetPlayer(license, out GamePlayer gamePlayer))
            {
                gamePlayer.CurrentCharacter.Position.X = x;
                gamePlayer.CurrentCharacter.Position.Y = y;
                gamePlayer.CurrentCharacter.Position.Z = z;

                if (s_Debug)
                    Debug.WriteLine($"[{gamePlayer.DatabaseId}] Character Updated Position: {x} {y} {z}");
            }
        }
    }
}