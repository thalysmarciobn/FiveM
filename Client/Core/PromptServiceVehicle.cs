using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core
{
    public class PromptServiceVehicle
    {
        public int Blip { get; set; }
        public long ValueId { get; set; }
        public int DriverEntityId { get; set; }
        public int VehicleEntityId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
