using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core;
using Client.Extensions;
using Mono.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class MarkScript : BaseScript
    {
        private IList<Prompt> Prompts { get; } = new List<Prompt>();
        private PromptServiceVehicle CurrentPromptServiceVehicle { get; set; }
        private Queue<long> ServiceCarQueue { get; } = new Queue<long>();

        public MarkScript()
        {
            Tick += OnTick;
            Tick += OnTickQueue;
            Tick += async () => OnTickDraw();
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
                        KeyLabel = "E",
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

            if (CurrentPromptServiceVehicle != null)
            {
                var vehicleId = CurrentPromptServiceVehicle.VehicleEntityId;
                var driverId = CurrentPromptServiceVehicle.DriverEntityId;

                if (DoesEntityExist(driverId))
                    DeletePed(ref driverId);
                if (DoesEntityExist(vehicleId))
                    DeleteVehicle(ref vehicleId);
            }
        }

        private async Task OnTick()
        {
            if (CurrentPromptServiceVehicle != null)
            {
                var player = Game.Player;
                var playerPed = Game.PlayerPed;
                var playerCharacter = player.Character;
                var playerCoords = playerCharacter.Position;

                var vehicleId = CurrentPromptServiceVehicle.VehicleEntityId;
                var driverId = CurrentPromptServiceVehicle.DriverEntityId;

                var distance = GetDistanceBetweenCoords(playerCoords.X, playerCoords.Y, playerCoords.Z, CurrentPromptServiceVehicle.X, CurrentPromptServiceVehicle.Y, CurrentPromptServiceVehicle.Z, true);

                if (distance < 20.0f)
                    player.CanControlCharacter = true;

                if (DoesEntityExist(vehicleId))
                {
                    if (!IsPedInVehicle(playerPed.Handle, vehicleId, false))
                    {
                        if (DoesEntityExist(driverId))
                        {
                            DeletePed(ref driverId);
                            while (DoesEntityExist(driverId))
                                await Delay(10);
                        }
                        DeleteVehicle(ref vehicleId);
                        while (DoesEntityExist(vehicleId))
                            await Delay(10);
                        CurrentPromptServiceVehicle = null;
                    }
                }
                else
                {
                    if (DoesEntityExist(driverId))
                    {
                        DeletePed(ref driverId);
                        while (DoesEntityExist(driverId))
                            await Delay(10);
                    }
                    player.CanControlCharacter = true;
                    CurrentPromptServiceVehicle = null;
                }
            }
            await Delay(10);
        }

        private async Task OnTickQueue()
        {
            var player = Game.Player;
            var playerPed = Game.PlayerPed;
            var playerCharacter = player.Character;
            var playerCoords = playerCharacter.Position;

            while (ServiceCarQueue.Count > 0)
            {
                var serviceId = ServiceCarQueue.Dequeue();

                TriggerServerEvent(EventName.Server.SpawnVehicleService, serviceId, new Action<string>(async (arg) =>
                {
                    var vehicle = JsonConvert.DeserializeObject<ServerVehicleService>(arg);

                    DoScreenFadeOut(500);
                    while (IsScreenFadingOut())
                        await Delay(0);

                    while (!NetworkDoesEntityExistWithNetworkId(vehicle.ServerVehicleNetworkId))
                        Wait(0);
                    var vehicleEntity = NetworkGetEntityFromNetworkId(vehicle.ServerVehicleNetworkId);
                    while (!DoesEntityExist(vehicleEntity))
                        Wait(0);
                    if (!IsEntityAVehicle(vehicleEntity))
                        return;

                    while (!NetworkDoesEntityExistWithNetworkId(vehicle.ServerDriverNetworkId))
                        Wait(0);
                    var pilotEntity = NetworkGetEntityFromNetworkId(vehicle.ServerDriverNetworkId);
                    while (!DoesEntityExist(pilotEntity))
                        Wait(0);

                    SetDriverAbility(pilotEntity, 1.0f);
                    SetDriverAggressiveness(pilotEntity, 0.5f);

                    SetEntityInvincible(pilotEntity, false);
                    SetEntityInvincible(vehicleEntity, false);
                    SetEntityCompletelyDisableCollision(vehicleEntity, true, true);

                    //TaskEnterVehicle(playerPed.Handle, vehicleEntity, -1, 0, 1.5f, 1, 0); // not working
                    //SetPedIntoVehicle(playerPed.Handle, vehicleEntity, 1);

                    while (!IsPedInVehicle(playerPed.Handle, vehicleEntity, false))
                        Wait(0);

                    DoScreenFadeIn(500);
                    while (IsScreenFadingIn())
                        await Delay(0);

                    //player.LastVehicle.IsCollisionEnabled = false;
                    //player.LastVehicle.IsInvincible = true;
                    //
                    player.CanControlCharacter = false;

                    var speed = 20f;
                    var drivingStyle = 0;
                    var stopRange = 8.0f;

                    TaskVehicleDriveToCoordLongrange(pilotEntity, vehicleEntity, vehicle.DriveToX, vehicle.DriveToY, vehicle.DriveToZ, speed, drivingStyle, stopRange);

                    CurrentPromptServiceVehicle = new PromptServiceVehicle
                    {
                        VehicleId = vehicle.ServerVehicleId,
                        VehicleNetworkId = vehicle.ServerVehicleNetworkId,
                        VehicleEntityId = vehicleEntity,
                        DriverId = vehicle.ServerDriverId,
                        DriverNetworkId = vehicle.ServerDriverNetworkId,
                        DriverEntityId = pilotEntity,
                        X = vehicle.DriveToX,
                        Y = vehicle.DriveToY,
                        Z = vehicle.DriveToZ
                    };
                }));
            }
            await Delay(1000);
        }

        private void OnTickDraw()
        {
            var player = Game.Player;
            var playerCharacter = player.Character;
            var playerCoords = playerCharacter.Position;

            foreach (var prompt in Prompts)
            {
                var drawDistance = prompt.Config.DrawDistance;
                var interactDistance = prompt.Config.InteractDistance;
                var coords = prompt.Config.Coords;

                var distance = GetDistanceBetweenCoords(playerCoords.X, playerCoords.Y, playerCoords.Z, coords.X, coords.Y, coords.Z, true);
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
                    Wait(0);
                }
                else
                {
                    prompt.IsPressed = false;
                    prompt.CanInteract = false;
                }

                if (prompt.IsPressed)
                {
                    if (prompt.Service == PromptService.ServiceCar && CurrentPromptServiceVehicle == null)
                    {
                        ServiceCarQueue.Enqueue(prompt.ValueId);
                    }
                }
            }
        }
    }
}
