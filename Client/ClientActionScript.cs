using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Client.Core;
using Client.Core.Color;
using Client.Core.Prompts;
using Client.Helper;
using Shared.Helper;
using Shared.Models.Database;
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;
using static Client.GlobalVariables;

namespace Client
{
    public class ClientActionScript : BaseScript
    {
        public ClientActionScript()
        {
            Debug.WriteLine("[PROJECT] Script: MarkScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
            EventHandlers[EventName.External.Client.OnClientResourceStop] += new Action<string>(OnClientResourceStop);
        }

        private IReadOnlyCollection<Prompt> Prompts { get; set; }
        private Queue<long> ServicesToAction { get; } = new Queue<long>();

        private ConcurrentDictionary<long, PromptServiceData> ServicesInAction { get; } =
            new ConcurrentDictionary<long, PromptServiceData>();

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            TriggerServerEvent(EventName.Server.GetServiceVehicles, new Action<string>(arg =>
            {
                var collection = new List<Prompt>();
                var vehicles = JsonHelper.DeserializeObject<ICollection<ServerVehicleService>>(arg);
                foreach (var vehicle in vehicles)
                    collection.Add(new Prompt(PromptService.ServiceCar, vehicle.Id, new PromptConfig
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
                        BackgroundColor = new RGBA(0, 0, 0, 100),
                        LabelColor = new RGBA(255, 255, 255, 255),
                        ButtonColor = new RGBA(255, 255, 255, 255),
                        KeyColor = new RGBA(0, 0, 0, 255),
                        DrawDistance = 4.0f,
                        InteractDistance = 2.0f
                    }));

                Prompts = collection;

                foreach (var prompt in Prompts)
                    prompt.Update();
            }));
        }

        private void OnClientResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            if (G_Character.CurrentPromptServiceVehicle != null)
            {
                var driverId = G_Character.CurrentPromptServiceVehicle.DriverEntityId;
                var vehicleId = G_Character.CurrentPromptServiceVehicle.VehicleEntityId;

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

            if (G_Character.CurrentPromptServiceVehicle != null)
            {
                var currentPrompt = G_Character.CurrentPromptServiceVehicle;

                var blipId = currentPrompt.Blip;
                var driverId = currentPrompt.DriverEntityId;
                var vehicleId = currentPrompt.VehicleEntityId;

                var distance = GetDistanceBetweenCoords(localPlayerCharacter.Position.X,
                    localPlayerCharacter.Position.Y, localPlayerCharacter.Position.Z, currentPrompt.X, currentPrompt.Y,
                    currentPrompt.Z, true);

                if (distance < 20.0f)
                    localPlayer.CanControlCharacter = true;

                if (DoesEntityExist(vehicleId))
                {
                    if (!IsPedInVehicle(localPlayerPed.Handle, vehicleId, false))
                    {
                        DeleteEntity(ref driverId);
                        while (DoesEntityExist(driverId))
                            Wait(10);

                        RemoveBlip(ref blipId);
                        while (DoesBlipExist(blipId))
                            Wait(10);

                        DeleteVehicle(ref vehicleId);
                        while (DoesEntityExist(vehicleId))
                            Wait(10);

                        TriggerServerEvent(EventName.Server.SetPassive, false);
                        ServicesInAction.TryRemove(currentPrompt.ValueId, out var value);
                        G_Character.CurrentPromptServiceVehicle = null;
                    }
                }
                else
                {
                    localPlayer.CanControlCharacter = true;
                    TriggerServerEvent(EventName.Server.SetPassive, false);
                    ServicesInAction.TryRemove(currentPrompt.ValueId, out var value);
                    G_Character.CurrentPromptServiceVehicle = null;
                }
            }

            if (Prompts == null)
                return Task.FromResult(0);

            foreach (var prompt in Prompts)
            {
                var coords = prompt.Config.Coords;
                var drawDistance = prompt.Config.DrawDistance;

                var distance = GetDistanceBetweenCoords(localPlayerCharacter.Position.X,
                    localPlayerCharacter.Position.Y, localPlayerCharacter.Position.Z, coords.X, coords.Y, coords.Z,
                    true);
                if (distance < drawDistance)
                {
                    var interactDistance = prompt.Config.InteractDistance;
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
                    if (!ServicesInAction.ContainsKey(prompt.ValueId))
                    {
                        if (ServicesInAction.TryAdd(prompt.ValueId, new PromptServiceData
                        {
                            ValueId = prompt.ValueId,
                            Service = prompt.Service
                        }))
                            ServicesToAction.Enqueue(prompt.ValueId);
                    }
            }

            return Task.FromResult(0);
        }

        [Tick]
        public Task OnTickAction()
        {
            var localPlayer = Game.Player;
            var localPlayerPed = Game.PlayerPed;

            if (IsControlJustPressed(0, G_Key.OpenPanel))
            {
                G_Hud.PanelOpened = !G_Hud.PanelOpened;

                var opened = G_Hud.PanelOpened;

                SetNuiFocus(opened, opened);
                SetNuiFocusKeepInput(opened);

                NuiHelper.SendMessage("interface", "panel", new[] { opened ? "true" : "false" });
                return Task.FromResult(0);
            }

            if (IsControlJustPressed(0, G_Key.OpenInventory))
            {
                G_Hud.IventoryOpened = !G_Hud.IventoryOpened;

                var opened = G_Hud.IventoryOpened;

                SetNuiFocus(opened, opened);
                SetNuiFocusKeepInput(opened);

                NuiHelper.SendMessage("interface", "inventory", new[] { opened ? "true" : "false" });
                return Task.FromResult(0);
            }

            while (ServicesToAction.Count > 0)
            {
                var serviceId = ServicesToAction.Dequeue();

                var service = ServicesInAction.FirstOrDefault(x => x.Key == serviceId);

                var serviceType = service.Value.Service;

                if (serviceType == PromptService.ServiceCar)
                    TriggerServerEvent(EventName.Server.SpawnVehicleService, serviceId, new Action<string>(async arg =>
                    {
                        var vehicle = JsonHelper.DeserializeObject<SpawnServerVehicle>(arg);

                        DoScreenFadeOut(500);
                        while (IsScreenFadingOut())
                            await Delay(10);

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
                        AddTextComponentString("TAXI");
                        EndTextCommandSetBlipName(blip);

                        if (!HasModelLoaded((uint)PedHash.Farmer01AMM))
                            RequestModel((uint)PedHash.Farmer01AMM);
                        while (!HasModelLoaded((uint)PedHash.Farmer01AMM)) await Delay(1000);

                        var driver = CreatePedInsideVehicle(vehicleEntity, 1, (uint)PedHash.Farmer01AMM, -1, true,
                            true);

                        while (!DoesEntityExist(driver))
                            Wait(0);

                        SetPedCanBeTargetted(driver, false);
                        SetPedCanBeDraggedOut(driver, false);
                        SetPedCombatAbility(driver, 40);

                        //TaskEnterVehicle(localPlayerPed.Handle, vehicleEntity, -1, (int)VehicleSeat.LeftRear, 0f, 0, 0);
                        //Game.PlayerPed.Task.EnterVehicle(vehicleEntity, VehicleSeat.LeftRear));
                        SetPedIntoVehicle(localPlayerPed.Handle, vehicleEntity, (int)VehicleSeat.LeftRear);

                        localPlayer.CanControlCharacter = false;

                        while (!IsPedInVehicle(localPlayerPed.Handle, vehicleEntity, false))
                            Wait(0);

                        TriggerServerEvent(EventName.Server.SetPassive, true);

                        DoScreenFadeIn(500);
                        while (IsScreenFadingIn())
                            await Delay(10);

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

                            if (IsPedInVehicle(localPlayerPed.Handle, vehicleEntity, false))
                                TaskVehicleDriveToCoordLongrange(driver, vehicleEntity, vehicle.Model.DriveToX,
                                    vehicle.Model.DriveToY, vehicle.Model.DriveToZ, speed, drivingStyle, stopRange);
                        });

                        Screen.ShowNotification(vehicle.Model.Title, true);

                        G_Character.CurrentPromptServiceVehicle = new PromptServiceVehicle
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