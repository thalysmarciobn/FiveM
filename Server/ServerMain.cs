using CitizenFX.Core;
using FiveM.Server.Database;
using FluentScheduler;
using Server.Core.Game;
using Server.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class ServerMain : BaseScript
    {
        public ServerMain()
        {
            using (var context = new FiveMContext())
            {
                foreach (var player in Players.ToList())
                {
                    var license = player.Identifiers["license"];

                    var account = context.Account.SingleOrDefault(m => m.License == license);
                    if (account == null)
                        continue;

                    GameInstance.Instance.AddPlayer(new GamePlayer(account.Id, player));
                }
                Debug.WriteLine($"Players added: {GameInstance.Instance.PlayerCount}");
            }
        }

        [Command("project_players")]
        public void ProjectPlayers() => 
            Debug.WriteLine($"Players Connected: {GameInstance.Instance.PlayerCount} / {Players.Count()}");

        [Command("project_sync")]
        public void ProjectSync()
        {

        }
    }
}
