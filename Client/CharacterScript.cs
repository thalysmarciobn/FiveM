using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client;
using Client.Extensions;
using Newtonsoft.Json;
using Shared.Models.Database;
using static CitizenFX.Core.Native.API;

namespace FiveM.Client
{
    public class CharacterScript : BaseScript
    {
        public CharacterScript()
        {
            Debug.WriteLine("[PROJECT] Script: CharacterScript");
            EventHandlers[EventName.Client.InitCharacter] += new Action<string>(InitCharacter);
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