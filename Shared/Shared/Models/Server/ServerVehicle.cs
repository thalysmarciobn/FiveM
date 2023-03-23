using Shared.Models.Database;

namespace Shared.Models.Server
{
    public class ServerVehicle
    {
        public int ServerId { get; set; }
        public int NetworkId { get; set; }
        public VehicleModel Data { get; set; }
    }
}