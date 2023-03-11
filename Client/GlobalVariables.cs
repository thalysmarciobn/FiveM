using CitizenFX.Core;
using Client.Core;
using Shared.Enumerations;
using System.Collections.Generic;

namespace Client
{
    public class GlobalVariables
    {

        public static readonly bool S_Debug = true;

        public static readonly bool S_FriendlyFire = true;

        public static class World
        {
            private static int _hour { get; set; }
            private static int _minute { get; set; }
            private static int _second { get; set; }
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
            }

            public static int Hour
            {
                get
                {
                    return _hour;
                }
                set
                {
                    _hour = value;
                }
            }

            public static int Minute
            {
                get
                {
                    return _minute;
                }
                set
                {
                    _minute = value;
                }
            }

            public static int Second
            {
                get
                {
                    return _second;
                }
                set
                {
                    _second = value;
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
                    _canUpdate = true;
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
                    _canUpdate = true;
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
                    _canUpdate = true;
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
                    _canUpdate = true;
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