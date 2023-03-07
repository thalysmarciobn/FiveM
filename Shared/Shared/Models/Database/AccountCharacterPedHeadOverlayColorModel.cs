using Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class AccountCharacterPedHeadOverlayColorModel
    {
        public long CharacterId { get; set; }
        public OverlayEnum OverlayId { get; set; }
        public int ColorType { get; set; }
        public int ColorId { get; set; }
        public int SecondColorId { get; set; }
    }
}
