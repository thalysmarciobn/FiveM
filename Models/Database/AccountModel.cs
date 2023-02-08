﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Database
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string License { get; set; }
        public DateTime Created { get; set; }
        public bool WhiteListed { get; set; }
    }
}
