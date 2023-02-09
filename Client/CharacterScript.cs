using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
            EventHandlers[EventName.Native.Client.PlayerSpawned] += new Action(() => {
                TriggerServerEvent(EventName.Server.ProjectPlayerSpawned); 
            });
            EventHandlers[EventName.Client.ProjectInitCharacter] += new Action<string>(ProjectInitCharacter);
        }

        private async void ProjectInitCharacter(string json)
        {
            var player = Game.Player;

            var character = Serializer.Deserialize<AccountCharacterModel>(new JsonTextReader(new StringReader(json)));

            var vector3 = new Vector3
            {
                X = character.Position.X,
                Y = character.Position.Y,
                Z = character.Position.Z
            };
            player.Spawn(vector3);

            if (s_Debug)
                Debug.WriteLine($"Spawn: {vector3.X} {vector3.Y} {vector3.Z}");

            Game.PlayerPed.Heading = 226.2f;

            while (!await Game.Player.ChangeModel(new Model(character.Model))) await Delay(10);

            player.SetPedHeadBlendDatas(character.PedHeadData);
            player.SetPedHead(character.PedHead);
            player.SetPedHeadOverlays(character.PedHeadOverlay);
            player.SetPedHeadOverlayColors(character.PedHeadOverlayColor);
            player.SetPedFaceFeatures(character.PedFace);

            player.Character.Armor = character.Armor;

            player.StyleComponents(character.PedComponent);
            player.StyleProps(character.PedProp);

            Game.Player.Unfreeze();

            SwitchInPlayer(PlayerPedId());

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