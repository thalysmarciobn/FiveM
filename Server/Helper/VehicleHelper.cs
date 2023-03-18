﻿using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Helper
{
    public static class VehicleHelper
    {
        public static VehicleModel VehicleToData(uint model, int veh)
        {
            var data = new VehicleModel
            {
                Model = model,
                Livery = GetVehicleLivery(veh),
                BodyHealth = GetVehicleBodyHealth(veh),
                DirtLevel = GetVehicleDirtLevel(veh),
                DoorLockStatus = GetVehicleDoorLockStatus(veh),
                DoorsLockedForPlayer = GetVehicleDoorsLockedForPlayer(veh),
                DoorStatus = GetVehicleDoorStatus(veh),
                EngineHealth = GetVehicleEngineHealth(veh),
                Handbrake = GetVehicleHandbrake(veh),
                HeadlightsColour = GetVehicleHeadlightsColour(veh),
                HomingLockonState = GetVehicleHomingLockonState(veh),
                NumberPlateText = GetVehicleNumberPlateText(veh),
                NumberPlateTextIndex = GetVehicleNumberPlateTextIndex(veh),
                WheelType = GetVehicleWheelType(veh),
                WindowTint = GetVehicleWindowTint(veh),
            };

            var dashboardColor = data.DashboardColor;

            GetVehicleDashboardColour(veh, ref dashboardColor);

            var interiorColor = data.InteriorColor;

            GetVehicleInteriorColour(veh, ref interiorColor);

            var lightsOn = data.LightsOn;
            var highbeamsOn = data.HighbeamsOn;

            GetVehicleLightsState(veh, ref lightsOn, ref highbeamsOn);

            var primaryColour = data.PrimaryColour;
            var secondaryColour = data.SecondaryColour;

            GetVehicleColours(veh, ref primaryColour, ref secondaryColour);

            var pearlColour = data.PearlColour;
            var wheelColour = data.WheelColour;

            GetVehicleExtraColours(veh, ref pearlColour, ref wheelColour);

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

            var tyreSmokeColorR = data.TyreSmokeColorR;
            var tyreSmokeColorG = data.TyreSmokeColorG;
            var tyreSmokeColorB = data.TyreSmokeColorB;

            GetVehicleTyreSmokeColor(veh, ref tyreSmokeColorR, ref tyreSmokeColorG, ref tyreSmokeColorB);

            return data;
        }
    }
}
