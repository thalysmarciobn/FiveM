using System;

namespace Shared.Models.Messages
{
    public class ServerTimeSyncMessage : IDisposable
    {
        public uint Weather { get; set; }
        public float RainLevel { get; set; }
        public float WindSpeed { get; set; }
        public float WindDirection { get; set; }
        public long Ticks { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}