using CitizenFX.Core;
using Client.Core;
using Shared.Enumerations;
using System;
using System.Collections.Generic;

namespace Client
{
    public class GlobalVariables
    {

        public static readonly bool S_Debug = true;

        public static readonly bool S_FriendlyFire = true;

        public static class Character
        {
            public const int MaxHealth = 500;

            public static bool LocallyVisible = false;
            public static bool AllInvisible = false;
        }

        public static class Key
        {
            public const int OpenPanel = 344; // F11
        }

        public static class Hud
        {
            public static bool PanelOpened = false;
        }

        public static class G_World
        {
            public static bool HasTime { get; set; }
            public static DateTime LastRealTime { get; set; }
            public static DateTime LastServerTime { get; set; }

            private static double _timeElapsed
            {
                get
                {
                    return DateTime.UtcNow.Subtract(LastRealTime).TotalMilliseconds;
                }
            }

            public static DateTime CurrentDate
            {
                get
                {
                    return LastServerTime.AddMilliseconds(_timeElapsed);
                }
            }

            public static TimeSpan CurrentTime
            {
                get
                {
                    return CurrentDate.TimeOfDay;
                }
            }

            private static uint _weather { get; set; }
            private static float _rainLevel { get; set; }
            private static float _windSpeed { get; set; }
            private static float _windDirection { get; set; }
            private static bool _canUpdate { get; set; }
            public static bool Update
            {
                get
                {
                    if (_canUpdate)
                    {
                        _canUpdate = false;
                        return true;
                    }
                    return false;
                }
                set
                {
                    _canUpdate = value;
                }
            }

            public static uint LastWeather { get; private set; }

            public static uint Weather
            {
                get
                {
                    return _weather;
                }
                set
                {
                    LastWeather = _weather;
                    _weather = value;
                }
            }

            public static float RainLevel
            {
                get
                {
                    return _rainLevel;
                }
                set
                {
                    _rainLevel = value;
                }
            }

            public static float WindSpeed
            {
                get
                {
                    return _windSpeed;
                }
                set
                {
                    _windSpeed = value;
                }
            }

            public static float WindDirection
            {
                get
                {
                    return _windDirection;
                }
                set
                {
                    _windDirection = value;
                }
            }
        }

        public static PromptServiceVehicle CurrentPromptServiceVehicle { get; set; }

        public static class Creation
        {
            public static readonly Vector3 Position = new Vector3
            {
                X = -1062.02f,
                Y = -2711.85f,
                Z = 0.83f
            };

            public static readonly Vector3 Rotation = new Vector3
            {
                X = 0,
                Y = 0,
                Z = -135.78f
            };

            public static readonly float Heading = 226.2f;
        }
    }
}