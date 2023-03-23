using Shared.Enumerations;

namespace Shared.Models.Database
{
    public class AccountCharacterPedFaceModel
    {
        public long CharacterId { get; set; }
        public FaceShapeEnum Index { get; set; }
        public float Scale { get; set; }
    }
}