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
                // Quando a resource sofre reload os dados no registro GameInstance vão ser perdido
                // Aqui será forçado uma requisição novamente dos dados com os platers conectados
                TriggerClientEvent(EventName.Server.ProjectPlayerRequestData);
            });
        }

        [EventHandler(EventName.Server.ProjectPlayerReceivedData)]
        public void ProjectPlayerReceivedData()
        {

        }

        [Command("project_players")]
        public void ProjectPlayers() => 
            Debug.WriteLine($"Players Connected: {GameInstance.Instance.PlayerCount} / {Players.Count()}");
    }
}
