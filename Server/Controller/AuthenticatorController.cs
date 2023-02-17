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
    public class AuthenticatorController : AbstractController
    {
        public AuthenticatorController(BaseScript baseScript) : base(baseScript) { }

        public void PlayerConnecting(Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            deferrals.defer();

            var license = player.Identifiers["license"];

            using (var context = DatabaseContextManager.Context)
            {
                try
                {
                    deferrals.update($"Carregando dados...");
            
                    if (context.Account.Any(m => m.License == license))
                    {
                        Debug.WriteLine($"Logging account: {playerName}");

                        var account = context.GetAccount(license);
            
                        var gamePlayer = new GamePlayer(player, account);

                        if (GameInstance.Instance.AddPlayer(license, gamePlayer))
                            deferrals.done();
                        else
                            kickCallback("Conta já conectada, tente novamente");
                    }
                    else
                    {
                        Debug.WriteLine($"Creating account for: {playerName}");

                        var account = new AccountModel()
                        {
                            License = license,
                            Created = DateTime.Now,
                            WhiteListed = true
                        };
                        context.Account.Add(account);
            
                        context.SaveChanges();
            
                        var gamePlayer = new GamePlayer(player, account);
            
                        if (GameInstance.Instance.AddPlayer(license, gamePlayer))
                            deferrals.done();
                    }
                }
                catch
                {
                    deferrals.done($"Houve um problema no registro \"{license}\", tente novamente ou entre em contato com a equipe.");
                }
            }
        }

        public void OnPlayerDropped(Player player, string reason)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.RemovePlayer(license, out var gamePlayer))
            {
                Debug.WriteLine($"[{gamePlayer.Account.Id}] Player {gamePlayer.Player.Name} dropped for reason: {reason}");
            
                using (var context = DatabaseContextManager.Context)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try {
                            var account = gamePlayer.Account;
            
                            var gameCharacter = player.Character;
            
                            var gameCharacterPosition = gameCharacter.Position;
                            var gameCharacterRotation = gameCharacter.Rotation;
            
                            var dbCharacter = account.Character.FirstOrDefault(m => m.Slot == account.CurrentCharacter);

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
                            context.SaveChanges();
            
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
