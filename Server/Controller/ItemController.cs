using CitizenFX.Core;
using Server.Core;
using Shared.Enumerations;
using Shared.Models.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Server.Controller
{
    public class ItemController : AbstractController
    {
        private ConcurrentDictionary<long, ServerItemData> Items = new ConcurrentDictionary<long, ServerItemData>();

        public void AddItem(long itemId, int itemType)
        {
            var data = new ServerItemData
            {
                Type = (ItemTypeEnum)itemType
            };
            if (Items.TryAdd(itemId, data))
            {
                Debug.WriteLine($"[ItemController][{itemId}] new item added -> {data.Type}");
            }
        }
    }
}
