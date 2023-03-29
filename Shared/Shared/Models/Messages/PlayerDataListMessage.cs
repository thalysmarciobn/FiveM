using Shared.Models.Database;
using Shared.Models.Server;
using System;
using System.Collections.Generic;

namespace Shared.Models.Messages
{
    public class PlayerDataListMessage : IDisposable
    {
        public ICollection<KeyValuePair<int, ServerPlayer>> List { get; set; }

        public void Dispose()
        {
            List = null;
            GC.SuppressFinalize(this);
        }
    }
}