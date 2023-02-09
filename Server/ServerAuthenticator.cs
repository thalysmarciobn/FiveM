using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using FiveM.Server.Database;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Database;
using Newtonsoft.Json;
using Server.Instances;

namespace FiveM.Server
{
    public class ServerAuthenticator : BaseScript
    {
        public static bool s_Debug = true;
        public ServerAuthenticator()
        {
            Debug.WriteLine("FiveM Project!");
        }

        [EventHandler(EventName.Native.Server.PlayerConnecting)]
        private void PlayerConnecting([FromSource] Player player, string playerName, dynamic kickCallback, dynamic deferrals)
        {
            var license = player.Identifiers["license"];

            deferrals.defer();

            using (var context = new FiveMContext())
            {
                var account = context.Accounts.FirstOrDefault(x => x.License == license);

                if (account == null)
                {
                    account = new AccountModel()
                    {
                        License = license,
                        Created = DateTime.Now,
                        WhiteListed = true
                    };
                    context.Accounts.Add(account);
                    context.SaveChanges();
                }
                else
                {
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

                    CharacterInstance.Instance.AddCharacter(license, character);
                }

                deferrals.done();

                Debug.WriteLine($"[{account.Id}] Account connected: {account.License}");
            }
        }

        [EventHandler(EventName.Native.Server.PlayerDropped)]
        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var license = player.Identifiers["license"];

            if (CharacterInstance.Instance.RemoveCharacter(license, out AccountCharacterModel account))
            {
                Debug.WriteLine($"[{account.Id}] Account {player.Name} dropped for reason: {reason}");
            }
        }
    }
}