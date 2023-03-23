using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client;
using Client.Extensions;
using Client.Helper;
using Shared.Enumerations;
using Shared.Helper;
using Shared.Models.Database;
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;
using static CitizenFX.Core.UI.Screen;
using static Client.GameCamera;
using static Client.GlobalVariables;

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
            EventHandlers[EventName.External.BaseEvents.OnBaseResourceStart] += new Action(OnBaseResourceStart);
            //EventHandlers["onClientResourceStart"] += new Action(OnBaseResourceStart);
            EventHandlers[EventName.Client.InitAccount] += new Action<string>(InitAccount);

            RegisterNuiCallbackType("characterRequest");
            EventHandlers["__cfx_nui:characterRequest"] += new Action<int>(CharacterRequest);

            RegisterNuiCallbackType("changeModel");
            EventHandlers["__cfx_nui:changeModel"] += new Action<string, CallbackDelegate>(NUIChangeModel);

            RegisterNuiCallbackType("setCamera");
            EventHandlers["__cfx_nui:setCamera"] += new Action<string, CallbackDelegate>(NUIChangeCamera);

            RegisterNuiCallbackType("setPedHeadBlend");
            EventHandlers["__cfx_nui:setPedHeadBlend"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadBlend);

            RegisterNuiCallbackType("setPedFaceFeatures");
            EventHandlers["__cfx_nui:setPedFaceFeatures"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedFaceFeatures);

            RegisterNuiCallbackType("setPedProps");
            EventHandlers["__cfx_nui:setPedProps"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedProps);

            RegisterNuiCallbackType("setPedHeadOverlay");
            EventHandlers["__cfx_nui:setPedHeadOverlay"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadOverlay);

            RegisterNuiCallbackType("setPedHeadOverlayColor");
            EventHandlers["__cfx_nui:setPedHeadOverlayColor"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadOverlayColor);

            RegisterNuiCallbackType("setPedComponentVariation");
            EventHandlers["__cfx_nui:setPedComponentVariation"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedComponentVariation);

            RegisterNuiCallbackType("setPedEyeColor");
            EventHandlers["__cfx_nui:setPedEyeColor"] += new Action<int, CallbackDelegate>(NUISetPedEyeColor);

            RegisterNuiCallbackType("setPedHairColor");
            EventHandlers["__cfx_nui:setPedHairColor"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHairColor);

            RegisterNuiCallbackType("registerCharacter");
            EventHandlers["__cfx_nui:registerCharacter"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>(NUIRegisterCharacter);
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            // Weather
            ClearOverrideWeather();
            ClearWeatherTypePersist();

            TriggerServerEvent(EventName.Server.GetTimeSync, new Action<string>(arg =>
            {
                var data = JsonHelper.DeserializeObject<ServerTimeSync>(arg);

                if (G_World.Weather != data.Weather)
                    G_World.Weather = data.Weather;

                if (G_World.RainLevel != data.RainLevel)
                    G_World.RainLevel = data.RainLevel;

                if (G_World.WindSpeed != data.WindSpeed)
                    G_World.WindSpeed = data.WindSpeed;

                if (G_World.WindDirection != data.WindDirection)
                    G_World.WindDirection = data.WindDirection;

                G_World.LastRealTime = DateTime.UtcNow;
                G_World.LastServerTime = new DateTime(data.Ticks);
                G_World.HasTime = true;

                G_World.Update = true;
            }));
            TriggerServerEvent(EventName.Server.GetBlips, new Action<string>(arg =>
            {
                var data = JsonHelper.DeserializeObject<ICollection<KeyValuePair<long, BlipModel>>>(arg);

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

                    BeginTextCommandSetBlipName("STRING");
                    AddTextComponentString(model.Title);
                    EndTextCommandSetBlipName(blipId);

                    Blips.Add(id, blipId);
                }
            }));
        }

        public void OnClientResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            foreach (var blip in Blips)
            {
                var blipId = blip.Value;
                RemoveBlip(ref blipId);
            }

            GameCamera.DeleteCamera();
        }

        public void OnBaseResourceStart()
        {
            TriggerServerEvent(EventName.Server.AccountRequest);
        }

        public async void NUIChangeModel(string data, CallbackDelegate cb)
        {
            var player = Game.Player;
            var playerPed = Game.PlayerPed;

            while (!await player.ChangeModel(data)) await Delay(10);

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

        private void NUISetPedEyeColor(int color, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            SetPedEyeColor(ped, color);

            cb(new { status = 1 });
        }

        private void NUISetPedHairColor(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var ped = Game.Player.Character.Handle;

            SetPedHairColor(ped, data.GetInt("color"), data.GetInt("highlight"));

            cb(new { status = 1 });
        }

        private void NUIRegisterCharacter(IDictionary<string, object> data, CallbackDelegate cb)
        {
            var name = data.GetString("name");
            var lastName = data.GetString("lastName");
            var age = data.GetInt("age");
            var slot = data.GetInt("slot");
            var appearance = data.GetObject("appearance");

            TriggerServerEvent(EventName.Server.RegisterCharacter, name, lastName, age, slot, appearance,
                new Action<int>(serverStatus =>
                {
                    var player = Game.Player;

                    if (serverStatus == (int)RegisterCharacterEnum.Success)
                    {
                        player.Character.IsCollisionEnabled = true;
                        player.Character.IsPositionFrozen = false;
                        player.Character.IsInvincible = false;
                    }

                    cb(new { status = serverStatus });
                }));
        }

        private async void InitAccount(string json)
        {
            DoScreenFadeOut(500);
            while (IsScreenFadedOut())
                await Delay(10);

            var account = JsonHelper.DeserializeObject<AccountModel>(json);

            var player = Game.Player;

            if (account.Character.Count <= 0)
            {
                var characterPosition = Creation.Position;
                var heading = Creation.Heading;

                while (!await Game.Player.ChangeModel(PedHash.FreemodeMale01)) await Delay(10);

                LoadScene(characterPosition.X, characterPosition.Y, characterPosition.Z);
                SetPedDefaultComponentVariation(PlayerPedId());
                SetPedHeadBlendData(PlayerPedId(), 0, 0, 0, 0, 0, 0, 0f, 0f, 0f, false);
                RequestCollisionAtCoord(characterPosition.X, characterPosition.Y, characterPosition.Z);

                SetEntityCoordsNoOffset(PlayerPedId(), characterPosition.X, characterPosition.Y, characterPosition.Z,
                    false, false, false);
                NetworkResurrectLocalPlayer(characterPosition.X, characterPosition.Y, characterPosition.Z, heading,
                    true, true);
                ClearPedTasksImmediately(PlayerPedId());

                while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await Delay(10);

                var groundZ = 0f;
                var ground = GetGroundZFor_3dCoord(characterPosition.X, characterPosition.Y, characterPosition.Z,
                    ref groundZ, false);

                player.Character.Position = new Vector3
                {
                    X = characterPosition.X,
                    Y = characterPosition.Y,
                    Z = ground ? groundZ : characterPosition.Z
                };

                player.CanControlCharacter = false;

                player.Character.IsCollisionEnabled = false;
                player.Character.IsPositionFrozen = true;
                player.Character.IsInvincible = true;

                player.Character.Heading = heading;
                player.Character.Rotation = Creation.Rotation;

                SetNuiFocus(true, true);

                NuiHelper.SendMessage("interface", "creation", new[] { "true", "0" });

                SetCamera(CameraType.Entity, 50f);

                RenderScriptCams(true, false, 0, true, true);
            }
            else
            {
                var resCharacter = account.Character.SingleOrDefault(x => x.Id == account.CurrentCharacter);
                await EnterCharacter(resCharacter);
            }

            SetCanAttackFriendly(PlayerPedId(), S_FriendlyFire, false);
            NetworkSetFriendlyFireOption(S_FriendlyFire);

            ClearPlayerWantedLevel(PlayerId());
            SetMaxWantedLevel(0);

            ShutdownLoadingScreen();

            DoScreenFadeIn(500);
            while (IsScreenFadingIn()) await Delay(1);
        }

        private void CharacterRequest(int slot)
        {
            TriggerServerEvent(EventName.Server.CharacterRequest, slot, new Action<string>(async json =>
            {
                var data = JsonHelper.DeserializeObject<AccountCharacterModel>(json);

                await EnterCharacter(data);
            }));
        }

        private async Task EnterCharacter(AccountCharacterModel resCharacter)
        {
            if (G_Character.Entered)
                return;

            var player = Game.Player;

            var resCharacterPosition = resCharacter.Position;
            var resCharacterRotation = resCharacter.Rotation;

            LoadScene(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z);
            RequestCollisionAtCoord(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z);

            while (!await Game.Player.ChangeModel(new Model(resCharacter.Model))) await Delay(10);

            player.SetEyeColor(resCharacter);
            player.SetHairColor(resCharacter.PedHead);
            player.SetPedHeadBlendDatas(resCharacter.PedHeadData);
            player.SetPedFaceFeatures(resCharacter.PedFace);

            // components e props
            player.SetComponentVariation(resCharacter.PedComponent);
            player.SetPropIndex(resCharacter.PedProp);

            player.SetPedHeadOverlays(resCharacter.PedHeadOverlay);
            player.SetPedHeadOverlayColors(resCharacter.PedHeadOverlayColor);

            while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await Delay(10);

            //SetEntityCoordsNoOffset(GetPlayerPed(-1), vector3.X, vector3.Y, vector3.Z, false, false, false); ;
            //NetworkResurrectLocalPlayer(vector3.X, vector3.Y, vector3.Z, heading, true, true);
            var groundZ = 0f;
            var ground = GetGroundZFor_3dCoord(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z,
                ref groundZ, false);

            player.Character.Position = new Vector3
            {
                X = resCharacterPosition.X,
                Y = resCharacterPosition.Y,
                Z = ground ? groundZ : resCharacterPosition.Z
            };

            player.Character.Rotation = new Vector3
            {
                X = resCharacterRotation.X,
                Y = resCharacterRotation.Y,
                Z = resCharacterRotation.Z
            };
            player.Character.Heading = resCharacter.Heading;

            player.Character.Armor = resCharacter.Armor;
            player.Character.Health = resCharacter.Health;
            player.Character.MaxHealth = G_Character.MaxHealth;

            //ClearPlayerWantedLevel(PlayerId());
            player.WantedLevel = 0;

            player.CanControlCharacter = true;

            player.Character.IsCollisionEnabled = true;
            player.Character.IsPositionFrozen = false;
            player.Character.IsInvincible = false;

            G_Character.Entered = true;

            SetNuiFocus(false, false);

            NuiHelper.SendMessage("interface", "creation", new[] { "false", "0" });

            DeleteCamera();

            RenderScriptCams(false, false, 0, true, true);
        }

        [Command("test")]
        public void Test()
        {
            var ped = Game.PlayerPed;
            ped.Resurrect();
            var position = ped.Position;
            NetworkResurrectLocalPlayer(position.X, position.Y, position.Z, ped.Heading, true, false);
            ped.IsInvincible = false;
            ped.ClearBloodDamage();
            ped.Health = G_Character.MaxHealth;
            //ped.Resurrect();
        }

        [Tick]
        public async Task OnTick()
        {
            if (G_World.Update)
            {
                SetRainFxIntensity(G_World.RainLevel);
                SetWindSpeed(G_World.WindSpeed);
                SetWindDirection(G_World.WindDirection);

                if (G_World.Weather == (uint)Weather.Christmas)
                {
                    SetForceVehicleTrails(true);
                    SetForcePedFootstepsTracks(true);
                }

                if (G_World.Weather != (uint)Weather.Christmas &&
                    G_World.LastWeather != (uint)Weather.Christmas)
                {
                    SetForceVehicleTrails(false);
                    SetForcePedFootstepsTracks(false);
                }

                World.TransitionToWeather((Weather)G_World.Weather, 45f);
            }

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
            await Delay(1000);
        }

        [Tick]
        public async Task OnFrame()
        {
            var character = Game.Player.Character;
            var isInVehicle = character.IsInVehicle();

            if (isInVehicle)
            {
                var vehicle = character.CurrentVehicle;
                if (vehicle != null)
                {
                    var seatDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver);
                    G_Character.DisplayRadar = seatDriver == character;
                    DisplayRadar(G_Character.DisplayRadar);
                }
            }
            else if (G_Character.DisplayRadar)
            {
                G_Character.DisplayRadar = false;
                DisplayRadar(false);
            }

            await Delay(100);
        }

        [Tick]
        public async Task OnTickOverrideClockTime()
        {
            if (!G_World.HasTime)
                return;

            NetworkOverrideClockTime(G_World.CurrentTime.Hours, G_World.CurrentTime.Minutes,
                G_World.CurrentTime.Seconds);

            await Delay(10);
        }

        [Command("forcevehicle")]
        public void ForceVehicle(int src, List<object> args, string raw)
        {
            var model = new Model(args[0].ToString());

            var id = (uint)model.Hash;

            TriggerServerEvent(EventName.Server.ForceVehicle, id, new Action<string>(arg => { Debug.WriteLine(arg); }));
        }

        [Command("fps")]
        public void Fps(int src, List<object> args, string raw)
        {
            var active = args[0].ToString();
            switch (active)
            {
                case "on":
                    SetTimecycleModifier("cinema");
                    break;
                case "off":
                default:
                    SetTimecycleModifier("default");
                    break;
            }

            NuiHelper.SendMessage("interface", "notification", new object[]
            {
                "info",
                active == "on" ? "Ciclo mudado para cinema." : "Ciclo definido como pardão."
            });
        }
    }
}