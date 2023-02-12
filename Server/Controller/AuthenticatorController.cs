using CitizenFX.Core;
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
    public class AuthenticatorController : AbstractController
    {
        public AuthenticatorController(BaseScript baseScript) : base(baseScript) { }

        public void PlayerConnecting(Player player, string license, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            deferrals.defer();

            var logged = new Action<AccountModel, long>((AccountModel account, long slot) =>
            {
                var gamePlayer = new GamePlayer(account.Id, player);

                if (!GameInstance.Instance.AddPlayer(gamePlayer))
                    return;

                Debug.WriteLine($"[{gamePlayer.DatabaseId}] Account connected: {gamePlayer.License}");
                deferrals.done();
            });

            using (var context = DatabaseContextManager.Context)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    // Ban System
                    // kickCallback("reason")
                    // CancelEvent();
                    if (context.Account.Any(m => m.License == license))
                    {
                        deferrals.update($"Carregando dados...");

                        var account = context.GetAccount(license);

                        logged.Invoke(account, 0);
                        return;
                    }
                    try
                    {
                        deferrals.update($"Criando dados...");

                        var account = new AccountModel()
                        {
                            License = license,
                            Created = DateTime.Now,
                            WhiteListed = true
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

        public async Task OnPlayerDropped(Player player, string license, string reason)
        {
            if (GameInstance.Instance.RemovePlayer(license, out var gamePlayer))
                Debug.WriteLine($"[{gamePlayer.DatabaseId}] Player {gamePlayer.Name} dropped for reason: {reason}");

            using (var context = DatabaseContextManager.Context)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    if (context.Account.Any(m => m.License == license))
                    {
                        var account = context.GetAccount(license);

                        var gameCharacter = player.Character;
                        var gameCharacterPosition = gameCharacter.Position;
                        var gameCharacterRotation = gameCharacter.Rotation;

                        var dbCharacter = account.Character.FirstOrDefault(m => m.Slot == 0);
                        var dbCharacterPosition = dbCharacter.Position;
                        var dbCharacterRotation = dbCharacter.Rotation;

                        dbCharacter.Heading = gameCharacter.Heading;

                        dbCharacterPosition.X = gameCharacterPosition.X;
                        dbCharacterPosition.Y = gameCharacterPosition.Y;
                        dbCharacterPosition.Z = gameCharacterPosition.Z;

                        dbCharacterRotation.X = gameCharacterRotation.X;
                        dbCharacterRotation.Y = gameCharacterRotation.Y;
                        dbCharacterRotation.Z = gameCharacterRotation.Z;

                        context.Update(account);
                        await context.SaveChangesAsync();

                        Debug.WriteLine("updated");
                    }
                    await transaction.CommitAsync();
                }
            }
        }
    }
}
