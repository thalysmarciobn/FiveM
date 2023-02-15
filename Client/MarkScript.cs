using CitizenFX.Core;
using Client.Core;
using Mono.CSharp;
using Newtonsoft.Json;
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
            Prompts.Add(new Prompt(new PromptConfig
            {
                Key = Control.Pickup,
                KeyLabel = "E",
                TextLabel = "Chamar Taxi",
                Font = 0,
                Scale = 0.4f,
                Coords = new Vector3(-1036.2177f, -2732.6296f, 13.7566f),
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
            foreach (var prompt in Prompts)
            {
                prompt.Update();
            }
            Tick += OnTick;
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
        }

        public async Task OnTick()
        {
            var pcoords = Game.Player.Character.Position;

            foreach (var prompt in Prompts)
            {
                var drawDistance = prompt.Config.DrawDistance;
                var interactDistance = prompt.Config.InteractDistance;
                var coords = prompt.Config.Coords;

                var mathX = coords.X - pcoords.X;
                var mathY = coords.Y - pcoords.Y;

                if (mathX < drawDistance && mathY < drawDistance)
                {
                    if (mathX < interactDistance && mathY < interactDistance)
                    {
                        prompt.IsPressed = IsControlJustPressed(0, (int)prompt.Config.Key);
                        prompt.CanInteract = true;
                    }
                    else
                        prompt.CanInteract = false;
                    prompt.Draw();

                    Wait(1000);
                }
                else
                {
                    prompt.IsPressed = false;
                    prompt.CanInteract = false;
                }

                if (prompt.IsPressed)
                    TriggerServerEvent(EventName.Server.SpawnVehicleService, 1, new Action<string>((arg) =>
                    {
                        var vehicle = JsonConvert.DeserializeObject<VehicleService>(arg);
                        Debug.WriteLine($"aaaaaa: {vehicle.Id}  {vehicle.Name}");
                    }));

                Wait(0);
            }
        }
    }
}
