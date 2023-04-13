using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client;
using Client.Core;
using Client.Core.Instances;
using Client.Helper;
using Shared.Helper;
using Shared.Models.Server;
using static CitizenFX.Core.Native.API;
using static Client.GlobalVariables;

namespace FiveM.Client
{
    public class ClientMainScript : BaseScriptAbstract
    {

        public ClientMainScript()
        {
            RegisterHandler(EventName.External.BaseEvents.OnBaseResourceStart, new Action(GameInstance.Instance.OnBaseResourceStart));
            RegisterHandler(EventName.Client.InitAccount, new Action<string>(PlayerInstance.Instance.InitAccount));
            RegisterHandler(EventName.Client.UpdatePlayerDataList, new Action<string>(PlayerInstance.Instance.UpdatePlayerDataList));

            RegisterNui("characterRequest", new Action<int>(PlayerInstance.Instance.CharacterRequest));
            RegisterNui("changeModel", new Action<string, CallbackDelegate>(NuiInstance.Instance.NUIChangeModel));
            RegisterNui("setCamera", new Action<string, CallbackDelegate>(NuiInstance.Instance.NUIChangeCamera));
            RegisterNui("setPedHeadBlend", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedHeadBlend));
            RegisterNui("setPedFaceFeatures", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedFaceFeatures));
            RegisterNui("setPedProps", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedProps));
            RegisterNui("setPedHeadOverlay", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedHeadOverlay));
            RegisterNui("setPedHeadOverlayColor", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedHeadOverlayColor));
            RegisterNui("setPedComponentVariation", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedComponentVariation));
            RegisterNui("setPedEyeColor", new Action<int, CallbackDelegate>(NuiInstance.Instance.NUISetPedEyeColor));
            RegisterNui("setPedHairColor", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUISetPedHairColor));
            RegisterNui("registerCharacter", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.Instance.NUIRegisterCharacter));
        }

        protected override void OnClientResourceStart(string resourceName)
        {
            base.OnClientResourceStart(resourceName);

            if (GetCurrentResourceName() != resourceName) return;

            PlayerInstance.Instance?.GetPlayerDataList();

            // Weather
            GameInstance.Instance?.ClearWeather();

            GameInstance.Instance?.GetTimeSync();
            GameInstance.Instance?.GetBlips();
        }

        protected override void OnClientResourceStop(string resourceName)
        {
            base.OnClientResourceStop(resourceName);

            if (GetCurrentResourceName() != resourceName) return;

            GameInstance.Instance?.RemoveBlips();

            GameCamera.DeleteCamera();
        }

        #region Ticks
        [Tick]
        public async Task TickPlayerData()
        {
            await PlayerInstance.Instance?.TickPlayerData(PlayerList.Players);
        }

        [Tick]
        public async Task TickOverrideClockTime()
        {
            await GameInstance.Instance?.TickOverrideClockTime();
        }

        [Tick]
        public async Task TickTimeAndWeather()
        {
            GameInstance.Instance?.UpdateWeather();

            NuiInstance.Instance?.UpdateTime();

            await Delay(1000);
        }
        #endregion

        #region Commands
        [Command("forcevehicle")]
        public void ForceVehicle(int src, List<object> args, string raw)
        {
            var model = new Model(args[0].ToString());

            var id = (uint)model.Hash;

            TriggerServerEvent(EventName.Server.ForceVehicle, id, new Action<string>(arg => {

                var vehicle = JsonHelper.DeserializeObject<ServerVehicle>(arg);
                Debug.WriteLine(arg);
            }));
        }

        [Command("fps")]
        public void Fps(int src, List<object> args, string raw)
        {
            var active = args[0].ToString();
            CommandInstance.Instance.Fps(active);
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
        #endregion
    }
}