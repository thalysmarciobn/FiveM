using CitizenFX.Core;
using Client.Core;
using Newtonsoft.Json;
using Shared.Models.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class GlobalVariables
    {
        public static readonly bool S_Debug = true;
        public static AccountModel Account { get; set; }
        public static PromptServiceVehicle CurrentPromptServiceVehicle { get; set; }

        public static class Character
        {
            public static AccountCharacterModel Model { get; set; }
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
