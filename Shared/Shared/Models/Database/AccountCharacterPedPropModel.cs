using Shared.Enumerations;
using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterPedPropModel : ICharacterComponent, ICharacterPedComponent
    {
        public long CharacterId { get; set; }
        public PropVariationEnum PropId { get; set; }
        public int ComponentId { get; set; }
        public int DrawableId { get; set; }
        public int TextureId { get; set; }
        public bool Attach { get; set; }
    }
}