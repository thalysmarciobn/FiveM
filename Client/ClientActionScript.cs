using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client.Core;
using Client.Extensions;
using Client.Helper;
using Mono.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Models.Database;
using Shared.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class ClientActionScript : BaseScript
    {
        private IList<Prompt> Prompts { get; } = new List<Prompt>();
        private Queue<long> ServicesToAction { get; } = new Queue<long>();
        private Dictionary<long, PromptServiceData> ServicesInAction { get; } = new Dictionary<long, PromptServiceData>();

        public ClientActionScript()
        {
            Debug.WriteLine("[PROJECT] Script: MarkScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
            EventHandlers[EventName.External.Client.OnClientResourceStop] += new Action<string>(OnClientResourceStop);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            TriggerServerEvent(EventName.Server.GetServiceVehicles, new Action<string>((arg) =>
            {
                var vehicles = JsonConvert.DeserializeObject<ICollection<ServerVehicleService>>(arg);
                foreach (var vehicle in vehicles)
                {
                    Prompts.Add(new Prompt(PromptService.ServiceCar, vehicle.Id, new PromptConfig
                    {
                        Key = (Control)vehicle.Key,
                        KeyLabel = GetControlInstructionalButton(2, vehicle.Key, 1),
                        TextLabel = vehicle.Title,
                        Font = 0,
                        Scale = 0.4f,
                        Coords = new Vector3(vehicle.MarkX, vehicle.MarkY, vehicle.MarkZ),
                        Origin = new Vector2(0, 0),
                        Offset = new Vector3(0, 0, 0),
                        Margin = 0.008f,
                        Padding = 0.004f,
                        TextOffset = 0,
                        ButtonSize = 0.015f,
                        BackgroundColor = new RGBAColor(0, 0, 0, 100),
                        LabelColor = new RGBAColor(255, 255, 255, 255),
                        ButtonColor = new RGBAColor(255, 255, 255, 255),
                        KeyColor = new RGBAColor(0, 0, 0, 255),
                        DrawDistance = 4.0f,
                        InteractDistance = 2.0f
                    }));
                }

                foreach (var prompt in Prompts)
                    prompt.Update();
            }));
        }

        private void OnClientResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            if (GlobalVariables.CurrentPromptServiceVehicle != null)
            {
                var driverId = GlobalVariables.CurrentPromptServiceVehicle.DriverEntityId;
                var vehicleId = GlobalVariables.CurrentPromptServiceVehicle.VehicleEntityId;

                if (DoesEntityExist(driverId))
                    DeleteEntity(ref driverId);

                if (DoesEntityExist(vehicleId))
                    DeleteVehicle(ref vehicleId);
            }
        }

        [Tick]
        public Task OnTickDraw()
        {
            var localPlayer = Game.Player;
            var localPlayerPed = Game.PlayerPed;
            var localPlayerCharacter = localPlayer.Character;

            if (GlobalVariables.CurrentPromptServiceVehicle != null)
            {
                var blipId = GlobalVariables.CurrentPromptServiceVehicle.Blip;
                var driverId = GlobalVariables.CurrentPromptServiceVehicle.DriverEntityId;
                var vehicleId = GlobalVariables.CurrentPromptServiceVehicle.VehicleEntityId;

                var distance = GetDistanceBetweenCoords(localPlayerCharacter.Position.X, localPlayerCharacter.Position.Y, localPlayerCharacter.Position.Z, GlobalVariables.CurrentPromptServiceVehicle.X, GlobalVariables.CurrentPromptServiceVehicle.Y, GlobalVariables.CurrentPromptServiceVehicle.Z, true);

                if (distance < 20.0f)
                    localPlayer.CanControlCharacter = true;

                if (DoesEntityExist(vehicleId))
                {
                    if (!IsPedInVehicle(localPlayerPed.Handle, vehicleId, false))
                    {
                        DeleteEntity(ref driverId);
                        while (DoesEntityExist(driverId))
                            Wait(0);

                        RemoveBlip(ref blipId);
                        while (DoesBlipExist(blipId))
                            Wait(0);

                        DeleteVehicle(ref vehicleId);
                        while (DoesEntityExist(vehicleId))
                            Wait(0);

                        ServicesInAction.Remove(GlobalVariables.CurrentPromptServiceVehicle.ValueId);
                        GlobalVariables.CurrentPromptServiceVehicle = null;
                        TriggerServerEvent(EventName.Server.SetPassive, false);
                    }
                }
                else
                {
                    ServicesInAction.Remove(GlobalVariables.CurrentPromptServiceVehicle.ValueId);
                    GlobalVariables.CurrentPromptServiceVehicle = null;
                    localPlayer.CanControlCharacter = true;
                    TriggerServerEvent(EventName.Server.SetPassive, false);
                }
            }

            foreach (var prompt in Prompts)
            {
                var drawDistance = prompt.Config.DrawDistance;
                var interactDistance = prompt.Config.InteractDistance;
                var coords = prompt.Config.Coords;

                var distance = GetDistanceBetweenCoords(localPlayerCharacter.Position.X, localPlayerCharacter.Position.Y, localPlayerCharacter.Position.Z, coords.X, coords.Y, coords.Z, true);
                if (distance < drawDistance)
                {
                    if (distance < interactDistance)
                    {
                        if (IsControlJustPressed(0, (int)prompt.Config.Key))
                        {
                            prompt.IsPressed = true;
                            prompt.IsDrawPressed = true;
                        }
                        prompt.CanInteract = true;
                    }
                    else
                    {
                        prompt.IsPressed = false;
                        prompt.CanInteract = false;
                    }

                    prompt.Draw();
                }
                else
                {
                    prompt.IsPressed = false;
                    prompt.CanInteract = false;
                }
                if (prompt.IsPressed)
                {
                    if (!ServicesInAction.ContainsKey(prompt.ValueId))
                    {
                        ServicesInAction.Add(prompt.ValueId, new PromptServiceData
                        {
                            ValueId = prompt.ValueId,
                            Service = prompt.Service
                        });
                        ServicesToAction.Enqueue(prompt.ValueId);
                    }
                }
            }
            return Task.FromResult(0);
        }

        [Tick]
        public Task OnTickAction()
        {
            var localPlayer = Game.Player;
            var localPlayerPed = Game.PlayerPed;

            if (IsControlJustPressed(0, GlobalVariables.Key.OpenPanel))
            {
                GlobalVariables.Hud.PanelOpened = !GlobalVariables.Hud.PanelOpened;

                var opened = GlobalVariables.Hud.PanelOpened;

                SetNuiFocus(opened, opened);
                SetNuiFocusKeepInput(opened);

                DisableControlAction(2, (int)Control.Attack, opened);

                NuiHelper.SendMessage(new NuiMessage
                {
                    Action = "interface",
                    Key = "panel",
                    Params = new[] { opened ? "true" : "false" }
                });
                return Task.FromResult(0);
            }

            while (ServicesToAction.Count > 0)
            {
                var serviceId = ServicesToAction.Dequeue();

                var service = ServicesInAction.FirstOrDefault(x => x.Key == serviceId);

                var serviceType = service.Value.Service;

                if (serviceType == PromptService.ServiceCar)
                    TriggerServerEvent(EventName.Server.SpawnVehicleService, serviceId, new Action<string>(async (arg) =>
                    {
                        var vehicle = JsonConvert.DeserializeObject<SpawnServerVehicle>(arg);

                        DoScreenFadeOut(500);
                        while (IsScreenFadingOut())
                            await Delay(0);

                        while (!NetworkDoesEntityExistWithNetworkId(vehicle.NetworkId))
                            Wait(0);

                        var vehicleEntity = NetworkGetEntityFromNetworkId(vehicle.NetworkId);
                        while (!DoesEntityExist(vehicleEntity))
                            Wait(0);

                        if (!IsEntityAVehicle(vehicleEntity))
                            return;

                        var blip = AddBlipForEntity(vehicleEntity);
                        SetBlipColour(blip, (int)BlipColor.Yellow);
                        BeginTextCommandSetBlipName("STRING");
                        AddTextComponentString($"TAXI");
                        EndTextCommandSetBlipName(blip);

                        if (!HasModelLoaded((uint)PedHash.Farmer01AMM))
                            RequestModel((uint)PedHash.Farmer01AMM);
                        while (!HasModelLoaded((uint)PedHash.Farmer01AMM)) await Delay(1000);

                        var driver = CreatePedInsideVehicle(vehicleEntity, 1, (uint) PedHash.Farmer01AMM, -1, true, true);

                        while (!DoesEntityExist(driver))
                            Wait(0);

                        SetPedCanBeTargetted(driver, false);
                        SetPedCanBeDraggedOut(driver, false);
                        SetPedCombatAbility(driver, 40);

                        //TaskEnterVehicle(localPlayerPed.Handle, vehicleEntity, -1, (int)VehicleSeat.LeftRear, 0f, 0, 0);
                        //Game.PlayerPed.Task.EnterVehicle(vehicleEntity, VehicleSeat.LeftRear));
                        SetPedIntoVehicle(localPlayerPed.Handle, vehicleEntity, (int) VehicleSeat.LeftRear);

                        localPlayer.CanControlCharacter = false;

                        while (!IsPedInVehicle(localPlayerPed.Handle, vehicleEntity, false))
                            Wait(0);

                        TriggerServerEvent(EventName.Server.SetPassive, true);

                        DoScreenFadeIn(500);
                        while (IsScreenFadingIn())
                            await Delay(0);

                        var speed = 20f;
                        // https://vespura.com/fivem/drivingstyle/
                        var drivingStyle = 0; // 15;
                        var stopRange = 8.0f;

                        SetDriverAbility(driver, 1.0f);
                        SetDriverAggressiveness(driver, 0.0f);

                        SetDriveTaskDrivingStyle(driver, drivingStyle);

                        TaskVehicleDriveWander(driver, vehicleEntity, speed, drivingStyle);

                        _ = Task.Factory.StartNew(async () =>
                        {
                            await Delay(5000);
                            
                            TaskVehicleDriveToCoordLongrange(driver, vehicleEntity, vehicle.Model.DriveToX, vehicle.Model.DriveToY, vehicle.Model.DriveToZ, speed, drivingStyle, stopRange);
                        });
                        
                        Screen.ShowNotification(vehicle.Model.Title, true);

                        GlobalVariables.CurrentPromptServiceVehicle = new PromptServiceVehicle
                        {
                            Blip = blip,
                            ValueId = service.Value.ValueId,
                            DriverEntityId = driver,
                            VehicleEntityId = vehicleEntity,
                            X = vehicle.Model.DriveToX,
                            Y = vehicle.Model.DriveToY,
                            Z = vehicle.Model.DriveToZ
                        };
                    }));
            }
            return Task.FromResult(0);
        }
    }
}
