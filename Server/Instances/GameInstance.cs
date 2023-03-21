using CitizenFX.Core;
using Server.Core;
using Server.Core.Game;
using Shared.Models.Database;
using Shared.Models.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Server.Instances
{
    public class GameInstance : AbstractInstance<GameInstance>
    {
        private ConcurrentDictionary<string, GamePlayer> Players = new ConcurrentDictionary<string, GamePlayer>();
        public int PlayerCount => Players.Count;
        public bool ContainsPlayer(string license) => Players.ContainsKey(license);
        public bool AddPlayer(string license, GamePlayer model) => Players.TryAdd(license, model);
        public bool RemovePlayer(string license, out GamePlayer model) => Players.TryRemove(license, out model);
        public bool GetPlayer(string license, out GamePlayer model) => Players.TryGetValue(license, out model);
        public ICollection<GamePlayer> GetPlayers => Players.Values;


        private ConcurrentDictionary<long, ServerVehicleService> Vehicles = new ConcurrentDictionary<long, ServerVehicleService>();
        public int VehicleCount => Vehicles.Count;
        public bool AddVehicle(long id, ServerVehicleService model) => Vehicles.TryAdd(id, model);
        public bool RemoveVehicle(long id, out ServerVehicleService model) => Vehicles.TryRemove(id, out model);
        public bool GetVehicle(long id, out ServerVehicleService model) => Vehicles.TryGetValue(id, out model);
        public ICollection<ServerVehicleService> GetVehicles => Vehicles.Values;


        private ConcurrentDictionary<long, SpawnServerVehicle> SpawnVehicles = new ConcurrentDictionary<long, SpawnServerVehicle>();
        public int SpawnVehicleCount => SpawnVehicles.Count;
        public bool AddSpawnVehicle(long id, SpawnServerVehicle model) => SpawnVehicles.TryAdd(id, model);
        public bool RemoveSpawnVehicle(long id, out SpawnServerVehicle model) => SpawnVehicles.TryRemove(id, out model);
        public bool GetSpawnVehicle(long id, out SpawnServerVehicle model) => SpawnVehicles.TryGetValue(id, out model);
        public bool ContainsSpawnVehicle(long id) => SpawnVehicles.ContainsKey(id);
        public ICollection<SpawnServerVehicle> GetSpawnVehicles => SpawnVehicles.Values;


        private ConcurrentDictionary<int, ServerPlayer> PlayersData { get; } = new ConcurrentDictionary<int, ServerPlayer>();
        public int PlayerDataCount => PlayersData.Count;
        public void SetPlayerData(int id, ServerPlayer data) => PlayersData.AddOrUpdate(id, data, (key, oldValue) => data);
        public bool RemovePlayerData(int id) => PlayersData.TryRemove(id, out var serverPlayer);
        public ServerPlayer GetOrAddPlayerData(int id)
        {
            if (PlayersData.TryGetValue(id, out var serverPlayer))
            {
                return serverPlayer;
            }

            var newServerPlayer = new ServerPlayer
            {
                ServerId = id,
                IsPassive = false
            };

            PlayersData.AddOrUpdate(id, newServerPlayer, (key, oldValue) => newServerPlayer);
            return newServerPlayer;
        }
        public bool GetPlayerIsPassive(int id) => PlayersData.TryGetValue(id, out var serverPlayer) && serverPlayer.IsPassive;
        public ICollection<KeyValuePair<int, ServerPlayer>> GetPlayerDataList => PlayersData.ToImmutableList();


        private ConcurrentDictionary<long, BlipModel> BlipModels { get; } = new ConcurrentDictionary<long, BlipModel>();
        public int BlipCount => BlipModels.Count;
        public void AddBlip(long id, BlipModel model) => BlipModels.AddOrUpdate(id, model, (key, oldValue) => model);
        public ICollection<KeyValuePair<long, BlipModel>> GetBlipList => BlipModels.ToImmutableList();

    }
}
