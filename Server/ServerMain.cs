using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Core;
using Database;
using Microsoft.EntityFrameworkCore;
using Models.Database;
using Newtonsoft.Json;

namespace FiveM.Server
{
    public class ServerMain : BaseScript
    {
        public ServerMain()
        {
            Debug.WriteLine("FiveM Project!");
        }

        [EventHandler("playerConnecting")]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            var license = player.Identifiers["license"];

            deferrals.defer();

            using (var context = new FiveMContext())
            {
                var account = context.Accounts.FirstOrDefault(x => x.License == license);

                if (account == null)
                {
                    account = new AccountModel
                    {
                        License = license,
                        Created = DateTime.Now,
                        WhiteListed = true
                    };
                    context.Accounts.Add(account);
                    context.SaveChanges();
                }

                deferrals.done();

                Debug.WriteLine($"[{account.Id}] Account connected: {account.License}");
            }
        }

        [EventHandler("playerDropped")]
        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var license = player.Identifiers["license"];

            using (var context = new FiveMContext())
            {
                var account = context.Accounts.FirstOrDefault(x => x.License == license);
                if (account != null)
                {
                    Debug.WriteLine($"[{account.Id}] Account {player.Name} dropped for reason: {reason}");
                }
            }
        }

        [EventHandler("ProjectPlayerSpawned")]
        public void ProjectPlayerSpawned([FromSource] Player player)
        {
            Debug.WriteLine("aaaaaaaaaaa");
            var license = player.Identifiers["license"];

            using (var context = new FiveMContext())
            {
                var account = context.Accounts.FirstOrDefault(x => x.License == license);

                if (account == null) return;

                var character = context.AccountCharacter
                    .Include(m => m.Position)
                    .Include(m => m.PedHeadData)
                    .Include(m => m.PedHead)
                    .Include(m => m.PedFace)
                    .Include(m => m.PedComponent)
                    .Include(m => m.PedProp)
                    .Include(m => m.PedHeadOverlay)
                    .Include(m => m.PedHeadOverlayColor)
                    .FirstOrDefault(x => x.AccountId == account.Id);

                if (account == null) return;

                var json = JsonConvert.SerializeObject(character);

                Debug.WriteLine(json);
            }
        }

        [Command("hello_server")]
        public void HelloServer()
        {
            Debug.WriteLine("Sure, hello.");
        }
    }
}