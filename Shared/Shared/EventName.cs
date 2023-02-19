
public static class EventName
{
    public static class External
    {
        public static class Client
        {
            public const string OnClientResourceStart = "onClientResourceStart";
            public const string OnClientResourceStop = "onClientResourceStop";
        }
        public static class Server
        {
            public const string PlayerConnecting = "playerConnecting";
            public const string PlayerDropped = "playerDropped";
        }
        public const string OnResourceStart = "onResourceStart";
        public const string OnResourceStop = "onResourceStop";
        public const string OnResourceStarting = "onResourceStarting";
    }
    public static class Client
    {
        public const string InitCharacter = "initCharacter";

        #region Passive
        public const string UpdatePassiveList = "updatePassiveList";
        #endregion
    }
    public static class Server
    {
        public const string SpawnRequest = "spawnRequest";

        public const string GetServiceVehicles = "getServiceVehicles";
        public const string SpawnVehicleService = "spawnVehicleService";

        #region Passive
        public const string SetPassive = "setPassive";
        public const string GetPassive = "getPassive";
        public const string GetPassiveList = "getPassiveList";
        #endregion
    }
}