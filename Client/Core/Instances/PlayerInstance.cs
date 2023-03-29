using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client.Extensions;
using Client.Helper;
using FiveM.Client;
using Shared.Enumerations;
using Shared.Helper;
using Shared.Models.Database;
using Shared.Models.Messages;
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;
using static CitizenFX.Core.UI.Screen;
using static Client.GameCamera;
using static Client.GlobalVariables;

namespace Client.Core.Instances
{
    public class PlayerInstance : IInstance
    {
        public ClientMainScript Script { get; set; }

        private readonly ConcurrentDictionary<int, ServerPlayer> PlayerDataList = new ConcurrentDictionary<int, ServerPlayer>();

        public PlayerInstance(ClientMainScript script)
        {
            Script = script;
        }

        public void GetPlayerDataList()
        {
            Script.P_TriggerServerEvent(EventName.Server.GetPlayerDataList, new Action<string>(arg =>
            {
                using (var data = JsonHelper.DeserializeObject<PlayerDataListMessage>(arg))
                {
                    foreach (var kvp in data.List)
                        PlayerDataList.TryAdd(kvp.Key, kvp.Value);
                }
            }));
        }

        public void UpdatePlayerDataList(string arg)
        {
            using (var data = JsonHelper.DeserializeObject<PlayerDataListMessage>(arg))
            {
                foreach (var kvp in data.List)
                    PlayerDataList.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => kvp.Value);
            }
        }

        public async void InitAccount(string json)
        {
            DoScreenFadeOut(500);
            while (IsScreenFadedOut())
                await Script.P_Delay(10);

            var account = JsonHelper.DeserializeObject<AccountModel>(json);

            var player = Game.Player;

            if (account.Character.Count <= 0)
            {
                var characterPosition = Creation.Position;
                var heading = Creation.Heading;

                while (!await Game.Player.ChangeModel(PedHash.FreemodeMale01)) await Script.P_Delay(10);

                LoadScene(characterPosition.X, characterPosition.Y, characterPosition.Z);
                SetPedDefaultComponentVariation(PlayerPedId());
                SetPedHeadBlendData(PlayerPedId(), 0, 0, 0, 0, 0, 0, 0f, 0f, 0f, false);
                RequestCollisionAtCoord(characterPosition.X, characterPosition.Y, characterPosition.Z);

                SetEntityCoordsNoOffset(PlayerPedId(), characterPosition.X, characterPosition.Y, characterPosition.Z,
                    false, false, false);
                NetworkResurrectLocalPlayer(characterPosition.X, characterPosition.Y, characterPosition.Z, heading,
                    true, true);
                ClearPedTasksImmediately(PlayerPedId());

                while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await Script.P_Delay(10);

                var groundZ = 0f;
                var ground = GetGroundZFor_3dCoord(characterPosition.X, characterPosition.Y, characterPosition.Z,
                    ref groundZ, false);

                player.Character.Position = new Vector3
                {
                    X = characterPosition.X,
                    Y = characterPosition.Y,
                    Z = ground ? groundZ : characterPosition.Z
                };

                player.Character.IsCollisionEnabled = false;
                player.Character.IsPositionFrozen = true;
                player.Character.IsInvincible = true;

                player.CanControlCharacter = false;

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

            ClearPlayerWantedLevel(PlayerId());
            SetMaxWantedLevel(0);

            ShutdownLoadingScreen();

            DoScreenFadeIn(500);
            while (IsScreenFadingIn()) await Script.P_Delay(1);
        }

        public void CharacterRequest(int slot)
        {
            Script.P_TriggerServerEvent(EventName.Server.CharacterRequest, slot, new Action<string>(async json =>
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

            while (!await Game.Player.ChangeModel(new Model(resCharacter.Model))) await Script.P_Delay(10);

            player.SetEyeColor(resCharacter);
            player.SetHairColor(resCharacter.PedHead);
            player.SetPedHeadBlendDatas(resCharacter.PedHeadData);
            player.SetPedFaceFeatures(resCharacter.PedFace);

            // components e props
            player.SetComponentVariation(resCharacter.PedComponent);
            player.SetPropIndex(resCharacter.PedProp);

            player.SetPedHeadOverlays(resCharacter.PedHeadOverlay);
            player.SetPedHeadOverlayColors(resCharacter.PedHeadOverlayColor);

            while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await Script.P_Delay(10);

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

            if (resCharacter.Health == 0)
                player.Character.Kill();

            G_Character.Entered = true;

            SetNuiFocus(false, false);

            NuiHelper.SendMessage("interface", "creation", new[] { "false", "0" });

            DeleteCamera();

            RenderScriptCams(false, false, 0, true, true);
        }

        public Task TickPlayerData()
        {
            var localPlayer = Game.Player;

            var localPed = localPlayer.Character;
            var localVehicle = localPed?.CurrentVehicle;
            var localHooked = localVehicle?.GetHookedVehicle();

            var localPassive = false;

            if (PlayerDataList.TryGetValue(localPlayer.ServerId, out var isLocalPassive))
                localPassive = isLocalPassive.IsPassive;

            foreach (var player in Script.P_Players())
                if (PlayerDataList.ContainsKey(player.ServerId))
                {
                    var data = PlayerDataList[player.ServerId];

                    var disableCollisions = data.IsPassive || localPassive;

                    if (player.Handle == localPlayer.Handle) continue;

                    var otherPed = player.Character;
                    var otherVehicle = otherPed?.CurrentVehicle;
                    var otherHooked = otherVehicle?.GetHookedVehicle();

                    var alpha = disableCollisions && !GetIsTaskActive(otherPed.Handle, 2) &&
                                localVehicle?.Handle != otherVehicle?.Handle
                        ? 200
                        : 255;
                    otherPed.SetAlpha(alpha);
                    otherVehicle?.SetAlpha(alpha);
                    otherHooked?.SetAlpha(alpha);

                    if (disableCollisions)
                    {
                        otherPed.SetEntityNoCollision(localPed);
                        otherPed?.SetEntityNoCollision(localVehicle);
                        otherPed?.SetEntityNoCollision(localHooked);

                        otherVehicle.SetEntityNoCollision(localPed);
                        otherVehicle?.SetEntityNoCollision(localVehicle);
                        otherVehicle?.SetEntityNoCollision(localHooked);

                        otherHooked.SetEntityNoCollision(localPed);
                        otherHooked?.SetEntityNoCollision(localVehicle);
                        otherHooked?.SetEntityNoCollision(localHooked);
                    }
                }

            var vehicles = World.GetAllVehicles();
            foreach (var vehicle in vehicles)
            {
                const int passiveAlpha = 200;
                const int activeAlpha = 255;
                var alpha = localPassive ? passiveAlpha : activeAlpha;

                if (localVehicle?.Handle == vehicle.Handle)
                    alpha = activeAlpha;

                vehicle.SetAlpha(alpha);

                if (!localPassive && localVehicle?.Handle != vehicle.Handle) continue;

                vehicle.SetEntityNoCollision(localPed);
                vehicle.SetEntityNoCollision(localVehicle);
                vehicle.SetEntityNoCollision(localHooked);
            }

            var peds = World.GetAllPeds();
            foreach (var ped in peds)
            {
                const int passiveAlpha = 200;
                const int activeAlpha = 255;
                var alpha = localPassive ? passiveAlpha : activeAlpha;

                if (localVehicle?.Handle == ped.CurrentVehicle?.Handle)
                    alpha = activeAlpha;

                ped.SetAlpha(alpha);

                if (!localPassive && localVehicle?.Handle != ped.CurrentVehicle?.Handle) continue;

                ped.SetEntityNoCollision(localPed);
                ped.SetEntityNoCollision(localVehicle);
                ped.SetEntityNoCollision(localHooked);
            }

            return Task.FromResult(0);
        }
    }
}
