using CitizenFX.Core;
using Server.Core;
using Server.Core.Game;
using Shared.Models.Database;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Server.Instances
{
    public class GameInstance : AbstractInstance<GameInstance>
    {
        private ConcurrentDictionary<string, GamePlayer> Players = new ConcurrentDictionary<string, GamePlayer>();

        public bool ContainsPlayer(string license) => Players.ContainsKey(license);

        public bool AddPlayer(GamePlayer model) => Players.TryAdd(model.License, model);

        public bool RemovePlayer(string license, out GamePlayer model) => Players.TryRemove(license, out model);

        public bool GetPlayer(string license, out GamePlayer model) => Players.TryGetValue(license, out model);

        public ICollection<GamePlayer> GetPlayers => Players.Values;
    }
}
