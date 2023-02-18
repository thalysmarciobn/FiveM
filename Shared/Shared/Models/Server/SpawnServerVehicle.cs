using Shared.Enumerations;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Server
{
    public class SpawnServerVehicle
    {
        public int ServerId { get; set; }
        public int NetworkId { get; set; }
        public ServerVehicleService Model { get; set; }
    }
}
