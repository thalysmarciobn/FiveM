using CitizenFX.Core;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Server.Core.Game
{
    public class GamePlayer
    {
        public long DatabaseId { get; set; }

        public string Name { get; set; }

        public string License { get; set; }

        public string EndPoint { get; set; }

        public bool Spawned { get; set; }

        public Player Player { get; }

        public GamePlayer(long databaseId, Player player)
        {
            Player = player;
            DatabaseId = databaseId;
            Name = player.Name;
            License = player.Identifiers["license"];
            EndPoint = player.EndPoint;
        }
    }
}
