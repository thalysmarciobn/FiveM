using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class ServerVehicleService
    {
        public long Id { get; set; }
        public uint Model { get; set; }
        public uint Driver { get; set; }
        public bool IsSpawned { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
