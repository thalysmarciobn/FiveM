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
using Server.Extensions;
using Shared.Helper;
using Server.Database;

namespace FiveM.Server
{
    public class ServerCharacter : BaseScript
    {
        public static bool s_Debug = true;

        public ServerCharacter()
        {
            Debug.WriteLine("[PROJECT] ServerCharacter Started.");
        }

        [EventHandler(EventName.Server.SpawnRequest)]
        public void SpawnRequest([FromSource] Player player)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.GetPlayer(license, out GamePlayer gamePlayer))
            {
                using (var context = DatabaseContextManager.Context)
                {
                    var account = context.GetAccount(license);

                    if (account.Character.Count <= 0)
                    {
                        account.Character.Add(new AccountCharacterModel
                        {
                            Slot = 0,
                            DateCreated = DateTime.Now,
                            Model = "mp_m_freemode_01",
                            Position = new AccountCharacterPositionModel
                            {
                                X = -1062.02f,
                                Y = -2711.85f,
                                Z = 0.83f
                            },
                            PedHeadData = new AccountCharacterPedHeadDataModel
                            {

                            },
                            PedHead = new AccountCharacterPedHeadModel
                            {

                            },
                            PedFace = CharacterModelHelper.DefaultList<AccountCharacterPedFaceModel>(),
                            PedComponent = CharacterModelHelper.DefaultList<AccountCharacterPedComponentModel>(),
                            PedProp = CharacterModelHelper.DefaultList<AccountCharacterPedPropModel>(),
                            PedHeadOverlay = CharacterModelHelper.DefaultList<AccountCharacterPedHeadOverlayModel>(),
                            PedHeadOverlayColor = CharacterModelHelper.DefaultList<AccountCharacterPedHeadOverlayColorModel>()
                        });
                        context.SaveChanges();
                    }
                    var character = account.Character.First();
                    var json = JsonConvert.SerializeObject(character);
                    TriggerClientEvent(player, EventName.Client.InitCharacter, json);
                }
            }
        }

        [EventHandler(EventName.Server.ProjectPlayerPositionUpdate)]
        public void ProjectPlayerPositionUpdate([FromSource] Player player, float x, float y, float z)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.GetPlayer(license, out GamePlayer gamePlayer))
            {
                //gamePlayer.CurrentCharacter.Position.X = x;
                //gamePlayer.CurrentCharacter.Position.Y = y;
                //gamePlayer.CurrentCharacter.Position.Z = z;

                if (s_Debug)
                    Debug.WriteLine($"[{gamePlayer.DatabaseId}] Character Updated Position: {x} {y} {z}");
            }
        }
    }
}