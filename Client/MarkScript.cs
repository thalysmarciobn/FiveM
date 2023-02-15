using CitizenFX.Core;
using Client.Core;
using Mono.CSharp;
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

                Debug.WriteLine($"{mathX}   {mathY}");
                if (mathX < drawDistance && mathY < drawDistance)
                {
                    if (mathX < interactDistance && mathY < interactDistance)
                    {
                        if (IsControlJustPressed(0, (int)prompt.Config.Key))
                            prompt.IsPressed = true;
                        prompt.CanInteract = true;
                    }
                    else
                        prompt.CanInteract = false;
                    prompt.Draw();

                    Wait(1000);
                }
                else
                {
                    prompt.CanInteract = false;
                }

                Wait(0);
            }
        }
    }
}
