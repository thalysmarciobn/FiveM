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
using Server.Extensions;
using Server.Database;

namespace FiveM.Server
{
    public class ServerAuthenticator : BaseScript
    {
        public static bool s_Debug = true;
        public ServerAuthenticator()
        {
            Debug.WriteLine("[PROJECT] ServerAuthenticator Started.");
        }

        [EventHandler(EventName.External.Server.PlayerConnecting)]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            deferrals.defer();

            var license = player.Identifiers["license"];

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

        [EventHandler(EventName.External.Server.PlayerDropped)]
        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var license = player.Identifiers["license"];

            if (GameInstance.Instance.RemovePlayer(license, out var gamePlayer))
                Debug.WriteLine($"[{gamePlayer.DatabaseId}] Player {gamePlayer.Name} dropped for reason: {reason}");
        }
    }
}