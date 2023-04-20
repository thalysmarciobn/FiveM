using Shared.Enumerations;
using Shared.Interface;
using System;
using System.Collections.Generic;

namespace Shared.Models.Database
{
    public class AccountCharacterItemModel : ICharacterComponent
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public long ItemId { get; set; }
        public int Slot { get; set; }
        public int Quantity { get; set; }
        public bool Equipped { get; set; }
        public bool Deleted { get; set; }
        public DateTime Time { get; set; }
    }
}