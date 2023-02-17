using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Game
{
    public class VehicleService
    {
        public int VehicleId { get; set; }
        public int VehicleNetworkId { get; set; }
        public int DriverPedId { get; set; }
        public int DriverPedNetworkId { get; set; }
        public ServerVehicleService Model { get; set; }
    }
}
