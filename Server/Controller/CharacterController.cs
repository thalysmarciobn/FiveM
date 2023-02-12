using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Core;
using Server.Core.Game;
using Server.Database;
using Server.Extensions;
using Server.Instances;
using Shared.Helper;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static CitizenFX.Core.Native.API;

namespace Server.Controller
{
    public class CharacterController : AbstractController
    {
        public CharacterController(BaseScript baseScript) : base(baseScript)
        {
        }

        public void SpawnRequest(string license)
        {
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
                            Rotation = new AccountCharacterRotationModel
                            {
                                X = 0,
                                Y = 0,
                                Z = 0
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
                    TriggerClientEvent(gamePlayer.Player, EventName.Client.InitCharacter, json);
                }
            }
        }

        public void ProjectPlayerPositionUpdate(string license, float x, float y, float z)
        {

            if (GameInstance.Instance.GetPlayer(license, out GamePlayer gamePlayer))
            {
                //gamePlayer.CurrentCharacter.Position.X = x;
                //gamePlayer.CurrentCharacter.Position.Y = y;
                //gamePlayer.CurrentCharacter.Position.Z = z;
            }
        }
    }
}
