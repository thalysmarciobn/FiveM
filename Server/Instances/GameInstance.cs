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

        private ConcurrentDictionary<long, ServerVehicleService> Vehicles = new ConcurrentDictionary<long, ServerVehicleService>();

        public int PlayerCount => Players.Count;
        public int VehicleCount => Vehicles.Count;

        public bool ContainsPlayer(string license) => Players.ContainsKey(license);

        public bool AddPlayer(string license, GamePlayer model) => Players.TryAdd(license, model);

        public bool RemovePlayer(string license, out GamePlayer model) => Players.TryRemove(license, out model);

        public bool GetPlayer(string license, out GamePlayer model) => Players.TryGetValue(license, out model);

        public ICollection<GamePlayer> GetPlayers => Players.Values;

        public bool AddVehicle(long id, ServerVehicleService model) => Vehicles.TryAdd(id, model);

        public bool RemoveVehicle(long id, out ServerVehicleService model) => Vehicles.TryRemove(id, out model);

        public bool GetVehicle(long id, out ServerVehicleService model) => Vehicles.TryGetValue(id, out model);

        public ICollection<ServerVehicleService> GetVehicles => Vehicles.Values;
    }
}
