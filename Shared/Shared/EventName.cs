
public static class EventName
{
    public static class Native
    {
        public static class Client
        {
            public const string PlayerSpawned = "playerSpawned";
        }
        public static class Server
        {
            public const string PlayerConnecting = "playerConnecting";
            public const string PlayerDropped = "playerDropped";
        }
    }
    public static class Client
    {
        public const string ProjectInitCharacter = "ProjectInitCharacter";
    }
    public static class Server
    {
        public const string ProjectPlayerSpawned = "ProjectPlayerSpawned";
        public const string ProjectPlayerPositionUpdate = "ProjectPlayerPositionUpdate";
    }
}