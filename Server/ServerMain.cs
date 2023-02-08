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
            EventHandlers[EventName.ProjectPlayerSpawned] += new Action(ProjectPlayerSpawned);
            Debug.WriteLine("Hi from FiveM.Server!");
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

        public void ProjectPlayerSpawned([FromSource] Player player)
        {
            var license = player.Identifiers["license"];

            using (var context = new FiveMContext())
            {
                var account = context.Accounts.FirstOrDefault(x => x.License == license);

                if (account == null) return;

                var character = context.AccountCharacter.Include(m => new {
                    m.Position,
                    m.PeadHeadData,
                    m.PedHead,
                    m.PedFace,
                    m.PedComponent,
                    m.PedProp,
                    m.PedHeadOverlay,
                    m.PedHeadOverlayColor
                }).FirstOrDefault(x => x.AccountId == account.Id);

                if (account == null) return;

                var json = JsonConvert.SerializeObject(character);
            }
        }

        [Command("hello_server")]
        public void HelloServer()
        {
            Debug.WriteLine("Sure, hello.");
        }
    }
}