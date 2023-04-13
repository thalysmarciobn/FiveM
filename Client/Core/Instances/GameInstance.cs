using CitizenFX.Core;
using FiveM.Client;
using Shared.Helper;
using Shared.Models.Database;
using Shared.Models.Messages;
using Shared.Models.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static Client.GlobalVariables;

namespace Client.Core.Instances
{
    public class GameInstance : AbstractInstance<GameInstance>
    {
        private readonly Dictionary<long, int> Blips = new Dictionary<long, int>();

        public void OnBaseResourceStart()
        {
            BaseScript.TriggerServerEvent(EventName.Server.AccountRequest);
        }

        public void ClearWeather()
        {
            ClearOverrideWeather();
            ClearWeatherTypePersist();
        }

        public void GetTimeSync()
        {
            BaseScript.TriggerServerEvent(EventName.Server.GetTimeSync, new Action<string>(arg =>
            {
                using (var data = JsonHelper.DeserializeObject<ServerTimeSyncMessage>(arg))
                {
                    Debug.WriteLine(arg);
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
                }
            }));
        }

        public void GetBlips()
        {
            BaseScript.TriggerServerEvent(EventName.Server.GetBlips, new Action<string>(arg =>
            {
                using (var data = JsonHelper.DeserializeObject<BlipListMessage>(arg))
                {

                    foreach (var blip in data.List)
                    {
                        var id = blip.Key;
                        var model = blip.Value;

                        var blipId = AddBlipForCoord(model.X, model.Y, model.Z);

                        SetBlipSprite(blipId, model.BlipId);
                        SetBlipDisplay(blipId, model.DisplayId);
                        SetBlipScale(blipId, model.Scale);
                        SetBlipColour(blipId, model.Color);
                        SetBlipAsShortRange(blipId, model.ShortRange);

                        BeginTextCommandSetBlipName("STRING");
                        AddTextComponentString(model.Title);
                        EndTextCommandSetBlipName(blipId);

                        Blips.Add(id, blipId);
                    }
                }
            }));
        }

        public void RemoveBlips()
        {
            foreach (var blip in Blips)
            {
                var blipId = blip.Value;
                RemoveBlip(ref blipId);
            }
        }

        public async Task TickOverrideClockTime()
        {
            if (!G_World.HasTime)
                return;

            NetworkOverrideClockTime(G_World.CurrentTime.Hours, G_World.CurrentTime.Minutes,
                G_World.CurrentTime.Seconds);

            await BaseScript.Delay(100);
        }

        public void UpdateWeather()
        {
            if (!G_World.Update)
                return;

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
    }
}
