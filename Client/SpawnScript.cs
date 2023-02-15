using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client.Core;
using Mono.CSharp;
using MsgPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class SpawnScript : BaseScript
    {
        public SpawnScript()
        {
            Debug.WriteLine("[PROJECT] Script: SpawnScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            SetVehicleDensityMultiplierThisFrame(0.0f);

            SetPedDensityMultiplierThisFrame(0.0f);

            SetRandomVehicleDensityMultiplierThisFrame(0.0f);

            SetParkedVehicleDensityMultiplierThisFrame(0.0f);

            SetScenarioPedDensityMultiplierThisFrame(0.0f, 0.0f);

            SetGarbageTrucks(false);

            SetRandomBoats(false);

            SetCreateRandomCops(false);
            SetCreateRandomCopsNotOnScenarios(false);
            SetCreateRandomCopsOnScenarios(false);

            //var coords = GetEntityCoords(PlayerPedId(), true);

            //ClearAreaOfVehicles(coords.X, coords.Y, coords.Z, 1000, false, false, false, false, false);

            //RemoveVehiclesFromGeneratorsInArea(coords.X - 500.0f, coords.Y - 500.0f, coords.Z - 500.0f, coords.X + 500.0f, coords.Y + 500.0f, coords.Z + 500.0f);
            TriggerServerEvent(EventName.Server.SpawnRequest);
        }
    }
}
