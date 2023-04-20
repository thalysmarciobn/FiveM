using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterPositionModel : ICharacterComponent, ICoord
    {
        public long CharacterId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}