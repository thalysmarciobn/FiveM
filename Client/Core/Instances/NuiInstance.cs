using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Client.Extensions;
using Client.Helper;
using FiveM.Client;
using Shared.Enumerations;
using static CitizenFX.Core.Native.API;
using static Client.GameCamera;
using static Client.GlobalVariables;

namespace Client.Core.Instances
{
    public class NuiInstance : AbstractInstance<NuiInstance>
    {
        public async void NUIChangeModel(string data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var playerPed = Game.PlayerPed;

            while (!await player.ChangeModel(data)) await BaseScript.Delay(10);

            SetPedDefaultComponentVariation(playerPed.Handle);
            SetPedHeadBlendData(playerPed.Handle, 0, 0, 0, 0, 0, 0, 0f, 0f, 0f, false);

            cb(new { status = 1 });
        }

        public void NUIChangeCamera(string data, CallbackDelegate cb)
        {
            switch (data)
            {
                case "hair":
                    SetCamera(CameraType.Hair, 50f);
                    break;
                case "face":
                    SetCamera(CameraType.Face, 50f);
                    break;
                case "shoes":
                    SetCamera(CameraType.Shoes, 50f);
                    break;
                default:
                    SetCamera(CameraType.Entity, 50f);
                    break;
            }

            cb(new { status = 1 });
        }

        public void NUISetPedHeadBlend(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var ped = player.Character.Handle;

            SetPedHeadBlendData(ped, data.GetInt("shapeFirst"), data.GetInt("shapeSecond"), 0, data.GetInt("skinFirst"),
                data.GetInt("skinSecond"), 0, data.GetFloat("shapeMix"), data.GetFloat("skinMix"), 0f, false);

            cb(new { status = 1 });
        }

        public void NUISetPedFaceFeatures(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            var featureMap = new Dictionary<string, FaceShapeEnum>
            {
                { "noseWidth", FaceShapeEnum.NoseWidth },
                { "nosePeakHeight", FaceShapeEnum.NosePeakHeight },
                { "nosePeakSize", FaceShapeEnum.NosePeakLength },
                { "noseBoneHeight", FaceShapeEnum.NoseBoneHeight },
                { "nosePeakLowering", FaceShapeEnum.NosePeakLowering },
                { "noseBoneTwist", FaceShapeEnum.NoseBoneTwist },
                { "eyeBrownHeight", FaceShapeEnum.EyeBrowHeight },
                { "eyeBrownForward", FaceShapeEnum.EyeBrowLength },
                { "cheeksBoneHeight", FaceShapeEnum.CheekBoneHeight },
                { "cheeksBoneWidth", FaceShapeEnum.CheekBoneWidth },
                { "cheeksWidth", FaceShapeEnum.CheekWidth },
                { "eyesOpening", FaceShapeEnum.EyeOpenings },
                { "lipsThickness", FaceShapeEnum.LipThickness },
                { "jawBoneWidth", FaceShapeEnum.JawBoneWidth },
                { "jawBoneBackSize", FaceShapeEnum.JawBoneLength },
                { "chinBoneLowering", FaceShapeEnum.ChinBoneLowering },
                { "chinBoneLenght", FaceShapeEnum.ChinBoneLength },
                { "chinBoneSize", FaceShapeEnum.ChinBoneWidth },
                { "chinHole", FaceShapeEnum.ChinDimple },
                { "neckThickness", FaceShapeEnum.NeckThickness }
            };

            foreach (var feature in featureMap)
                if (data.TryGetValue(feature.Key, out var value))
                    SetPedFaceFeature(ped, (int)feature.Value,
                        float.TryParse(value.ToString(), out var result) ? result : 0);

            cb(new { status = 1 });
        }

        public void NUISetPedProps(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            var featureMap = new Dictionary<string, PropVariationEnum>
            {
                { "hat", PropVariationEnum.Hats },
                { "glass", PropVariationEnum.Glasses },
                { "ear", PropVariationEnum.EarPieces },
                { "watch", PropVariationEnum.Watches },
                { "bracelet", PropVariationEnum.Wristbands }
            };

            foreach (var feature in featureMap)
                if (data.TryGetValue(feature.Key, out var value))
                {
                    var _object = value as IDictionary<string, object>;
                    SetPedPropIndex(ped, _object.GetInt("componentId"), _object.GetInt("drawableId"),
                        _object.GetInt("textureId"), _object.GetBool("attach"));
                }

            cb(new { status = 1 });
        }

        public void NUISetPedHeadOverlay(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            foreach (var item in data)
            {
                var _object = item.Value as IDictionary<string, object>;

                SetPedHeadOverlay(ped, _object.GetInt("overlay"), _object.GetInt("index"), _object.GetFloat("opacity"));
            }

            cb(new { status = 1 });
        }

        public void NUISetPedHeadOverlayColor(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            foreach (var item in data)
            {
                var _object = item.Value as IDictionary<string, object>;

                SetPedHeadOverlayColor(ped, _object.GetInt("overlay"), _object.GetInt("colorType"),
                    _object.GetInt("colorId"), _object.GetInt("secondColorId"));
            }

            cb(new { status = 1 });
        }

        public void NUISetPedComponentVariation(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            var featureMap = new Dictionary<string, ComponentVariationEnum>
            {
                { "face", ComponentVariationEnum.Head },
                { "mask", ComponentVariationEnum.Masks },
                { "hair", ComponentVariationEnum.HairStyles },
                { "torso", ComponentVariationEnum.Torsos },
                { "legs", ComponentVariationEnum.Legs },
                { "bag", ComponentVariationEnum.BagsNParachutes },
                { "shoes", ComponentVariationEnum.Shoes },
                { "accessory", ComponentVariationEnum.Accessories },
                { "undershirt", ComponentVariationEnum.Undershirts },
                { "kevlar", ComponentVariationEnum.BodyArmors },
                { "badge", ComponentVariationEnum.Decals },
                { "torso2", ComponentVariationEnum.Tops }
            };

            foreach (var feature in featureMap)
                if (data.TryGetValue(feature.Key, out var value))
                {
                    var _object = value as IDictionary<string, object>;
                    SetPedComponentVariation(ped, _object.GetInt("componentId"), _object.GetInt("drawableId"),
                        _object.GetInt("textureId"), _object.GetInt("palleteId"));
                }
        }

        public void NUISetPedEyeColor(int color, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            SetPedEyeColor(ped, color);

            cb(new { status = 1 });
        }

        public void NUISetPedHairColor(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            SetPedHairColor(ped, data.GetInt("color"), data.GetInt("highlight"));

            cb(new { status = 1 });
        }

        public void NUIRegisterCharacter(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var name = data.GetString("name");
            var lastName = data.GetString("lastName");
            var age = data.GetInt("age");
            var slot = data.GetInt("slot");
            var appearance = data.GetObject("appearance");

            BaseScript.TriggerServerEvent(EventName.Server.RegisterCharacter, name, lastName, age, slot, appearance,
                new Action<int>(serverStatus =>
                {
                    var player = Game.Player;

                    if (serverStatus == (int)RegisterCharacterEnum.Success)
                    {
                        player.Character.IsCollisionEnabled = true;
                        player.Character.IsPositionFrozen = false;
                        player.Character.IsInvincible = false;

                        player.CanControlCharacter = true;
                    }

                    cb(new { status = serverStatus });
                }));
        }

        public void UpdateTime()
        {
            if (!G_World.HasTime)
                return;

            NuiHelper.SendMessage("interface", "world", new object[]
            {
                G_World.Weather,
                G_World.RainLevel,
                G_World.WindSpeed,
                G_World.WindDirection,
                G_World.CurrentTime.Hours,
                G_World.CurrentTime.Minutes,
                G_World.CurrentTime.Seconds
            });
        }
    }
}
