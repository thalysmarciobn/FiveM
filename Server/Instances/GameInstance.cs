using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using Server.Core;
using Server.Core.Game;
using Shared.Models.Database;
using Shared.Models.Server;

namespace Server.Instances
{
    public class GameInstance : AbstractInstance<GameInstance>
    {
        private readonly ConcurrentDictionary<string, GamePlayer> Players =
            new ConcurrentDictionary<string, GamePlayer>();


        private readonly ConcurrentDictionary<long, SpawnServerVehicle> SpawnVehicles =
            new ConcurrentDictionary<long, SpawnServerVehicle>();


        private readonly ConcurrentDictionary<long, ServerVehicleService> Vehicles =
            new ConcurrentDictionary<long, ServerVehicleService>();

        public int PlayerCount => Players.Count;
        public ICollection<GamePlayer> GetPlayers => Players.Values;
        public int VehicleCount => Vehicles.Count;
        public ICollection<ServerVehicleService> GetVehicles => Vehicles.Values;
        public int SpawnVehicleCount => SpawnVehicles.Count;
        public ICollection<SpawnServerVehicle> GetSpawnVehicles => SpawnVehicles.Values;


        private ConcurrentDictionary<int, ServerPlayer> PlayersData { get; } =
            new ConcurrentDictionary<int, ServerPlayer>();

        public int PlayerDataCount => PlayersData.Count;
        public ICollection<KeyValuePair<int, ServerPlayer>> GetPlayerDataList => PlayersData.ToImmutableList();


        private ConcurrentDictionary<long, BlipModel> BlipModels { get; } = new ConcurrentDictionary<long, BlipModel>();
        public int BlipCount => BlipModels.Count;
        public ICollection<KeyValuePair<long, BlipModel>> GetBlipList => BlipModels.ToImmutableList();

        public bool ContainsPlayer(string license)
        {
            return Players.ContainsKey(license);
        }

        public bool AddPlayer(string license, GamePlayer model)
        {
            return Players.TryAdd(license, model);
        }

        public bool RemovePlayer(string license, out GamePlayer model)
        {
            return Players.TryRemove(license, out model);
        }

        public bool GetPlayer(string license, out GamePlayer model)
        {
            return Players.TryGetValue(license, out model);
        }

        public bool AddVehicle(long id, ServerVehicleService model)
        {
            return Vehicles.TryAdd(id, model);
        }

        public bool RemoveVehicle(long id, out ServerVehicleService model)
        {
            return Vehicles.TryRemove(id, out model);
        }

        public bool GetVehicle(long id, out ServerVehicleService model)
        {
            return Vehicles.TryGetValue(id, out model);
        }

        public bool AddSpawnVehicle(long id, SpawnServerVehicle model)
        {
            return SpawnVehicles.TryAdd(id, model);
        }

        public bool RemoveSpawnVehicle(long id, out SpawnServerVehicle model)
        {
            return SpawnVehicles.TryRemove(id, out model);
        }

        public bool GetSpawnVehicle(long id, out SpawnServerVehicle model)
        {
            return SpawnVehicles.TryGetValue(id, out model);
        }

        public bool ContainsSpawnVehicle(long id)
        {
            return SpawnVehicles.ContainsKey(id);
        }

        public void SetPlayerData(int id, ServerPlayer data)
        {
            PlayersData.AddOrUpdate(id, data, (key, oldValue) => data);
        }

        public bool RemovePlayerData(int id)
        {
            return PlayersData.TryRemove(id, out var serverPlayer);
        }

        public ServerPlayer GetOrAddPlayerData(int id)
        {
            if (PlayersData.TryGetValue(id, out var serverPlayer)) return serverPlayer;

            var newServerPlayer = new ServerPlayer
            {
                ServerId = id,
                IsPassive = false
            };

            PlayersData.AddOrUpdate(id, newServerPlayer, (key, oldValue) => newServerPlayer);
            return newServerPlayer;
        }

        public bool GetPlayerIsPassive(int id)
        {
            return PlayersData.TryGetValue(id, out var serverPlayer) && serverPlayer.IsPassive;
        }

        public void AddBlip(long id, BlipModel model)
        {
            BlipModels.AddOrUpdate(id, model, (key, oldValue) => model);
        }
    }
}