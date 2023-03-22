using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core
{
    public class NuiMessage : IDisposable
    {
        public string Action { get; set; }
        public string Key { get; set; }
        public object[] Params { get; set; }

        public void Dispose()
        {
            Action = null;
            Key = null;
            Params = null;
            GC.SuppressFinalize(this);
        }
    }
}
