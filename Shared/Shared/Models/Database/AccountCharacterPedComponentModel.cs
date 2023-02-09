using Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class AccountCharacterPedComponentModel
    {
        public long CharacterId { get; set; }
        public ComponentVariationEnum ComponentId { get; set; }
        public int Index { get; set; }
        public int Texture { get; set; }
    }
}
