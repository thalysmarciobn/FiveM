using System;

namespace Server.Core
{
    public abstract class AbstractInstance<T> where T : new()
    {
        private static readonly Lazy<T> s_Instance = new Lazy<T>(() => new T());

        public static T Instance
        {
            get { return s_Instance.Value; }
        }
    }
}