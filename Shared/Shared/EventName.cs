
public static class EventName
{
    public static class External
    {
        public static class Client
        {
            public const string OnClientResourceStart = "onClientResourceStart";
        }
        public static class Server
        {
            public const string PlayerConnecting = "playerConnecting";
            public const string PlayerDropped = "playerDropped";
        }
        public const string OnResourceStart = "onResourceStart";
        public const string OnResourceStarting = "onResourceStarting";
    }
    public static class Client
    {
        public const string RequestCreateCharacter = "RequestCreateCharacter";
        public const string InitCharacter = "InitCharacter";
    }
    public static class Server
    {
        public const string SpawnRequest = "spawnRequest";
        public const string ProjectPlayerPositionUpdate = "ProjectPlayerPositionUpdate";
    }
}