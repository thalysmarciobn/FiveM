using Shared.Interface;

namespace Shared.Models.Database
{
    public class GarageModel : ICoord
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Tax { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}