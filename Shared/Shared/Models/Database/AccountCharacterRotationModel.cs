using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterRotationModel : ICharacterComponent, ICoord
    {
        public long CharacterId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}