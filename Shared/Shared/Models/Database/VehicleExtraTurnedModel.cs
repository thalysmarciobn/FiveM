using Shared.Models.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class VehicleExtraTurnedModel
    {
        public long CharacterId { get; set; }
        public int Index { get; set; }
        public bool Enable { get; set; }
    }
}
