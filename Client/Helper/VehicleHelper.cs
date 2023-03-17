using Mono.CSharp;
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
        public static bool IsVehicleExtraTurnedOn(int vehicleId, int index)
        {
            if (DoesExtraExist(vehicleId, index))
                return IsVehicleExtraTurnedOn(vehicleId, index);
            return false;
        }

        public static bool IsModOn(int entity, int index)
        {
            var isToggle = (index >= 17) && (index <= 22);
            if (isToggle)
                return IsToggleModOn(entity, index);
            return GetVehicleMod(entity, index) == 1 ? true : false;
        }
    }
}
