using Client.Helper;
using Mono.CSharp;
using Shared.Models.Database;
using Shared.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Extensions
{
    public static class VehicleExtensions
    {
        public static void CreateVehicleFromData(VehicleData data)
        {

        }

        public static VehicleModel VehicleToData(int veh)
        {
            var data = new VehicleModel
            {
                Model = GetEntityModel(veh),
                Livery = GetVehicleLivery(veh),
                NumberPlateText = GetVehicleNumberPlateText(veh),
                NumberPlateTextIndex = GetVehicleNumberPlateTextIndex(veh),
                WheelType = GetVehicleWheelType(veh),
                WindowTint = GetVehicleWindowTint(veh),
                TyresCanBurst = GetVehicleTyresCanBurst(veh),
                ModVariation = GetVehicleModVariation(veh, 23),
                NeonLight = new List<VehicleNeonLightModel>(),
                ExtraTurnedOn = new List<VehicleExtraTurnedModel>(),
                ModOn = new List<VehicleModModel>(),
            };

            var primaryColour = data.PrimaryColour;
            var secondaryColour = data.SecondaryColour;

            GetVehicleColours(veh, ref primaryColour, ref secondaryColour);

            var pearlColour = data.PearlColour;
            var wheelColour = data.WheelColour;

            GetVehicleExtraColours(veh, ref pearlColour, ref wheelColour);

            var modColour1PaintType = data.ModColour1PaintType;
            var modColour1 = data.ModColour1;
            var modColour1Pearlescent = data.ModColour1Pearlescent;

            GetVehicleModColor_1(veh, ref modColour1PaintType, ref modColour1, ref modColour1Pearlescent);

            var modColour2PaintType = data.ModColour2PaintType;
            var modColour2 = data.ModColour2;

            GetVehicleModColor_2(veh, ref modColour2PaintType, ref modColour2);

            var customPrimaryColourR = data.CustomPrimaryColourR;
            var customPrimaryColourG = data.CustomPrimaryColourG;
            var customPrimaryColourB = data.CustomPrimaryColourB;

            if (GetIsVehiclePrimaryColourCustom(veh))
                GetVehicleCustomPrimaryColour(veh, ref customPrimaryColourR, ref customPrimaryColourG, ref customPrimaryColourB);

            var customSecondaryColourR = data.CustomSecondaryColourR;
            var customSecondaryColourG = data.CustomSecondaryColourG;
            var customSecondaryColourB = data.CustomSecondaryColourB;

            if (GetIsVehicleSecondaryColourCustom(veh))
                GetVehicleCustomSecondaryColour(veh, ref customSecondaryColourR, ref customSecondaryColourG, ref customSecondaryColourB);

            var neonLightsColourR = data.NeonLightsColourR;
            var neonLightsColourG = data.NeonLightsColourG;
            var neonLightsColourB = data.NeonLightsColourB;

            GetVehicleNeonLightsColour(veh, ref neonLightsColourR, ref neonLightsColourG, ref neonLightsColourB);

            var tyreSmokeColorR = data.TyreSmokeColorR;
            var tyreSmokeColorG = data.TyreSmokeColorG;
            var tyreSmokeColorB = data.TyreSmokeColorB;

            GetVehicleTyreSmokeColor(veh, ref tyreSmokeColorR, ref tyreSmokeColorG, ref tyreSmokeColorB);

            for (int i = 0; i < 4; i++)
                data.NeonLight.Add(new VehicleNeonLightModel
                {
                    Index = i,
                    Enable = IsVehicleNeonLightEnabled(veh, i)
                });


            for (int i = 1; i <= 30; i++)
                if (DoesExtraExist(veh, i))
                {
                    data.ExtraTurnedOn.Add(new VehicleExtraTurnedModel
                    {
                        Index = i,
                        Enable = IsVehicleExtraTurnedOn(veh, i)
                    });
                }

            for (int i = 0; i < 50; i++)
                data.ModOn.Add(new VehicleModModel
                {
                    Index = i,
                    Enable = (i >= 17) && (i <= 22) ? IsToggleModOn(veh, i) : GetVehicleMod(veh, i) == 1
                });

            return data;
        }
    }
}
