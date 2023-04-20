using CitizenFX.Core;
using Server.Configurations;
using Server.Controller;
using Server.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Core
{
    public class BaseServerScriptAbstract : BaseScript
    {
        protected ServerController ServerController { get; } = new ServerController();
        protected AuthenticatorController AuthenticatorController { get; } = new AuthenticatorController();
        protected CharacterController CharacterController { get; } = new CharacterController();
        protected TimeSyncController TimeSyncController { get; } = new TimeSyncController();
        protected ItemController ItemController { get; } = new ItemController();
        protected ServerSettings Settings { get; }

        public BaseServerScriptAbstract()
        {
            var directory = Directory.GetCurrentDirectory();
            var location = $"{directory}/server.yml";
            if (!File.Exists(location))
            {
                File.WriteAllText(location, YamlInstance.Instance.SerializerBuilder.Serialize(new ServerSettings
                {
                    Database = new Configurations.Database
                    {
                        Server = "127.0.0.1",
                        Schema = "fivem",
                        Login = "root",
                        Password = "123",
                        Port = 3306
                    }
                }));
            }

            var configuration = File.ReadAllText(location);
            Settings = YamlInstance.Instance.DeserializerBuilder.Deserialize<ServerSettings>(configuration);

            OnStart();
        }

        protected virtual void OnStart()
        {
        }
    }
}
