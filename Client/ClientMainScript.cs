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
        private bool Boot { get; }
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

            Boot = true;
        }

        protected override void OnClientResourceStart(string resourceName)
        {
            base.OnClientResourceStart(resourceName);

            if (GetCurrentResourceName() != resourceName) return;

            // Weather
            GameInstance.ClearWeather();

            GameInstance.GetTimeSync();
            GameInstance.GetBlips();
            
            PlayerInstance.GetPlayerDataList();
        }

        protected override void OnClientResourceStop(string resourceName)
        {
            base.OnClientResourceStop(resourceName);

            if (GetCurrentResourceName() != resourceName) return;

            GameInstance.RemoveBlips();

            GameCamera.DeleteCamera();
        }

        public void OnBaseResourceStart()
        {
            TriggerServerEvent(EventName.Server.AccountRequest);
        }

        #region Ticks
        [Tick]
        public async Task TickPlayerData()
        {
            if (!Boot) return;

            await PlayerInstance.TickPlayerData();
        }

        [Tick]
        public async Task TickOverrideClockTime()
        {
            if (!Boot) return;

            await GameInstance.TickOverrideClockTime();
        }

        [Tick]
        public async Task TickTimeAndWeather()
        {
            if (Boot)
            {
                GameInstance.UpdateWeather();

                NuiInstance.UpdateTime();
            }

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