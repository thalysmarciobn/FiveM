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
using System.Collections.Generic;
using CitizenFX.Core.Native;
using System.Threading;
using Server.Core.Game;
using Shared.Helper;

namespace FiveM.Server
{
    public class ServerAuthenticator : BaseScript
    {
        public static bool s_Debug = true;
        public ServerAuthenticator()
        {
            Debug.WriteLine("FiveM Project!");
        }

        [EventHandler(EventName.Native.Server.PlayerConnecting)]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            deferrals.defer();

            var license = player.Identifiers["license"];

            var logged = new Action<AccountModel, long>((AccountModel account, long slot) =>
            {
                var gamePlayer = new GamePlayer(account.Id, player);

                if (!GameInstance.Instance.AddPlayer(gamePlayer))
                    return;

                var character = account.Character.SingleOrDefault(m => m.Slot == slot);

                gamePlayer.CurrentCharacter = character;

                Debug.WriteLine($"[{gamePlayer.DatabaseId}] Account connected: {gamePlayer.License}");
                deferrals.done();
            });

            using (var context = new FiveMContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    if (context.Account.Any(m => m.License == license))
                    {
                        deferrals.update($"Carregando dados...");

                        var account = context.Account
                            .Include(m => m.Character).ThenInclude(m => m.Position)
                            .Include(m => m.Character).ThenInclude(m => m.PedHeadData)
                            .Include(m => m.Character).ThenInclude(m => m.PedHead)
                            .Include(m => m.Character).ThenInclude(m => m.PedFace)
                            .Include(m => m.Character).ThenInclude(m => m.PedComponent)
                            .Include(m => m.Character).ThenInclude(m => m.PedProp)
                            .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlay)
                            .Include(m => m.Character).ThenInclude(m => m.PedHeadOverlayColor)
                            .Single(x => x.License == license);

                        logged.Invoke(account, 0);
                        return;
                    }
                    try
                    {
                        deferrals.update($"Criando dados...");

                        var character = new AccountCharacterModel
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
                        };
                        var account = new AccountModel()
                        {
                            License = license,
                            Created = DateTime.Now,
                            WhiteListed = true,
                            Character = new List<AccountCharacterModel> { character }
                        };

                        context.Account.Add(account);

                        if (context.SaveChanges() > 0)
                        {
                            Debug.WriteLine($"[{account.Id}] Account created: {account.License}");

                            logged.Invoke(account, 0);
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        deferrals.update($"Houve um problema no registro \"{license}\", tente novamente ou entre em contato com a equipe.");
                        transaction.Rollback();
                    }
                }
            }
        }

        [EventHandler(EventName.Native.Server.PlayerDropped)]
        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.RemovePlayer(license, out var gamePlayer))
                Debug.WriteLine($"[{gamePlayer.DatabaseId}] Player {gamePlayer.Name} dropped for reason: {reason}");
        }
    }
}