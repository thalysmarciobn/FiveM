using System;
using System.Collections.Generic;

namespace Shared.Models.Database
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string License { get; set; }
        public long CurrentCharacter { get; set; }
        public string LastAddress { get; set; }
        public DateTime Created { get; set; }
        public bool WhiteListed { get; set; }
        public ICollection<AccountCharacterModel> Character { get; set; } = new List<AccountCharacterModel>();
    }
}