using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CitizenFX.Core;
using Client.Core.Prompts;

namespace Client
{
    public class GlobalVariables
    {
        public static bool CDebug => true;

        public static class G_Character
        {
            public static int MaxHealth     => 500;

            public static bool Entered { get; set; } = false;

            public static PromptServiceVehicle CurrentPromptServiceVehicle { get; set; }
        }

        public static class G_Key
        {
            public const int OpenPanel              = 344; // F11
            public const int OpenInventory          = 289; // F2
        }

        public static class G_Hud
        {
            public static bool PanelOpened { get; set; } = false;
            public static bool IventoryOpened { get; set; } = false;

            public static int[] DisableKeys { get; } = new int[]
            {
                1, 2, 24, 25, 30, 31, 32, 34, 35, 36, 37
            };
        }

        public static class G_World
        {
            public static bool HasTime { get; set; }
            public static DateTime LastRealTime { get; set; }
            public static DateTime LastServerTime { get; set; }

            private static double TimeElapsed       => DateTime.UtcNow.Subtract(LastRealTime).TotalMilliseconds;

            public static DateTime CurrentDate      => LastServerTime.AddMilliseconds(TimeElapsed);

            public static TimeSpan CurrentTime      => CurrentDate.TimeOfDay;

            private static bool LocCanUpdate { get; set; }
            private static uint LocWeather { get; set; }
            private static float LocRainLevel { get; set; }
            private static float LocWindSpeed { get; set; }
            private static float LocaWindDirection { get; set; }

            public static bool Update
            {
                get
                {
                    if (LocCanUpdate)
                    {
                        LocCanUpdate = false;
                        return true;
                    }

                    return false;
                }
                set => LocCanUpdate = value;
            }

            public static uint LastWeather { get; private set; }

            public static uint Weather
            {
                get => LocWeather;
                set
                {
                    LastWeather = LocWeather;
                    LocWeather = value;
                }
            }

            public static float RainLevel
            {
                get => LocRainLevel;
                set => LocRainLevel = value;
            }

            public static float WindSpeed
            {
                get => LocWindSpeed;
                set => LocWindSpeed = value;
            }

            public static float WindDirection
            {
                get => LocaWindDirection;
                set => LocaWindDirection = value;
            }
        }

        public static class Creation
        {
            public static Vector3 Position { get; } = new Vector3
            {
                X = -1062.02f,
                Y = -2711.85f,
                Z = 0.83f
            };

            public static Vector3 Rotation { get; } = new Vector3
            {
                X = 0,
                Y = 0,
                Z = -135.78f
            };

            public static float Heading => 226.2f;
        }
    }
}