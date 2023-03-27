using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client;
using Client.Core;
using Client.Core.Instances;
using Client.Helper;
using static CitizenFX.Core.Native.API;
using static Client.GlobalVariables;

namespace FiveM.Client
{
    public class ClientMainScript : BaseScriptAbstract
    {
        private PlayerInstance PlayerInstance { get; }
        private NuiInstance NuiInstance { get; }
        private GameInstance GameInstance { get; }
        private CommandInstance CommandInstance { get; }

        public ClientMainScript()
        {
            PlayerInstance  = new PlayerInstance(this);
            NuiInstance = new NuiInstance(this);
            GameInstance = new GameInstance(this);
            CommandInstance = new CommandInstance(this);

            Debug.WriteLine("[PROJECT] Script: CharacterScript");
            RegisterHandler(EventName.External.BaseEvents.OnBaseResourceStart, new Action(OnBaseResourceStart));
            //EventHandlers["onClientResourceStart"] += new Action(OnBaseResourceStart);
            RegisterHandler(EventName.Client.InitAccount, new Action<string>(PlayerInstance.InitAccount));
            RegisterHandler(EventName.Client.UpdatePlayerDataList, new Action<string>(PlayerInstance.UpdatePlayerDataList));

            RegisterNui("characterRequest", new Action<int>(PlayerInstance.CharacterRequest));
            RegisterNui("changeModel", new Action<string, CallbackDelegate>(NuiInstance.NUIChangeModel));
            RegisterNui("setCamera", new Action<string, CallbackDelegate>(NuiInstance.NUIChangeCamera));
            RegisterNui("setPedHeadBlend", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedHeadBlend));
            RegisterNui("setPedFaceFeatures", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedFaceFeatures));
            RegisterNui("setPedProps", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedProps));
            RegisterNui("setPedHeadOverlay", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedHeadOverlay));
            RegisterNui("setPedHeadOverlayColor", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedHeadOverlayColor));
            RegisterNui("setPedComponentVariation", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedComponentVariation));
            RegisterNui("setPedEyeColor", new Action<int, CallbackDelegate>(NuiInstance.NUISetPedEyeColor));
            RegisterNui("setPedHairColor", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUISetPedHairColor));
            RegisterNui("registerCharacter", new Action<IDictionary<string, object>, CallbackDelegate>(NuiInstance.NUIRegisterCharacter));
        }

        protected override void OnClientResourceStart(string resourceName)
        {
            base.OnClientResourceStart(resourceName);

            // Weather
            GameInstance.ClearWeather();
            GameInstance.GetTimeSync();
            GameInstance.GetBlips();

            PlayerInstance.GetPlayerDataList();
        }

        protected override void OnClientResourceStop(string resourceName)
        {
            base.OnClientResourceStop(resourceName);

            GameInstance.RemoveBlips();

            GameCamera.DeleteCamera();
        }

        public PlayerList P_Players()
        {
            return Players;
        }

        #region BaseScript
        public void P_TriggerServerEvent(string name, params object[] args)
        {
            TriggerServerEvent(name, args);
        }

        public Task P_Delay(int delay)
        {
            return Delay(delay);
        }
        #endregion

        public void OnBaseResourceStart()
        {
            TriggerServerEvent(EventName.Server.AccountRequest);
        }

        #region Ticks
        [Tick]
        public async Task TickPlayerData()
        {
            await PlayerInstance.TickPlayerData();
        }

        [Tick]
        public async Task TickOverrideClockTime()
        {
            await GameInstance.TickOverrideClockTime();
        }

        [Tick]
        public async Task TickTimeAndWeather()
        {
            GameInstance.UpdateWeather(); ;

            NuiInstance.UpdateTime();

            await Delay(1000);
        }
        #endregion

        #region Commands
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
            CommandInstance.Fps(active);
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