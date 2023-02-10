
public static class EventName
{
    public static class External
    {
        public static class Client
        {
            public const string OnResourceStart = "onResourceStart";
        }
        public static class Server
        {
            public const string PlayerConnecting = "playerConnecting";
            public const string PlayerDropped = "playerDropped";
        }
    }
    public static class Client
    {
        public const string InitCharacter = "InitCharacter";
    }
    public static class Server
    {
        public const string SpawnRequest = "spawnRequest";
        public const string ProjectPlayerPositionUpdate = "ProjectPlayerPositionUpdate";
    }
}