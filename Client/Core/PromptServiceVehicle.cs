using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core
{
    public class PromptServiceVehicle
    {
        public int VehicleId { get; set; }
        public int VehicleNetworkId { get; set; }
        public int VehicleEntityId { get; set; }
        public int DriverId { get; set; }
        public int DriverNetworkId { get; set; }
        public int DriverEntityId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
