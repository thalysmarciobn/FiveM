using CitizenFX.Core;
using Shared.Models.Database;

namespace Server.Core.Game
{
    public class GamePlayer
    {
        public GamePlayer(Player player, AccountModel account)
        {
            Player = player;
            Account = account;
        }

        public Player Player { get; }
        public AccountModel Account { get; }
    }
}