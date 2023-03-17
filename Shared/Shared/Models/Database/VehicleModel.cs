using Shared.Models.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class VehicleModel
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public int Model { get; set; }
        public int Livery { get; set; }
        public string NumberPlateText { get; set; }
        public int NumberPlateTextIndex { get; set; }
        public int WheelType { get; set; }
        public int WindowTint { get; set; }
        public bool TyresCanBurst { get; set; }
        public bool ModVariation { get; set; }
        public int PrimaryColour { get; set; }
        public int SecondaryColour { get; set; }
        public int PearlColour { get; set; }
        public int WheelColour { get; set; }
        public int ModColour1PaintType { get; set; }
        public int ModColour1 { get; set; }
        public int ModColour1Pearlescent { get; set; }
        public int ModColour2PaintType { get; set; }
        public int ModColour2 { get; set; }
        public int CustomPrimaryColourR { get; set; }
        public int CustomPrimaryColourG { get; set; }
        public int CustomPrimaryColourB { get; set; }
        public int CustomSecondaryColourR { get; set; }
        public int CustomSecondaryColourG { get; set; }
        public int CustomSecondaryColourB { get; set; }
        public int NeonLightsColourR { get; set; }
        public int NeonLightsColourG { get; set; }
        public int NeonLightsColourB { get; set; }
        public int TyreSmokeColorR { get; set; }
        public int TyreSmokeColorG { get; set; }
        public int TyreSmokeColorB { get; set; }
        public IList<VehicleNeonLightModel> NeonLight { get; set; }
        public IList<VehicleExtraTurnedModel> ExtraTurnedOn { get; set; }
        public IList<VehicleModModel> ModOn { get; set; }
    }
}
