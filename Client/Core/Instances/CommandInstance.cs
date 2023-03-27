using Client.Helper;
using FiveM.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Core.Instances
{
    internal class CommandInstance : IInstance
    {
        public ClientMainScript Script { get; }

        public CommandInstance(ClientMainScript script)
        {
            Script = script;
        }

        public void Fps(string active)
        {
            switch (active)
            {
                case "on":
                    SetTimecycleModifier("cinema");
                    break;
                case "off":
                default:
                    SetTimecycleModifier("default");
                    break;
            }

            NuiHelper.SendMessage("interface", "notification", new object[]
            {
                "info",
                active == "on" ? "Ciclo mudado para cinema." : "Ciclo definido como pardão."
            });
        }
    }
}
