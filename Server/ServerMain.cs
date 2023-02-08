using System;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace FiveM.Server
{
    public class ServerMain : BaseScript
    {
        public ServerMain()
        {
            Debug.WriteLine("Hi from FiveM.Server!");
        }

        [EventHandler("playerConnecting")]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {

        }

        [Command("hello_server")]
        public void HelloServer()
        {
            Debug.WriteLine("Sure, hello.");
        }
    }
}