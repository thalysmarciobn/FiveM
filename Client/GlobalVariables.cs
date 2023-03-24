using System;
using CitizenFX.Core;
using Client.Core;

namespace Client
{
    public class GlobalVariables
    {
        public static readonly bool S_Debug = true;

        public static class G_Character
        {
            public const int MaxHealth = 500;

            public static bool Entered = false;

            public static PromptServiceVehicle CurrentPromptServiceVehicle { get; set; }
        }

        public static class G_Key
        {
            public const int OpenPanel = 344; // F11
        }

        public static class G_Hud
        {
            public static bool PanelOpened = false;
        }

        public static class G_World
        {
            public static bool HasTime { get; set; }
            public static DateTime LastRealTime { get; set; }
            public static DateTime LastServerTime { get; set; }

            private static double _timeElapsed => DateTime.UtcNow.Subtract(LastRealTime).TotalMilliseconds;

            public static DateTime CurrentDate => LastServerTime.AddMilliseconds(_timeElapsed);

            public static TimeSpan CurrentTime => CurrentDate.TimeOfDay;

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
                set => _canUpdate = value;
            }

            public static uint LastWeather { get; private set; }

            public static uint Weather
            {
                get => _weather;
                set
                {
                    LastWeather = _weather;
                    _weather = value;
                }
            }

            public static float RainLevel
            {
                get => _rainLevel;
                set => _rainLevel = value;
            }

            public static float WindSpeed
            {
                get => _windSpeed;
                set => _windSpeed = value;
            }

            public static float WindDirection
            {
                get => _windDirection;
                set => _windDirection = value;
            }
        }

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