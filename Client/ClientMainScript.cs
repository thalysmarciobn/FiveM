using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;
using static CitizenFX.Core.UI.Screen;
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
            EventHandlers["__cfx_nui:setPedHeadBlend"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadBlend);

            RegisterNuiCallbackType("setPedFaceFeatures");
            EventHandlers["__cfx_nui:setPedFaceFeatures"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedFaceFeatures);

            RegisterNuiCallbackType("setPedProps");
            EventHandlers["__cfx_nui:setPedProps"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedProps);

            RegisterNuiCallbackType("setPedHeadOverlay");
            EventHandlers["__cfx_nui:setPedHeadOverlay"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadOverlay);

            RegisterNuiCallbackType("setPedHeadOverlayColor");
            EventHandlers["__cfx_nui:setPedHeadOverlayColor"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHeadOverlayColor);

            RegisterNuiCallbackType("setPedComponentVariation");
            EventHandlers["__cfx_nui:setPedComponentVariation"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedComponentVariation);

            RegisterNuiCallbackType("setPedEyeColor");
            EventHandlers["__cfx_nui:setPedEyeColor"] += new Action<int, CallbackDelegate>(NUISetPedEyeColor);

            RegisterNuiCallbackType("setPedHairColor");
            EventHandlers["__cfx_nui:setPedHairColor"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUISetPedHairColor);

            RegisterNuiCallbackType("registerCharacter");
            EventHandlers["__cfx_nui:registerCharacter"] += new Action<IDictionary<string, object>, CallbackDelegate>(NUIRegisterCharacter);
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            // Weather
            ClearOverrideWeather();
            ClearWeatherTypePersist();

            TriggerServerEvent(EventName.Server.GetTimeSync, new Action<string>((arg) =>
            {
                var data = JsonConvert.DeserializeObject<ServerTimeSync>(arg);

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
                    GameCamera.SetCamera(CameraType.Hair, 50f);
                    break;
                case "face":
                    GameCamera.SetCamera(CameraType.Face, 50f);
                    break;
                case "shoes":
                    GameCamera.SetCamera(CameraType.Shoes, 50f);
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

            SetPedHeadBlendData(ped, data.GetInt("shapeFirst"), data.GetInt("shapeSecond"), 0, data.GetInt("skinFirst"), data.GetInt("skinSecond"), 0, data.GetFloat("shapeMix"), data.GetFloat("skinMix"), 0f, false);

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
                    SetPedFaceFeature(ped, (int)feature.Value, float.TryParse(value.ToString(), out var result) ? result : 0);

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
            {
                if (data.TryGetValue(feature.Key, out var value))
                {
                    var _object = value as IDictionary<string, object>;
                    SetPedPropIndex(ped, _object.GetInt("componentId"), _object.GetInt("drawableId"), _object.GetInt("textureId"), _object.GetBool("attach"));
                }
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

                SetPedHeadOverlayColor(ped, _object.GetInt("overlay"), _object.GetInt("colorType"), _object.GetInt("colorId"), _object.GetInt("secondColorId"));
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
                    SetPedComponentVariation(ped, _object.GetInt("componentId"), _object.GetInt("drawableId"), _object.GetInt("textureId"), _object.GetInt("palleteId"));
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

            TriggerServerEvent(EventName.Server.RegisterCharacter, name, lastName, age, slot, appearance, new Action<int>((serverStatus) =>
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
                await Delay(0);

            var account = JsonConvert.DeserializeObject<AccountModel>(json);

            var player = Game.Player;

            if (account.Character.Count <= 0)
            {
                var characterPosition = Creation.Position;
                var heading = Creation.Heading;

                var model = new Model("mp_m_freemode_01");

                while (!await Game.Player.ChangeModel(model)) await Delay(0);

                LoadScene(characterPosition.X, characterPosition.Y, characterPosition.Z);
                SetPedDefaultComponentVariation(PlayerPedId());
                SetPedHeadBlendData(PlayerPedId(), 0, 0, 0, 0, 0, 0, 0f, 0f, 0f, false);
                RequestCollisionAtCoord(characterPosition.X, characterPosition.Y, characterPosition.Z);

                SetEntityCoordsNoOffset(PlayerPedId(), characterPosition.X, characterPosition.Y, characterPosition.Z, false, false, false);
                NetworkResurrectLocalPlayer(characterPosition.X, characterPosition.Y, characterPosition.Z, heading, true, true);
                ClearPedTasksImmediately(PlayerPedId());
                RemoveAllPedWeapons(PlayerPedId(), false);
                ClearPlayerWantedLevel(PlayerId());

                while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await Delay(1);
                
                var groundZ = 0f;
                var ground = GetGroundZFor_3dCoord(characterPosition.X, characterPosition.Y, characterPosition.Z, ref groundZ, false);

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
                
                NuiHelper.SendMessage(new NuiMessage
                {
                    Action = "interface",
                    Key = "creation",
                    Params = new[] { "true", "0" }
                });
                
                GameCamera.SetCamera(CameraType.Entity, 50f);
                
                RenderScriptCams(true, false, 0, true, true);
            }
            else
            {
                var resCharacter = account.Character.SingleOrDefault(x => x.Slot == 0);
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
            TriggerServerEvent(EventName.Server.CharacterRequest, slot, new Action<string>(async (json) =>
            {
                var data = JsonConvert.DeserializeObject<AccountCharacterModel>(json);

                GameCamera.DeleteCamera();

                await EnterCharacter(data);
                RenderScriptCams(false, false, 0, true, true);
            }));
        }

        private async Task EnterCharacter(AccountCharacterModel resCharacter)
        {
            var player = Game.Player;
            var character = player.Character;

            var model = new Model(resCharacter.Model);

            var resCharacterPosition = resCharacter.Position;
            var resCharacterRotation = resCharacter.Rotation;

            while (!await Game.Player.ChangeModel(model)) await Delay(10);

            player.SetEyeColor(resCharacter);
            player.SetHairColor(resCharacter.PedHead);
            player.SetPedHeadBlendDatas(resCharacter.PedHeadData);
            player.SetPedFaceFeatures(resCharacter.PedFace);

            // components e props
            player.SetComponentVariation(resCharacter.PedComponent);
            player.SetPropIndex(resCharacter.PedProp);

            player.SetPedHeadOverlays(resCharacter.PedHeadOverlay);
            player.SetPedHeadOverlayColors(resCharacter.PedHeadOverlayColor);

            //SetEntityCoordsNoOffset(GetPlayerPed(-1), vector3.X, vector3.Y, vector3.Z, false, false, false); ;
            //NetworkResurrectLocalPlayer(vector3.X, vector3.Y, vector3.Z, heading, true, true);
            var groundZ = 0f;
            var ground = GetGroundZFor_3dCoord(resCharacterPosition.X, resCharacterPosition.Y, resCharacterPosition.Z, ref groundZ, false);

            player.Spawn(new Vector3
            {
                X = resCharacterPosition.X,
                Y = resCharacterPosition.Y,
                Z = ground ? groundZ : resCharacterPosition.Z
            });
            
            character.Rotation = new Vector3
            {
                X = resCharacterRotation.X,
                Y = resCharacterRotation.Y,
                Z = resCharacterRotation.Z
            };
            character.Heading = resCharacter.Heading;
            
            character.Armor = resCharacter.Armor;
            character.Health = resCharacter.Health;
            character.MaxHealth = G_Character.MaxHealth;
            
            // https://vespura.com/fivem/gta-stats/
            
            StatSetInt((uint)GetHashKey("MP0_WALLET_BALANCE"), resCharacter.MoneyBalance, true);
            StatSetInt((uint)GetHashKey("BANK_BALANCE"), resCharacter.BankBalance, true);
            
            ClearPedTasksImmediately(character.Handle);
            
            //RemoveAllPedWeapons(GetPlayerPed(-1), false);
            character.Weapons.Drop();
            
            //ClearPlayerWantedLevel(PlayerId());
            player.WantedLevel = 0;
            
            SetNuiFocus(false, false);

            NuiHelper.SendMessage(new NuiMessage
            {
                Action = "interface",
                Key = "creation",
                Params = new[] { "false", "0" }
            });
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

            NuiHelper.SendMessage(new NuiMessage
            {
                Action = "interface",
                Key = "world",
                Params = new object[]
                {
                    G_World.Weather,
                    G_World.RainLevel,
                    G_World.WindSpeed,
                    G_World.WindDirection,
                    G_World.CurrentTime.Hours,
                    G_World.CurrentTime.Minutes,
                    G_World.CurrentTime.Seconds,
                }
            });
            await Delay(1000);
        }

        [Tick] 
        public async Task OnFrame()
        {
            var player = Game.Player;
            var character = player.Character;

            if (character.IsInVehicle())
            {
                var vehicle = character.CurrentVehicle;
                if (vehicle != null)
                {
                    var seatDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver);
                    var isDriver = seatDriver == character;
                    G_Character.DisplayRadar = isDriver;
                    DisplayRadar(isDriver);
                }
            }
            else if (G_Character.DisplayRadar)
            {
                G_Character.DisplayRadar = false;
                DisplayRadar(false);
            }

            await Delay(10);
        }

        [Tick]
        public async Task OnTickOverrideClockTime()
        {
            if (!G_World.HasTime)
                return;

            NetworkOverrideClockTime(G_World.CurrentTime.Hours, G_World.CurrentTime.Minutes, G_World.CurrentTime.Seconds);

            await Delay(10);
        }

        [Tick]
        public Task RemoveNative()
        {
            if (GetPlayerWantedLevel(PlayerId()) != 0)
            {
                SetPlayerWantedLevel(PlayerId(), 0, false);
                SetPlayerWantedLevelNow(PlayerId(), false);
                SetPoliceIgnorePlayer(PlayerId(), true);
                SetDispatchCopsForPlayer(PlayerId(), false);
                SetDitchPoliceModels(false);
            }
            HideHudComponentThisFrame(1);
            HideHudComponentThisFrame(2);
            HideHudComponentThisFrame(3);
            HideHudComponentThisFrame(4);
            HideHudComponentThisFrame(5);
            HideHudComponentThisFrame(6);
            HideHudComponentThisFrame(7);
            HideHudComponentThisFrame(8);
            HideHudComponentThisFrame(9);
            HideHudComponentThisFrame(11);
            HideHudComponentThisFrame(12);
            HideHudComponentThisFrame(13);
            HideHudComponentThisFrame(15);
            HideHudComponentThisFrame(17);
            HideHudComponentThisFrame(18);
            HideHudComponentThisFrame(20);
            HideHudComponentThisFrame(21);
            HideHudComponentThisFrame(22);
            HideHudComponentThisFrame(23);
            HideHudComponentThisFrame(24);
            HideHudComponentThisFrame(25);
            HideHudComponentThisFrame(26);
            HideHudComponentThisFrame(27);
            HideHudComponentThisFrame(28);
            HideHudComponentThisFrame(29);
            HideHudComponentThisFrame(30);
            HideHudComponentThisFrame(31);
            HideHudComponentThisFrame(32);
            HideHudComponentThisFrame(33);
            HideHudComponentThisFrame(34);
            HideHudComponentThisFrame(35);
            HideHudComponentThisFrame(36);
            HideHudComponentThisFrame(37);
            HideHudComponentThisFrame(38);
            HideHudComponentThisFrame(39);
            HideHudComponentThisFrame(40);
            HideHudComponentThisFrame(41);
            HideHudComponentThisFrame(42);
            HideHudComponentThisFrame(43);
            HideHudComponentThisFrame(44);
            HideHudComponentThisFrame(45);
            HideHudComponentThisFrame(46);
            HideHudComponentThisFrame(47);
            HideHudComponentThisFrame(48);
            HideHudComponentThisFrame(49);
            HideHudComponentThisFrame(50);
            HideHudComponentThisFrame(51);
            RemoveAllPickupsOfType(0x6C5B941A);
            RemoveAllPickupsOfType(0xF33C83B0);
            RemoveAllPickupsOfType(0xDF711959);
            RemoveAllPickupsOfType(0xB2B5325E);
            RemoveAllPickupsOfType(0x85CAA9B1);
            RemoveAllPickupsOfType(0xB2930A14);
            RemoveAllPickupsOfType(0xFE2A352C);
            RemoveAllPickupsOfType(0x693583AD);
            RemoveAllPickupsOfType(0x1D9588D3);
            RemoveAllPickupsOfType(0x3A4C2AD2);
            RemoveAllPickupsOfType(0x4D36C349);
            RemoveAllPickupsOfType(0x2F36B434);
            RemoveAllPickupsOfType(0xA9355DCD);
            RemoveAllPickupsOfType(0x96B412A3);
            RemoveAllPickupsOfType(0x9299C95B);
            RemoveAllPickupsOfType(0xF9AFB48F);
            RemoveAllPickupsOfType(0x8967B4F3);
            RemoveAllPickupsOfType(0x3B662889);
            RemoveAllPickupsOfType(0xFD16169E);
            RemoveAllPickupsOfType(0xCB13D282);
            RemoveAllPickupsOfType(0xC69DE3FF);
            RemoveAllPickupsOfType(0x278D8734);
            RemoveAllPickupsOfType(0x5EA16D74);
            RemoveAllPickupsOfType(0x295691A9);
            RemoveAllPickupsOfType(0x81EE601E);
            RemoveAllPickupsOfType(0x88EAACA7);
            RemoveAllPickupsOfType(0x872DC888);
            RemoveAllPickupsOfType(0xC5B72713);
            RemoveAllPickupsOfType(0x9CF13918);
            RemoveAllPickupsOfType(0x0968339D);
            RemoveAllPickupsOfType(0xBFEE6C3B);
            RemoveAllPickupsOfType(0xBED46EC5);
            RemoveAllPickupsOfType(0x079284A9);
            RemoveAllPickupsOfType(0x8ADDEC75);
            DisablePlayerVehicleRewards(PlayerId());
            SetPedInfiniteAmmo(PlayerPedId(), true, (uint)WeaponHash.FireExtinguisher);
            SetCreateRandomCops(false);
            SetGarbageTrucks(false);
            SetRandomBoats(false);
            var coords = GetEntityCoords(PlayerPedId(), true);
            RemoveVehiclesFromGeneratorsInArea(coords.X - 9999.0f, coords.Y - 9999.0f, coords.Z - 9999.0f, coords.X + 9999.0f, coords.Y + 9999.0f, coords.Z + 9999.0f, 0);
            return Task.FromResult(0);
        }

        [Tick]
        public async Task Recoil()
        {
            if (IsPedShooting(PlayerPedId()) && !IsPedDoingDriveby(PlayerPedId()))
            {
                uint weapon = 0;
                if (GetCurrentPedWeapon(PlayerPedId(), ref weapon, true))
                {
                    int ammo = 0;
                    if (GetAmmoInClip(PlayerPedId(), weapon, ref ammo))
                    {
                        if (G_Gun.Recoils.TryGetValue(weapon, out double recoil))
                        {
                            float tv = 0;
                            do
                            {
                                await Delay(10);
                                float p = GetGameplayCamRelativePitch();
                                if (GetFollowPedCamViewMode() != 4)
                                {
                                    SetGameplayCamRelativePitch(p + 0.1f, 0.2f);
                                }
                                tv += 0.1f;
                            } while (tv < recoil);
                        }
                    }
                }
            }
            if (IsPedArmed(PlayerPedId(), 6))
            {
                DisableControlAction(1, 140, true);
                DisableControlAction(1, 141, true);
                DisableControlAction(1, 142, true);

            }
            await Delay(500);
        }

        [Command("forcevehicle")]
        public void ForceVehicle(int src, List<object> args, string raw)
        {
            var model = new Model(args[0].ToString());

            var id = (uint) model.Hash;

            TriggerServerEvent(EventName.Server.ForceVehicle, id, new Action<string>((arg) =>
            {
                Debug.WriteLine(arg);
            }));
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
            NuiHelper.SendMessage(new NuiMessage
            {
                Action = "interface",
                Key = "notification",
                Params = new object[]
                {
                    "info",
                    active == "on" ? "Ciclo mudado para cinema." : "Ciclo definido como pard�o."
                }
            });
        }
    }
}