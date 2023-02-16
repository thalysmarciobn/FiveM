using CitizenFX.Core;
using Client.Core;
using Mono.CSharp;
using Newtonsoft.Json;
using Shared.Models.Database;
using Shared.Models.Game;
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
        public IList<Prompt> Prompts { get; set; } = new List<Prompt>();
        public MarkScript()
        {
            Debug.WriteLine("[PROJECT] Script: MarkScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
            Tick += OnTick;
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            TriggerServerEvent(EventName.Server.GetServiceVehicles, new Action<string>((arg) =>
            {
                var vehicles = JsonConvert.DeserializeObject<ICollection<ServerVehicleService>>(arg);
                foreach (var vehicle in vehicles)
                {
                    var prompt = new Prompt(vehicle.Id, new PromptConfig
                    {
                        Key = (Control) vehicle.Key,
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
                    });
                    prompt.Update();
                    Prompts.Add(prompt);
                }
            }));
        }

        public async Task OnTick()
        {
            var playerCoords = Game.Player.Character.Position;

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
                            prompt.IsPressed = true;
                        prompt.CanInteract = true;
                    }
                    else
                        prompt.CanInteract = false;

                    prompt.Draw();
                }
                else
                {
                    prompt.CanInteract = false;
                }

                if (prompt.IsPressed)
                    TriggerServerEvent(EventName.Server.SpawnVehicleService, prompt.Id, new Action<string>((arg) =>
                    {
                        var vehicle = JsonConvert.DeserializeObject<VehicleService>(arg);

                        prompt.IsPressed = false;
                    }));

                Wait(1000);
            }
        }
    }
}
