using Shared.Models.Database;

namespace Shared.Models.Server
{
    public class SpawnServerVehicle
    {
        public int ServerId { get; set; }
        public int NetworkId { get; set; }
        public ServerVehicleService Model { get; set; }
    }
}