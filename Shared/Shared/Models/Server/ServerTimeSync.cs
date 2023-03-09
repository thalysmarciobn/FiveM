using Shared.Enumerations;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Server
{
    public class ServerTimeSync
    {
        public uint Weather { get; set; }
        public float RainLevel { get; set; }
        public float WindSpeed { get; set; }
        public float WindDirection { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}
