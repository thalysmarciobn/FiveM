using Shared.Models.Database;
using System;
using System.Collections.Generic;

namespace Shared.Models.Messages
{
    public class BlipListMessage : IDisposable
    {
        public ICollection<KeyValuePair<long, BlipModel>> List { get; set; }

        public void Dispose()
        {
            List = null;
            GC.SuppressFinalize(this);
        }
    }
}