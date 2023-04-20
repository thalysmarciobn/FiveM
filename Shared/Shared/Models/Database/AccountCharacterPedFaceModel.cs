using Shared.Enumerations;
using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterPedFaceModel : ICharacterComponent
    {
        public long CharacterId { get; set; }
        public FaceShapeEnum Index { get; set; }
        public float Scale { get; set; }
    }
}