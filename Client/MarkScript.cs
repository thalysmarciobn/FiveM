﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core;
using Client.Extensions;
using Mono.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Models.Database;
using Shared.Models.Server;
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
        private Queue<long> ServicesToAction { get; } = new Queue<long>();
        private Dictionary<long, PromptServiceData> ServicesInAction { get; } = new Dictionary<long, PromptServiceData>();

        public MarkScript()
        {
            Tick += OnTick;
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

                if (DoesEntityExist(vehicleId))
                    DeleteVehicle(ref vehicleId);
            }
        }

        private async Task OnTick()
        {
            var player = Game.Player;
            var playerPed = Game.PlayerPed;
            var playerCharacter = player.Character;
            var playerCoords = playerCharacter.Position;

            if (CurrentPromptServiceVehicle != null)
            {
                var vehicleId = CurrentPromptServiceVehicle.VehicleEntityId;

                var distance = GetDistanceBetweenCoords(playerCoords.X, playerCoords.Y, playerCoords.Z, CurrentPromptServiceVehicle.X, CurrentPromptServiceVehicle.Y, CurrentPromptServiceVehicle.Z, true);

                if (distance < 20.0f)
                    player.CanControlCharacter = true;

                if (DoesEntityExist(vehicleId))
                {
                    if (!IsPedInVehicle(playerPed.Handle, vehicleId, false))
                    {
                        DeleteVehicle(ref vehicleId);
                        while (DoesEntityExist(vehicleId))
                            Wait(0);
                        CurrentPromptServiceVehicle = null;
                    }
                }
                else
                {
                    player.CanControlCharacter = true;
                    CurrentPromptServiceVehicle = null;
                }
            }

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
                            Service = prompt.Service
                        });
                        ServicesToAction.Enqueue(prompt.ValueId);
                    }
                }
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

                        var driver = CreatePedInsideVehicle(vehicleEntity, 1, (uint) PedHash.FreemodeMale01, -1, true, true);

                        while (!DoesEntityExist(driver))
                            Wait(0);

                        SetDriverAbility(driver, 1.0f);
                        SetDriverAggressiveness(driver, 0.5f);

                        SetPedRandomProps(driver);
                        SetPedRandomComponentVariation(driver, true);

                        SetEntityInvincible(driver, false);
                        SetEntityInvincible(vehicleEntity, false);
                        SetEntityCompletelyDisableCollision(vehicleEntity, true, true);

                        SetPedIntoVehicle(playerPed.Handle, vehicleEntity, 1);

                        player.CanControlCharacter = false;

                        while (!IsPedInVehicle(playerPed.Handle, vehicleEntity, false))
                            Wait(0);

                        DoScreenFadeIn(500);
                        while (IsScreenFadingIn())
                            await Delay(0);

                        var speed = 30f;
                        // https://vespura.com/fivem/drivingstyle/
                        var drivingStyle = 191;
                        var stopRange = 20.0f;

                        TaskVehicleDriveToCoordLongrange(driver, vehicleEntity, vehicle.Model.DriveToX, vehicle.Model.DriveToY, vehicle.Model.DriveToZ, speed, drivingStyle, stopRange);

                        CurrentPromptServiceVehicle = new PromptServiceVehicle
                        {
                            VehicleNetworkId = vehicle.NetworkId,
                            VehicleEntityId = vehicleEntity,
                            X = vehicle.Model.DriveToX,
                            Y = vehicle.Model.DriveToY,
                            Z = vehicle.Model.DriveToZ
                        };
                    }));
            }
            Wait(0);
        }
    }
}
