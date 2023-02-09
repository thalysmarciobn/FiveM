using Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class AccountCharacterPedPropModel
    {
        public long CharacterId { get; set; }
        public PropVariationEnum PropId { get; set; }
        public int Index { get; set; }
        public int Texture { get; set; }
    }
}
