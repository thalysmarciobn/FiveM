using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client.Extensions;
using Newtonsoft.Json;
using Shared.Models.Database;
using static CitizenFX.Core.Native.API;

namespace FiveM.Client
{
    public class CharacterScript : BaseScript
    {
        public static bool s_Debug = true;

        public JsonSerializer Serializer = new JsonSerializer
        {
            Culture = CultureInfo.CurrentCulture,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Ignore
        };

        public CharacterScript()
        {
            Debug.WriteLine("[PROJECT] Script: CharacterScript");
            EventHandlers[EventName.Client.InitCharacter] += new Action<string>(InitCharacter);
        }

        private async void InitCharacter(string json)
        {
            var player = Game.Player;

            DoScreenFadeOut(500);

            while (IsScreenFadedOut())
                Wait(1);

            player.Freeze();

            var character = Serializer.Deserialize<AccountCharacterModel>(new JsonTextReader(new StringReader(json)));

            var heading = 226.2f;

            var model = new Model(character.Model);

            while (!await Game.Player.ChangeModel(model)) await Delay(10);

            Game.PlayerPed.Heading = heading;

            player.Character.Armor = character.Armor;
            player.SetPedHeadBlendDatas(character.PedHeadData);
            player.SetPedHead(character.PedHead);
            player.SetPedHeadOverlays(character.PedHeadOverlay);
            player.SetPedHeadOverlayColors(character.PedHeadOverlayColor);
            player.SetPedFaceFeatures(character.PedFace);

            player.StyleComponents(character.PedComponent);
            player.StyleProps(character.PedProp);

            var position = character.Position;

            //SetEntityCoordsNoOffset(GetPlayerPed(-1), vector3.X, vector3.Y, vector3.Z, false, false, false); ;
            //NetworkResurrectLocalPlayer(vector3.X, vector3.Y, vector3.Z, heading, true, true);
            var groundZ = 0f;
            var ground = GetGroundZFor_3dCoord(position.X, position.Y, position.Z, ref groundZ, false);
            position.Z = ground ? groundZ : position.Z;
            player.Character.Position = new Vector3
            {
                X = position.X,
                Y = position.Y,
                Z = position.Z
            };

            LoadScene(position.X, position.Y, position.Z);
            RequestCollisionAtCoord(position.X, position.Y, position.Z);

            //ClearPedTasksImmediately(GetPlayerPed(-1));

            //RemoveAllPedWeapons(GetPlayerPed(-1), false);
            player.Character.Weapons.Drop();

            //ClearPlayerWantedLevel(PlayerId());
            player.WantedLevel = 0;

            while (!HasCollisionLoadedAroundEntity(Game.PlayerPed.Handle))
                await Delay(1);

            ShutdownLoadingScreen();
            DoScreenFadeIn(500);

            while (IsScreenFadingIn())
                await Delay(1);

            player.Unfreeze();

            if (s_Debug)
                Debug.WriteLine($"Spawn: {position.X} {position.Y} {position.Z}");

            SwitchInPlayer(PlayerPedId());

            //player.Spawn(vector3);

            // API.RequestClipSet(character.WalkingStyle);
            // await BaseScript.Delay(100);
            // Game.Player.Character.MovementAnimationSet = this.WalkingStyle;

        }

        [Tick]
        public Task OnTick()
        {
            var player = Game.Player;

            var position = player.Character.Position;

            //TriggerServerEvent(EventName.Server.ProjectPlayerPositionUpdate, position.X, position.Y, position.Z);
            //Wait(5000);
            return Task.FromResult(0);
        }
    }
}