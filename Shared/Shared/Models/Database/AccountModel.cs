using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Database
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string License { get; set; }
        public DateTime Created { get; set; }
        public bool WhiteListed { get; set; }
        public ICollection<AccountCharacterModel> Character { get; set; }
    }
}
