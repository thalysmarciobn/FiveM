using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client;
using Client.Core;
using Client.Extensions;
using Client.Helper;
using Mono.CSharp;
using Newtonsoft.Json;
using Shared.Enumerations;
using Shared.Helper;
using Shared.Models.Database;
using static CitizenFX.Core.Native.API;

namespace FiveM.Client
{
    public class ClientMainScript : BaseScript
    {
        private readonly Dictionary<long, int> Blips = new Dictionary<long, int>();

        public ClientMainScript()
        {
            Debug.WriteLine("[PROJECT] Script: CharacterScript");
            EventHandlers[EventName.External.Client.OnClientResourceStart] += new Action<string>(OnClientResourceStart);
            EventHandlers[EventName.External.Client.OnClientResourceStop] += new Action<string>(OnClientResourceStop);
            EventHandlers[EventName.Client.InitAccount] += new Action<string>(InitAccount);
            EventHandlers[EventName.Client.InitCharacter] += new Action<string>(InitCharacter);

            RegisterNuiCallbackType("changeModel");
            EventHandlers["__cfx_nui:changeModel"] += new Action<string, CallbackDelegate>(NUIChangeModel);

            RegisterNuiCallbackType("setCamera");
            EventHandlers["__cfx_nui:setCamera"] += new Action<string, CallbackDelegate>(NUIChangeCamera);

            RegisterNuiCallbackType("setPedHeadBlend");
            EventHandlers["__cfx_nui:setPedHeadBlend"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadBlend);

            RegisterNuiCallbackType("setPedFaceFeatures");
            EventHandlers["__cfx_nui:setPedFaceFeatures"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedFaceFeatures);

            RegisterNuiCallbackType("setPedHeadOverlay");
            EventHandlers["__cfx_nui:setPedHeadOverlay"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadOverlay);

            RegisterNuiCallbackType("setPedHeadOverlayColor");
            EventHandlers["__cfx_nui:setPedHeadOverlayColor"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadOverlayColor);

            RegisterNuiCallbackType("setPedEyeColor");
            EventHandlers["__cfx_nui:setPedEyeColor"] += new Action<int, CallbackDelegate>(NUISetPedEyeColor);
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            SetVehicleDensityMultiplierThisFrame(0.0f);

            SetPedDensityMultiplierThisFrame(0.0f);

            SetRandomVehicleDensityMultiplierThisFrame(0.0f);

            SetParkedVehicleDensityMultiplierThisFrame(0.0f);

            SetScenarioPedDensityMultiplierThisFrame(0.0f, 0.0f);

            DisplayRadar(false);

            SetGarbageTrucks(false);

            SetRandomBoats(false);

            SetCreateRandomCops(false);
            SetCreateRandomCopsNotOnScenarios(false);
            SetCreateRandomCopsOnScenarios(false);

            TriggerServerEvent(EventName.Server.GetBlips, new Action<string>((arg) =>
            {
                var data = JsonConvert.DeserializeObject<ICollection<KeyValuePair<long, BlipModel>>>(arg);

                foreach (var blip in data)
                {
                    var id = blip.Key;
                    var model = blip.Value;

                    var blipId = AddBlipForCoord(model.X, model.Y, model.Z);

                    SetBlipSprite(blipId, model.BlipId);
                    SetBlipDisplay(blipId, model.DisplayId);
                    SetBlipScale(blipId, model.Scale);
                    SetBlipColour(blipId, model.Color);
                    SetBlipAsShortRange(blipId, model.ShortRange);
                    SetBlipPriority(blipId, (int) id);

                    BeginTextCommandSetBlipName("STRING");
                    AddTextComponentString(model.Title);
                    EndTextCommandSetBlipName(blipId);

                    Blips.Add(id, blipId);
                }
            }));

            TriggerServerEvent(EventName.Server.SpawnRequest);
        }

        public void OnClientResourceStop(string resourceName)
        {
            foreach (var blip in Blips)
            {
                var blipId = blip.Value;
                RemoveBlip(ref blipId);
            }
            GameCamera.DeleteCamera();
        }

        public async void NUIChangeModel(string data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var playerPed = Game.PlayerPed;

            while (!await player.ChangeModel(data)) await Delay(10);

            SetPedDefaultComponentVariation(playerPed.Handle);
            player.SetPedHeadBlendDatas(new AccountCharacterPedHeadDataModel());

            cb(new { status = 1 });
        }

        public void NUIChangeCamera(string data, CallbackDelegate cb)
        {
            switch (data)
            {
                case "features":
                    GameCamera.SetCamera(CameraType.Features, 50f);
                    break;
                case "face":
                    GameCamera.SetCamera(CameraType.Face, 50f);
                    break;
                default:
                    GameCamera.SetCamera(CameraType.Entity, 50f);
                    break;
            }

            cb(new { status = 1 });
        }

        public void NUISetPedHeadBlend(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var ped = player.Character.Handle;

            var shapeFirstID = 0;
            var shapeSecondID = 0;
            var shapeThirdID = 0;
            var skinFirstID = 0;
            var skinSecondID = 0;
            var skinThirdID = 0;
            var shapeMixScale = 0f;
            var skinMixScale = 0f;
            var thirdMixScale = 0f;
            var isParent = false;

            if (data.TryGetValue("shapeFirst", out var shapeFirst))
                shapeFirstID = int.TryParse(shapeFirst.ToString(), out var result) ? result : 0;

            if (data.TryGetValue("shapeSecond", out var shapeSecond))
                shapeSecondID = int.TryParse(shapeSecond.ToString(), out var result) ? result : 0;

            if (data.TryGetValue("skinFirst", out var skinFirst))
                skinFirstID = int.TryParse(skinFirst.ToString(), out var result) ? result : 0;

            if (data.TryGetValue("skinSecond", out var skinSecond))
                skinSecondID = int.TryParse(skinSecond.ToString(), out var result) ? result : 0;

            if (data.TryGetValue("shapeMix", out var shapeMix))
                shapeMixScale = float.TryParse(shapeMix.ToString(), out var result) ? result : 0;

            if (data.TryGetValue("skinMix", out var skinMix))
                skinMixScale = float.TryParse(skinMix.ToString(), out var result) ? result : 0;

            SetPedHeadBlendData(ped, shapeFirstID, shapeSecondID, shapeThirdID, skinFirstID, skinSecondID, skinThirdID, shapeMixScale, skinMixScale, thirdMixScale, isParent);

            cb(new { status = 1 });
        }

        public void NUISetPedFaceFeatures(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var ped = player.Character.Handle;

            if (data.TryGetValue("noseWidth", out var noseWidth))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NoseWidth, float.TryParse(noseWidth.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("nosePeakHeight", out var nosePeakHeight))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NosePeakHeight, float.TryParse(nosePeakHeight.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("nosePeakSize", out var nosePeakSize))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NosePeakLength, float.TryParse(nosePeakSize.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("noseBoneHeight", out var noseBoneHeight))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NoseBoneHeight, float.TryParse(noseBoneHeight.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("nosePeakLowering", out var nosePeakLowering))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NosePeakLowering, float.TryParse(nosePeakLowering.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("noseBoneTwist", out var noseBoneTwist))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NoseBoneTwist, float.TryParse(noseBoneTwist.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("eyeBrownHeight", out var eyeBrownHeight))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.EyeBrowHeight, float.TryParse(eyeBrownHeight.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("eyeBrownForward", out var eyeBrownForward))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.EyeBrowLength, float.TryParse(eyeBrownForward.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("cheeksBoneHeight", out var cheeksBoneHeight))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.CheekBoneHeight, float.TryParse(cheeksBoneHeight.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("cheeksBoneWidth", out var cheeksBoneWidth))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.CheekBoneWidth, float.TryParse(cheeksBoneWidth.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("cheeksWidth", out var cheeksWidth))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.CheekWidth, float.TryParse(cheeksWidth.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("eyesOpening", out var eyesOpening))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.EyeOpenings, float.TryParse(eyesOpening.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("lipsThickness", out var lipsThickness))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.LipThickness, float.TryParse(lipsThickness.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("jawBoneWidth", out var jawBoneWidth))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.JawBoneWidth, float.TryParse(jawBoneWidth.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("jawBoneBackSize", out var jawBoneBackSize))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.JawBoneLength, float.TryParse(jawBoneBackSize.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("chinBoneLowering", out var chinBoneLowering))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.ChinBoneLowering, float.TryParse(chinBoneLowering.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("chinBoneLenght", out var chinBoneLenght))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.ChinBoneLength, float.TryParse(chinBoneLenght.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("chinBoneSize", out var chinBoneSize))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.ChinBoneWidth, float.TryParse(chinBoneSize.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("chinHole", out var chinHole))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.ChinDimple, float.TryParse(chinHole.ToString(), out var result) ? result : 0);

            if (data.TryGetValue("neckThickness", out var neckThickness))
                SetPedFaceFeature(ped, (int)FaceShapeEnum.NeckThickness, float.TryParse(neckThickness.ToString(), out var result) ? result : 0);

            cb(new { status = 1 });
        }

        public void NUISetPedHeadOverlay(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var ped = player.Character.Handle;

            foreach (var item in data)
            {
                var overlayID = 0;
                var indexID = 0;
                var opacityScale = 0f;

                var _object = item.Value as IDictionary<string, object>;

                if (_object.TryGetValue("overlay", out var overlay))
                    overlayID = int.TryParse(overlay.ToString(), out var result) ? result : 0;

                if (_object.TryGetValue("index", out var index))
                    indexID = int.TryParse(index.ToString(), out var result) ? result : 0;

                if (_object.TryGetValue("opacity", out var opacity))
                    opacityScale = float.TryParse(opacity.ToString(), out var result) ? result : 0;

                SetPedHeadOverlay(ped, overlayID, indexID, opacityScale);
            }

            cb(new { status = 1 });
        }

        public void NUISetPedHeadOverlayColor(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var ped = player.Character.Handle;

            foreach (var item in data)
            {
                var overlayID = 0;
                var colorTypeID = 0;
                var colorID = 0;
                var secondColorID = 0;

                var _object = item.Value as IDictionary<string, object>;

                if (_object.TryGetValue("overlay", out var overlay))
                    overlayID = int.TryParse(overlay.ToString(), out var result) ? result : 0;

                if (_object.TryGetValue("colorType", out var colorType))
                    colorTypeID = int.TryParse(colorType.ToString(), out var result) ? result : 0;

                if (_object.TryGetValue("colorId", out var colorId))
                    colorID = int.TryParse(colorId.ToString(), out var result) ? result : 0;

                if (_object.TryGetValue("secondColorId", out var secondColorId))
                    secondColorID = int.TryParse(secondColorId.ToString(), out var result) ? result : 0;

                SetPedHeadOverlayColor(ped, overlayID, colorTypeID, colorID, secondColorID);
            }

            cb(new { status = 1 });
        }

        private void NUISetPedEyeColor(int color, CallbackDelegate cb)
        {
            var player = Game.Player;
            var ped = player.Character.Handle;

            SetPedEyeColor(ped, color);

            cb(new { status = 1 });
        }

        private async void InitAccount(string json)
        {
            DoScreenFadeOut(500);
            while (IsScreenFadedOut())
                await Delay(0);

            GlobalVariables.Account = JsonConvert.DeserializeObject<AccountModel>(json);

            var player = Game.Player;
            var playerPed = Game.PlayerPed;
            //player.SetPedHead(new AccountCharacterPedHeadModel
            //{
            //
            //});
            //player.SetPedHeadOverlays(CharacterModelHelper.DefaultList<AccountCharacterPedHeadOverlayModel>());
            //player.SetPedHeadOverlayColors(CharacterModelHelper.DefaultList<AccountCharacterPedHeadOverlayColorModel>());
            //player.SetPedFaceFeatures(CharacterModelHelper.DefaultList<AccountCharacterPedFaceModel>());
            //
            //player.StyleComponents(CharacterModelHelper.DefaultList<AccountCharacterPedComponentModel>());
            //player.StyleProps(CharacterModelHelper.DefaultList<AccountCharacterPedPropModel>());

            //SetEntityCoordsNoOffset(GetPlayerPed(-1), vector3.X, vector3.Y, vector3.Z, false, false, false); ;
            //NetworkResurrectLocalPlayer(vector3.X, vector3.Y, vector3.Z, heading, true, true);

            //StatSetInt((uint)GetHashKey("MP0_WALLET_BALANCE"), 0, true);
            //StatSetInt((uint)GetHashKey("BANK_BALANCE"), 0, true);

            //ClearPedTasksImmediately(GetPlayerPed(-1));

            if (GlobalVariables.Account.Character.Count <= 0)
            {
                var model = new Model("mp_m_freemode_01");

                while (!await Game.Player.ChangeModel(model)) await Delay(10);

                SetPedDefaultComponentVariation(player.Character.Handle);
                SetPedHeadBlendData(player.Character.Handle, 0, 0, 0, 0, 0, 0, 0f, 0f, 0f, false);

                var characterPosition = GlobalVariables.Creation.Position;

                LoadScene(characterPosition.X, characterPosition.Y, characterPosition.Z);
                RequestCollisionAtCoord(characterPosition.X, characterPosition.Y, characterPosition.Z);

                var groundZ = 0f;
                var ground = GetGroundZFor_3dCoord(characterPosition.X, characterPosition.Y, characterPosition.Z, ref groundZ, false);

                player.Character.Position = new Vector3
                {
                    X = characterPosition.X,
                    Y = characterPosition.Y,
                    Z = ground ? groundZ : characterPosition.Z
                };

                player.Character.Rotation = GlobalVariables.Creation.Rotation;

                playerPed.Heading = GlobalVariables.Creation.Heading;

                ClearPedTasksImmediately(playerPed.Handle);

                SetNuiFocus(true, true);

                NuiHelper.SendMessage(new NuiMessage
                {
                    Action = "interface",
                    Key = "creation",
                    Params = new[] { "true" }
                });

                GameCamera.SetCamera(CameraType.Entity, 50f);

                RenderScriptCams(true, false, 0, true, true);

                //TriggerEvent(EventName.Client.InitCreation);
            }

            ShutdownLoadingScreen();

            DoScreenFadeIn(500);
            while (IsScreenFadingIn())
                await Delay(0);
            //player.Character.Armor = 200;
            //player.Character.Health = 200;
            //player.Character.Weapons.Drop();
        }

        private async void InitCharacter(string json)
        {
            var player = Game.Player;
            var playerPed = Game.PlayerPed;

            DoScreenFadeOut(500);
            while (IsScreenFadedOut())
                await Delay(0);

            player.Freeze();

            var resCharacter = JsonConvert.DeserializeObject<AccountCharacterModel>(json);

            var model = new Model(resCharacter.Model);

            while (!await Game.Player.ChangeModel(model)) await Delay(10);

            player.SetPedHeadBlendDatas(resCharacter.PedHeadData);
            //player.SetPedHead(resCharacter.PedHead);
            player.SetPedHeadOverlays(resCharacter.PedHeadOverlay);
            player.SetPedHeadOverlayColors(resCharacter.PedHeadOverlayColor);
            player.SetPedFaceFeatures(resCharacter.PedFace);

            player.StyleComponents(resCharacter.PedComponent);
            player.StyleProps(resCharacter.PedProp);

            var resCharacterPosition = resCharacter.Position;
            var resCharacterRotation = resCharacter.Rotation;

            //SetEntityCoordsNoOffset(GetPlayerPed(-1), vector3.X, vector3.Y, vector3.Z, false, false, false); ;
            //NetworkResurrectLocalPlayer(vector3.X, vector3.Y, vector3.Z, heading, true, true);
            var groundZ = 0f;
            var ground = GetGroundZFor_3dCoord(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z, ref groundZ, false);
            resCharacterPosition.Z = ground ? groundZ : resCharacterPosition.Z;

            player.Character.Position = new Vector3
            {
                X = resCharacterPosition.X,
                Y = resCharacterPosition.Y,
                Z = resCharacterPosition.Z
            };

            player.Character.Rotation = new Vector3
            {
                X = resCharacterRotation.X,
                Y = resCharacterRotation.Y,
                Z = resCharacterRotation.Z
            };

            player.Character.Armor = resCharacter.Armor;
            player.Character.Health = resCharacter.Health;

            // https://vespura.com/fivem/gta-stats/

            var dateCreated = resCharacter.DateCreated;

            var year = dateCreated.Year;
            var month = dateCreated.Month;
            var day = dateCreated.Day;
            var hour = dateCreated.Hour;
            var minute = dateCreated.Minute;
            var second = dateCreated.Second;
            var mili = dateCreated.Millisecond;

            var data = Convert.ToInt32(Game.GameTime);

            Debug.WriteLine($"{data}");

            StatSetDate((uint)GetHashKey("CHAR_DATE_CREATED"), ref data, 7, true);
            Debug.WriteLine($"{data}");


            StatSetInt((uint)GetHashKey("MP0_WALLET_BALANCE"), resCharacter.MoneyBalance, true);
            StatSetInt((uint)GetHashKey("BANK_BALANCE"), resCharacter.BankBalance, true);

            playerPed.Heading = resCharacter.Heading;

            LoadScene(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z);
            RequestCollisionAtCoord(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z);

            //ClearPedTasksImmediately(GetPlayerPed(-1));

            //RemoveAllPedWeapons(GetPlayerPed(-1), false);
            player.Character.Weapons.Drop();

            //ClearPlayerWantedLevel(PlayerId());
            player.WantedLevel = 0;

            while (!HasCollisionLoadedAroundEntity(Game.PlayerPed.Handle))
                await Delay(0);

            ShutdownLoadingScreen();

            DoScreenFadeIn(500);
            while (IsScreenFadingIn())
                await Delay(0);

            player.Unfreeze();

            if (GlobalVariables.S_Debug)
                Debug.WriteLine($"Spawn: {resCharacterPosition.X} {resCharacterPosition.Y} {resCharacterPosition.Z}");

            //SwitchInPlayer(PlayerPedId());

            //player.Spawn(vector3);

            // API.RequestClipSet(character.WalkingStyle);
            // await BaseScript.Delay(100);
            // Game.Player.Character.MovementAnimationSet = this.WalkingStyle;

        }
    }
}