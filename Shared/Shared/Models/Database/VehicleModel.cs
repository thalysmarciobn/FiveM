namespace Shared.Models.Database
{
    public class VehicleModel
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public uint Model { get; set; }
        public int Livery { get; set; }
        public int RoofLivery { get; set; }
        public float BodyHealth { get; set; }
        public int DashboardColor { get; set; }
        public float DirtLevel { get; set; }
        public int DoorLockStatus { get; set; }
        public int DoorsLockedForPlayer { get; set; }
        public int DoorStatus { get; set; }
        public float EngineHealth { get; set; }
        public bool Handbrake { get; set; }
        public int HeadlightsColour { get; set; }
        public int HomingLockonState { get; set; }
        public int InteriorColor { get; set; }
        public bool LightsOn { get; set; }
        public bool HighbeamsOn { get; set; }
        public string NumberPlateText { get; set; }
        public int NumberPlateTextIndex { get; set; }
        public int WheelType { get; set; }
        public int WindowTint { get; set; }
        public float PetrolTankHealth { get; set; }
        public int PrimaryColour { get; set; }
        public int SecondaryColour { get; set; }
        public int PearlColour { get; set; }
        public int WheelColour { get; set; }
        public int CustomPrimaryColourR { get; set; }
        public int CustomPrimaryColourG { get; set; }
        public int CustomPrimaryColourB { get; set; }
        public int CustomSecondaryColourR { get; set; }
        public int CustomSecondaryColourG { get; set; }
        public int CustomSecondaryColourB { get; set; }
        public int TyreSmokeColorR { get; set; }
        public int TyreSmokeColorG { get; set; }
        public int TyreSmokeColorB { get; set; }
    }
}