using Shared.Models.Database;
using System;

namespace Shared.Models.Message
{
    public class VehicleDataMessage : IDisposable
    {
        public int Status { get; set; }
        public VehicleModel Model { get; set; }

        public void Dispose()
        {
            Model = null;
            GC.SuppressFinalize(this);
        }
    }
}