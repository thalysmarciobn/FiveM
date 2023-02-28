using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
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
            EventHandlers["__cfx_nui:changeModel"] += new Action<string, CallbackDelegate>(async (data, cb) =>
            {
                var player = Game.Player;
                var playerPed = Game.PlayerPed;

                while (!await player.ChangeModel(data)) await Delay(10);

                SetPedDefaultComponentVariation(playerPed.Handle);
                player.SetPedHeadBlendDatas(new AccountCharacterPedHeadDataModel());

                cb(new { status = 1 });
            });

            RegisterNuiCallbackType("setCamera");
            EventHandlers["__cfx_nui:setCamera"] += new Action<string, CallbackDelegate>((data, cb) =>
            {
                Debug.WriteLine(data);
                switch (data)
                {
                    case "face":
                        GameCamera.SetCamera(CameraType.Face, 50f);
                        break;
                    default:
                        GameCamera.SetCamera(CameraType.Entity, 50f);
                        break;
                }

                cb(new { status = 1 });
            });

            RegisterNuiCallbackType("setPedHeadBlend");
            EventHandlers["__cfx_nui:setPedHeadBlend"] += new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
            {
                var player = Game.Player;
                var playerPed = Game.PlayerPed;

                var pedHeadData = new AccountCharacterPedHeadDataModel();

                if (data.TryGetValue("shapeFirst", out var shapeFirst))
                    pedHeadData.ShapeFirstID = int.TryParse(shapeFirst.ToString(), out var result) ? result : 0;

                if (data.TryGetValue("shapeSecond", out var shapeSecond))
                    pedHeadData.ShapeSecondID = int.TryParse(shapeSecond.ToString(), out var result) ? result : 0;

                if (data.TryGetValue("skinFirst", out var skinFirst))
                    pedHeadData.SkinFirstID = int.TryParse(skinFirst.ToString(), out var result) ? result : 0;

                if (data.TryGetValue("skinSecond", out var skinSecond))
                    pedHeadData.SkinSecondID = int.TryParse(skinSecond.ToString(), out var result) ? result : 0;

                if (data.TryGetValue("shapeMix", out var shapeMix))
                    pedHeadData.ShapeMix = float.TryParse(shapeMix.ToString(), out var result) ? result : 0;

                if (data.TryGetValue("skinMix", out var skinMix))
                    pedHeadData.SkinMix = float.TryParse(skinMix.ToString(), out var result) ? result : 0;

                player.SetPedHeadBlendDatas(pedHeadData);
                cb(new { status = 1 });
            });
        }

        public void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            SetVehicleDensityMultiplierThisFrame(0.0f);

            SetPedDensityMultiplierThisFrame(0.0f);

            SetRandomVehicleDensityMultiplierThisFrame(0.0f);

            SetParkedVehicleDensityMultiplierThisFrame(0.0f);

            SetScenarioPedDensityMultiplierThisFrame(0.0f, 0.0f);

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
        }

        private async void InitAccount(string json)
        {
            DoScreenFadeOut(500);
            while (IsScreenFadedOut())
                await Delay(0);

            var account = JsonConvert.DeserializeObject<AccountModel>(json);

            var player = Game.Player;
            var playerPed = Game.PlayerPed;

            var model = new Model("mp_m_freemode_01");

            while (!await Game.Player.ChangeModel(model)) await Delay(10);

            SetPedDefaultComponentVariation(playerPed.Handle);
            player.SetPedHeadBlendDatas(new AccountCharacterPedHeadDataModel());
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

            ShutdownLoadingScreen();

            DoScreenFadeIn(500);
            while (IsScreenFadingIn())
                await Delay(0);

            if (account.Character.Count <= 0)
            {
                var characterPosition = GlobalVariables.Creation.Position;

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

                LoadScene(characterPosition.X, characterPosition.Y, characterPosition.Z);
                RequestCollisionAtCoord(characterPosition.X, characterPosition.Y, characterPosition.Z);

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
            player.SetPedHead(resCharacter.PedHead);
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