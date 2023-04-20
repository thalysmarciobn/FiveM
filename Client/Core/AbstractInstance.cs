using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core
{
    public abstract class AbstractInstance<T> where T : new()
    {
        private static T s_Instance { get; set; }

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new T();
                return s_Instance;
            }
        }
    }
}
