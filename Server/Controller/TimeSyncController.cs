using CitizenFX.Core;
using Server.Core;
using Server.Core.Server;
using Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Controller
{
    public class TimeSyncController : AbstractController
    {
        public static bool IsRunning { get; set; } = true;

        public static DateTime CurrentDate => DateTime.Now;
        public static long LastUpdate { get; set; }
        public static long CanUpdate { get; set; }

        private static WeatherEnum _localWeatherType = WeatherEnum.ExtraSunny;
        public static WeatherEnum LastWeatherType { get; private set; }
        public static WeatherEnum CurrentWeather
        {
            get => _localWeatherType;
            private set
            {
                LastWeatherType = _localWeatherType;
                LastUpdate = CurrentDate.Ticks;
                CanUpdate = CurrentDate.AddMinutes(5).Ticks;
                _localWeatherType = value;
            }
        }

        public float WindSpeed
        {
            get
            {
                return WindSpeeds.TryGetValue(_localWeatherType, out var level) ? level : 0;
            }
        }

        public float WindDirection { get; private set; }

        public static float RainLevel
        {
            get
            {
                return RainLevels.TryGetValue(_localWeatherType, out var level) ? level : 0;
            }
        }

        private struct Transition
        {
            public WeatherEnum To { get; set; }
            public float Chance { get; set; }
        }

        private static readonly Dictionary<WeatherEnum, float> WindSpeeds = new Dictionary<WeatherEnum, float>
        {
            { WeatherEnum.ExtraSunny, 0.5f },
            { WeatherEnum.Clear, 1f },
            { WeatherEnum.Clearing, 4f },
            { WeatherEnum.Overcast, 5f },
            { WeatherEnum.Smog, 2f },
            { WeatherEnum.Foggy, 4f },
            { WeatherEnum.Clouds, 5f },
            { WeatherEnum.Raining, 8f },
            { WeatherEnum.ThunderStorm, 12f },
            { WeatherEnum.Snowing, 6f },
            { WeatherEnum.Blizzard, 12f },
            { WeatherEnum.Snowlight, 4f },
            { WeatherEnum.Christmas, 6f },
            { WeatherEnum.Halloween, 12f }
        };

        private static readonly Dictionary<WeatherEnum, float> RainLevels = new Dictionary<WeatherEnum, float>
        {
            { WeatherEnum.Raining, 0.25f },
            { WeatherEnum.ThunderStorm, 0.25f },
            { WeatherEnum.Clearing, 0.1f },
            { WeatherEnum.Halloween, 0.5f }
        };

        private static Dictionary<WeatherEnum, List<Transition>> Transitions = new Dictionary<WeatherEnum, List<Transition>>
        {
            {
                WeatherEnum.ExtraSunny, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Clear, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Overcast, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Clear, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Foggy, Chance = 10 } },
                    { new Transition { To = WeatherEnum.Clear, Chance = 10 } },
                    { new Transition { To = WeatherEnum.Clouds, Chance = 25 } },
                    { new Transition { To = WeatherEnum.Smog, Chance = 25 } },
                    { new Transition { To = WeatherEnum.ExtraSunny, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Clearing, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Foggy, Chance = 10 } },
                    { new Transition { To = WeatherEnum.Clouds, Chance = 25 } },
                    { new Transition { To = WeatherEnum.Smog, Chance = 25 } },
                    { new Transition { To = WeatherEnum.Clear, Chance = 50 } },
                    { new Transition { To = WeatherEnum.ExtraSunny, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Overcast, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Raining, Chance = 10 } },
                    { new Transition { To = WeatherEnum.ThunderStorm, Chance = 5 } },
                    { new Transition { To = WeatherEnum.Clouds, Chance = 25 } },
                    { new Transition { To = WeatherEnum.Smog, Chance = 25 } },
                    { new Transition { To = WeatherEnum.Foggy, Chance = 25 } },
                    { new Transition { To = WeatherEnum.Clear, Chance = 50 } },
                    { new Transition { To = WeatherEnum.ExtraSunny, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Smog, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Clear, Chance = 100 } }
                }
            },
            {
                WeatherEnum.Foggy, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Raining, Chance = 10 } },
                    { new Transition { To = WeatherEnum.Clear, Chance = 100 } }
                }
            },
            {
                WeatherEnum.Clouds, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Raining, Chance = 10 } },
                    { new Transition { To = WeatherEnum.Clearing, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Overcast, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Raining, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Clearing, Chance = 100 } }
                }
            },
            {
                WeatherEnum.ThunderStorm, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Clearing, Chance = 100 } }
                }
            },
            {
                WeatherEnum.Snowing, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Clearing, Chance = 5 } },
                    { new Transition { To = WeatherEnum.Overcast, Chance = 5 } },
                    { new Transition { To = WeatherEnum.Foggy, Chance = 5 } },
                    { new Transition { To = WeatherEnum.Clouds, Chance = 5 } },
                    { new Transition { To = WeatherEnum.Christmas, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Snowlight, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Blizzard, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Blizzard, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Christmas, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Snowlight, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Snowlight, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Snowing, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Christmas, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Christmas, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Snowing, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Snowlight, Chance = 50 } },
                    { new Transition { To = WeatherEnum.Blizzard, Chance = 50 } }
                }
            },
            {
                WeatherEnum.Halloween, new List<Transition>
                {
                    { new Transition { To = WeatherEnum.Clearing, Chance = 100 } }
                }
            }
        };

        private static readonly Random Random = new Random();

        private static readonly WeatherEnum[] Available = new[]
        {
            WeatherEnum.Clear,
            WeatherEnum.ExtraSunny,
            WeatherEnum.Clouds,
            WeatherEnum.Overcast,
            WeatherEnum.Raining,
            WeatherEnum.Clearing
        };

        private static Dictionary<WeatherEnum, List<Transition>> AvaiableTransation = new Dictionary<WeatherEnum, List<Transition>>();

        public void Initialize()
        {
            CurrentWeather = WeatherEnum.ExtraSunny;
            foreach (var transition in Transitions)
            {
                if (Available.Contains(transition.Key))
                {
                    var avaiableTransation = new List<Transition>();
                    foreach (var transition2 in transition.Value)
                        if (Available.Contains(transition2.To))
                            avaiableTransation.Add(transition2);
                    AvaiableTransation.Add(transition.Key, avaiableTransation);
                }
            }
        }

        public void Next()
        {
            if (AvaiableTransation.TryGetValue(CurrentWeather, out var transition))
            {
                var rand = Random.Next(0, 100);
                WindDirection = Random.Next(0, 7);
                var linq = transition.Where(x => x.Chance >= rand);
                var count = linq.Count();
                if (count == 1)
                {
                    var currentTrasation = linq.First();
                    CurrentWeather = currentTrasation.To;
                }
                else if (count > 1)
                {
                    var chance = Random.Next(0, count);
                    var list = linq.ToArray();
                    var element = list.ElementAt(chance);
                    CurrentWeather = element.To;
                }
                else
                    Next();
            }
        }

        public void Update(uint weather)
        {
            CurrentWeather = (WeatherEnum)weather;
        }
    }
}
