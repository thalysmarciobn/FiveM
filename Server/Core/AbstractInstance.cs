namespace Server.Core
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