using Shared.Enumerations;

namespace Shared.Models.Database
{
    public class AccountCharacterPedComponentModel
    {
        public long CharacterId { get; set; }
        public ComponentVariationEnum ComponentId { get; set; }
        public int DrawableId { get; set; }
        public int TextureId { get; set; }
        public int PalleteId { get; set; }
    }
}