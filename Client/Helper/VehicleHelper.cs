using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Shared.Models.Database;

namespace Client.Helper
{
    public static class VehicleHelper
    {
        public static void DataToVehicle(VehicleModel data, int vehicle)
        {
            SetVehicleColours(vehicle, data.PrimaryColour, data.SecondaryColour);
            SetVehicleDashboardColour(vehicle, data.DashboardColor);
			SetVehicleInteriorColour(vehicle, data.InteriorColor);
			SetVehicleExtraColours(vehicle, data.PearlColour, data.WheelColour);
			SetVehicleHeadlightsColour(vehicle, data.HeadlightsColour);
			SetVehicleNumberPlateTextIndex(vehicle, data.NumberPlateTextIndex);
			SetVehicleWindowTint(vehicle, data.WindowTint);
			SetVehicleBodyHealth(vehicle, data.BodyHealth);
			SetVehicleDirtLevel(vehicle, data.DirtLevel);
			SetVehicleEngineHealth(vehicle, data.EngineHealth);
			SetVehiclePetrolTankHealth(vehicle, data.PetrolTankHealth);
			SetVehicleLivery(vehicle, data.Livery);
			SetVehicleNumberPlateText(vehicle, data.NumberPlateText);
			SetVehicleRoofLivery(vehicle, data.RoofLivery);
			SetVehicleWheelType(vehicle, data.WheelType);
            SetVehicleTyreSmokeColor(vehicle, data.TyreSmokeColorR, data.TyreSmokeColorG, data.TyreSmokeColorB);
            SetVehicleCustomPrimaryColour(vehicle, data.CustomPrimaryColourR, data.CustomPrimaryColourG, data.CustomPrimaryColourB);
            SetVehicleCustomSecondaryColour(vehicle, data.CustomSecondaryColourR, data.CustomSecondaryColourG, data.CustomSecondaryColourB);
        }
    }
}