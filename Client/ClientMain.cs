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
    public class ClientMain : BaseScript
    {
        public JsonSerializer Serializer = new JsonSerializer
        {
            Culture = CultureInfo.CurrentCulture,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Ignore
        };

        public ClientMain()
        {
            EventHandlers["playerSpawned"] += new Action(() => {
                TriggerServerEvent("ProjectPlayerSpawned"); 
            });
            EventHandlers["ProjectCharacterData"] += new Action<string>(ProjectCharacterData);
        }

        private async void ProjectCharacterData(string json)
        {
            var player = Game.Player;

            var character = Serializer.Deserialize<AccountCharacterModel>(new JsonTextReader(new StringReader(json)));

            player.Spawn(new Vector3
            {
                X = character.Position.X,
                Y = character.Position.Y,
                Z = character.Position.Z
            });

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



            Debug.WriteLine(character.Name);

        }

        [Tick]
        public Task OnTick()
        {
            //DrawRect(0.5f, 0.5f, 0.5f, 0.5f, 255, 255, 255, 150);

            return Task.FromResult(0);
        }
    }
}