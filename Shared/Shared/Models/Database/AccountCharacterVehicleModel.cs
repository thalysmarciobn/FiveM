using Shared.Interface;

namespace Shared.Models.Database
{
    public class AccountCharacterVehicleModel : ICharacterComponent
    {
        public long CharacterId { get; set; }
        public uint Model { get; set; }
        public float Health { get; set; }
        public float DirtLevel { get; set; }
        public string Plate { get; set; }
    }
}