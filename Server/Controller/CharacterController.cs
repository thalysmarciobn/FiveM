using CitizenFX.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Core;
using Server.Core.Game;
using Server.Core.Server;
using Server.Database;
using Server.Extensions;
using Server.Instances;
using Shared.Helper;
using Shared.Models.Database;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controller
{
    public class CharacterController : AbstractController
    {
        public void SpawnRequest(Player player)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.GetPlayer(license, out GamePlayer gamePlayer))
            {
                using (var context = DatabaseContextManager.Context)
                {
                    var account = gamePlayer.Account;

                    var json = JsonConvert.SerializeObject(account);

                    TriggerClientEvent(gamePlayer.Player, EventName.Client.InitAccount, json);

                    //if (account.Character.Count <= 0)
                    //{
                    //    var createCharacter = new AccountCharacterModel
                    //    {
                    //        AccountId = account.Id,
                    //        Slot = 0,
                    //        DateCreated = DateTime.Now,
                    //        Model = "mp_m_freemode_01",
                    //        Health = 0,
                    //        Armor = 100,
                    //        Heading = 100,
                    //        Position = new AccountCharacterPositionModel
                    //        {
                    //            X = -1062.02f,
                    //            Y = -2711.85f,
                    //            Z = 0.83f
                    //        },
                    //        Rotation = new AccountCharacterRotationModel
                    //        {
                    //            X = 0,
                    //            Y = 0,
                    //            Z = 0
                    //        },
                    //        PedHeadData = new AccountCharacterPedHeadDataModel
                    //        {
                    //
                    //        },
                    //        PedHead = new AccountCharacterPedHeadModel
                    //        {
                    //
                    //        },
                    //        PedFace = CharacterModelHelper.DefaultList<AccountCharacterPedFaceModel>(),
                    //        PedComponent = CharacterModelHelper.DefaultList<AccountCharacterPedComponentModel>(),
                    //        PedProp = CharacterModelHelper.DefaultList<AccountCharacterPedPropModel>(),
                    //        PedHeadOverlay = CharacterModelHelper.DefaultList<AccountCharacterPedHeadOverlayModel>(),
                    //        PedHeadOverlayColor = CharacterModelHelper.DefaultList<AccountCharacterPedHeadOverlayColorModel>()
                    //    };
                    //    account.Character.Add(createCharacter);
                    //
                    //    context.Update(account);
                    //
                    //    context.SaveChanges();
                    //
                    //    var json = JsonConvert.SerializeObject(createCharacter);
                    //    TriggerClientEvent(gamePlayer.Player, EventName.Client.InitCharacter, json);
                    //}
                    //else
                    //{
                    //    var character = account.Character.FirstOrDefault(x => x.Slot == account.CurrentCharacter);
                    //    var json = JsonConvert.SerializeObject(character);
                    //    TriggerClientEvent(gamePlayer.Player, EventName.Client.InitCharacter, json);
                    //}
                }
            }
        }
    }
}
