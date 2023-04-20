using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterPedHeadModel : ICharacterComponent
    {
        public long CharacterId { get; set; }
        public int HairColorId { get; set; }
        public int HairHighlightColor { get; set; }
    }
}