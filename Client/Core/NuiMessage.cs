using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core
{
    public class NuiMessage
    {
        public string Action { get; set; }
        public string Key { get; set; }
        public string[] Params { get; set; }
    }
}
