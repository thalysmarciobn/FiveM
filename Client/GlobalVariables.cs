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

        public static class G_Character
        {
            public const int MaxHealth = 500;

            public static bool DisplayRadar = false;
        }

        public static class G_Gun
        {
            public static Dictionary<uint, float> Recoils = new Dictionary<uint, float>()
            {
                { (uint)WeaponHash.Pistol, 0.4f },
                { (uint)WeaponHash.PistolMk2, 0.4f },
                { (uint)WeaponHash.CombatPistol, 0.3f },
                { (uint)WeaponHash.APPistol, 0.4f },
                { 2578377531, 0.6f },
                { 324215364, 0.5f },
                { 736523883, 0.2f },
                { 2024373456, 0.1f },
                { 4024951519, 0.6f },
                { 3220176749, 0.8f },
                { 961495388, 0.2f },
                { 2210333304, 0.8f },
                { 4208062921, 0.1f },
                { 2937143193, 0.1f },
                { 2634544996, 0.1f },
                { 2144741730, 0.1f },
                { 3686625920, 0.1f },
                { 487013001, 0.4f },
                { 1432025498, 0.35f },
                { 2017895192, 0.7f },
                { 3800352039, 0.4f },
                { 2640438543, 0.2f },
                { 911657153, 0.9f },
                { 177293209, 0.6f },
                { 856002082, 1.2f },
                { 2726580491, 1.0f },
                { 1305664598, 1.0f },
                { 2982836145, 0.0f },
                { 1752584910, 0.0f },
                { 1119849093, 0.01f },
                { 3218215474, 0.4f },
                { 1627465347, 0.6f },
                { 3231910285, 0.2f },
                { 3523564046, 0.5f },
                { 2132975508, 0.2f },
                { 137902532, 0.4f },
                { 2828843422, 0.7f },
                { 984333226, 0.2f },
                { 3342088282, 0.3f },
                { 1785463520, 0.25f },
                { 1672152130, 0f },
                { 1198879012, 0.9f },
                { 171789620, 0.1f },
                { 3696079510, 0.9f },
                { 1834241177, 2.4f },
                { 3675956304, 0.3f },
                { 3249783761, 0.6f },
                { 4019527611, 0.7f },
                { 1649403952, 0.3f },
                { 317205821, 0.2f },
                { 125959754, 0.5f },
                { 3173288789, 0.1f }
            };
        }

        public static class Key
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