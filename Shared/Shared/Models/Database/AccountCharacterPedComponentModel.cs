using Shared.Enumerations;
using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterPedComponentModel : ICharacterComponent, ICharacterPedComponent
    {
        public long CharacterId { get; set; }
        public ComponentVariationEnum ComponentId { get; set; }
        public int DrawableId { get; set; }
        public int TextureId { get; set; }
        public int PalleteId { get; set; }
    }
}