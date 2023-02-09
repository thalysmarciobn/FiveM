using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using Shared.Models.Database;
using static CitizenFX.Core.Native.API;

namespace FiveM.Client
{
    public class ClientMain : BaseScript
    {
        public JsonSerializer Serializer = new JsonSerializer
        {
            Culture = CultureInfo.CurrentCulture,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Ignore
        };

        public ClientMain()
        {
            EventHandlers["playerSpawned"] += new Action(() => {
                TriggerServerEvent("ProjectPlayerSpawned"); 
            });
            EventHandlers["ProjectCharacterData"] += new Action<string>(ProjectCharacterData);
        }

        private void ProjectCharacterData(string json)
        {
            Debug.WriteLine("xxxxxxxxxxx");
            Debug.WriteLine(json);
            AccountCharacterModel jsonResponse = Serializer.Deserialize<AccountCharacterModel>(new JsonTextReader(new StringReader(json)));
            Debug.WriteLine(jsonResponse.Name);
        }

        [Tick]
        public Task OnTick()
        {
            //DrawRect(0.5f, 0.5f, 0.5f, 0.5f, 255, 255, 255, 150);

            return Task.FromResult(0);
        }
    }
}