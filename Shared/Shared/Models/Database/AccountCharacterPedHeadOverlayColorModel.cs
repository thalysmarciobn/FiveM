using Shared.Enumerations;
using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterPedHeadOverlayColorModel : ICharacterComponent
    {
        public long CharacterId { get; set; }
        public OverlayEnum OverlayId { get; set; }
        public int ColorType { get; set; }
        public int ColorId { get; set; }
        public int SecondColorId { get; set; }
    }
}