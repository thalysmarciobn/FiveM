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
        public Player Player { get; }
        public AccountModel Account { get; }

        public GamePlayer(Player player, AccountModel account)
        {
            Player = player;
            Account = account;
        }
    }
}
