using Shared.Enumerations;
using System;
using System.Collections.Generic;

namespace Shared.Models.Database
{
    public class AccountCharacterItemModel
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public long ItemId { get; set; }
        public float ItemValue { get; set; }
        public ItemCharacterEnum Type { get; set; }
        public bool Equipped { get; set; }
        public bool Deleted { get; set; }
        public DateTime Time { get; set; }
    }
}