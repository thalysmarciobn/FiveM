using Shared.Enumerations;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Server
{
    public class ServerPlayer
    {
        public int ServerId { get; set; }
        public bool IsPassive { get; set; }
    }
}
