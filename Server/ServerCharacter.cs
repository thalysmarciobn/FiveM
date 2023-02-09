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

            if (CharacterInstance.Instance.GetCharacter(license, out AccountCharacterModel accountCharacter))
            {
                var json = JsonConvert.SerializeObject(accountCharacter);

                TriggerClientEvent(player, EventName.Client.ProjectInitCharacter, json);
            }
        }

        [EventHandler(EventName.Server.ProjectPlayerPositionUpdate)]
        public void ProjectPlayerPositionUpdate([FromSource] Player player, float x, float y, float z)
        {
            var license = player.Identifiers["license"];

            if (CharacterInstance.Instance.GetCharacter(license, out AccountCharacterModel accountCharacter))
            {
                accountCharacter.Position.X = x;
                accountCharacter.Position.Y = y;
                accountCharacter.Position.Z = z;

                if (s_Debug)
                    Debug.WriteLine($"[{accountCharacter.Id}] Character Updated Position: {x} {y} {z}");
            }
        }
    }
}