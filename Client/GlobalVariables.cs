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
            public static int Hour { get; set; }
            public static int Minute { get; set; }
            public static int Second { get; set; }
            public static uint Weather { get; set; }
            public static float RainLevel { get; set; }
            public static float WindSpeed { get; set; }
            public static float WindDirection { get; set; }
            public static bool HasOverrideClockTime { get; set; }
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