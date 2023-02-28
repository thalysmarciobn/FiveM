using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class VehicleModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public long CharacterId { get; set; }
    }
}
