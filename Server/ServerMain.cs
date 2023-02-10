using CitizenFX.Core;
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
            Players.ToList().ForEach(player =>
            {
            });
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
