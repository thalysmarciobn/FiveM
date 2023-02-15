using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Mono.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Core
{
    public class Prompt
    {
        public float KeyTextWidth { get; private set; }
        public float LabelTextWidth { get; private set; }
        public float TextHeight { get; private set; }
        public PromptConfig Config { get; set; }
        public Vector2 BoxPadding { get; set; }
        public float MinWidth { get; private set; }
        public float MaxWidth{ get; private set; }
        public bool CanInteract { get; set; }

        public PromptButton Button = new PromptButton();
        public PromptBackground Background = new PromptBackground();
        public PromptFX FX = new PromptFX();

        public void Update()
        {
            int sw = 0;
            int sh = 0;
            // Dimensions
            GetActiveScreenResolution(ref sw, ref sh);

            KeyTextWidth = GetTextWidth(Config.KeyLabel);
            LabelTextWidth = GetTextWidth(Config.TextLabel);

            TextHeight = GetRenderedCharacterHeight(Config.Scale, Config.Font);
            
            if (Config.Font == 0)
            {
                Config.TextOffset = 0.0065f;
            }
            
            //

            // Buttons
            Button.W = (Math.Max(Config.ButtonSize, KeyTextWidth) * sw) / sw;
            Button.H = (Config.ButtonSize * sw) / sh;
            Button.BC = Config.ButtonColor;
            Button.FC = Config.KeyColor;

            FX.W = Button.W;
            FX.H = Button.H;
            FX.A = 255;
            //

            BoxPadding = new Vector2
            {
                X = (Config.Padding * sw) / sw,
                Y = (Config.Padding * sw) / sh
            };

            // Background
            MinWidth = Button.W + (BoxPadding.X * 2);
            MaxWidth = LabelTextWidth + Button.W + (BoxPadding.X * 3) + (Config.Margin * 2);

            Background.W = MaxWidth;
            Background.H = Button.H + (BoxPadding.Y * 2);
            Background.BC = Config.BackgroundColor;
            Background.FC = Config.LabelColor;

            Button.X = Config.Origin.X - (Background.W / 2) + (Button.W / 2) + BoxPadding.X;
            Button.Y = Config.Origin.Y - (Background.H / 2) + (Button.H / 2) + BoxPadding.Y;

            Button.TextX = Button.X;
            Button.TextY = Button.Y - TextHeight + Config.TextOffset;

            Background.TextX = Button.X + (Button.W / 2) + Config.Margin + BoxPadding.X;
            Background.TextY = Button.Y - TextHeight + Config.TextOffset;


            //
        }

        public void Draw()
        {
            var background = Background;
            var button = Button;

            if (CanInteract)
            {
                if (background.W < MaxWidth)
                    background.W = background.W + 0.008f;
                else
                    background.W = MaxWidth;
                background.FC.A = 255;
            }
            else
            {
                if (background.W > MinWidth)
                    background.W = background.W - 0.008f;
                else
                    background.W = MinWidth;
                background.FC.A = 0;
            }

            Button.TextX = Button.X;
            Button.X = Config.Origin.X - (Background.W / 2) + (Button.W / 2) + BoxPadding.X;

            RenderElement(Config.TextLabel, background, false);
            RenderElement(Config.KeyLabel, button);
        }

        private void RenderElement(string text, IBox box, bool centered = true)
        {
            SetTextScale(Config.Scale, Config.Scale);
            SetTextFont(Config.Font);
            SetTextColour(box.FC.R, box.FC.G, box.FC.B, box.FC.A);
            SetTextEntry("STRING");
            SetTextCentre(centered);
            AddTextComponentString(text);
            SetDrawOrigin(Config.Coords.X, Config.Coords.Y, Config.Coords.Z, 0);
            EndTextCommandDisplayText(box.TextX, box.TextY);
            DrawRect(box.X, box.Y, box.W, box.H, box.BC.R, box.BC.G, box.BC.B, box.BC.A);
            ClearDrawOrigin();
        }

        public float GetTextWidth(string text)
        {
            BeginTextCommandGetWidth("STRING");
            SetTextScale(Config.Scale, Config.Scale);
            SetTextFont(Config.Font);
            SetTextEntry("STRING");
            AddTextComponentString(text);
            return EndTextCommandGetWidth(true);
        }
    }
    public class PromptConfig
    {
        public string TextLabel { get; set; }
        public string KeyLabel { get; set; }
        public int Font { get; set; }
        public float Scale { get; set; }
        public Vector3 Coords { get; set; }
        public Vector2 Origin { get; set; }
        public Vector3 Offset { get; set; }
        public float Margin { get; set; }
        public float Padding { get; set; }
        public float TextOffset { get; set; }
        public float ButtonSize { get; set; }
        public RGBAColor BackgroundColor { get; set; }
        public RGBAColor LabelColor { get; set; }
        public RGBAColor ButtonColor { get; set; }
        public RGBAColor KeyColor { get; set; }
        public float DrawDistance { get; set; }
        public float InteractDistance { get; set; }
    }
    public interface IBox
    {
        float X { get; set; }
        float Y { get; set; }
        float W { get; set; }
        float H { get; set; }
        float TextX { get; set; }
        float TextY { get; set; }
        RGBAColor BC { get; set; }
        RGBAColor FC { get; set; }
    }

    public class PromptButton : IBox
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float W { get; set; }
        public float H { get; set; }
        public float TextX { get; set; }
        public float TextY { get; set; }
        public RGBAColor BC { get; set; }
        public RGBAColor FC { get; set; }
    }

    public class PromptBackground : IBox
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float W { get; set; }
        public float H { get; set; }
        public float TextX { get; set; }
        public float TextY { get; set; }
        public RGBAColor BC { get; set; }
        public RGBAColor FC { get; set; }
    }
    public class PromptFX
    {
        public float W { get; set; }
        public float H { get; set; }
        public int A { get; set; }
    }
}
