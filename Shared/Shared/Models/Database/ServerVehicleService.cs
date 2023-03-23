namespace Shared.Models.Database
{
    public class ServerVehicleService
    {
        public long Id { get; set; }
        public uint Model { get; set; }
        public uint Driver { get; set; }
        public int Key { get; set; }
        public string Title { get; set; }
        public float MarkX { get; set; }
        public float MarkY { get; set; }
        public float MarkZ { get; set; }
        public float SpawnX { get; set; }
        public float SpawnY { get; set; }
        public float SpawnZ { get; set; }
        public float SpawnHeading { get; set; }
        public float DriveToX { get; set; }
        public float DriveToY { get; set; }
        public float DriveToZ { get; set; }
    }
}