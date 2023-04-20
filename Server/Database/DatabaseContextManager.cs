using FiveM.Server.Database;

namespace Server.Database
{
    public static class DatabaseContextManager
    {
        private static string ConnectionString { get; set; }

        public static FiveMContext Context => new FiveMContext(ConnectionString);

        public static void Build(Configurations.Database database)
        {
            ConnectionString =
                $"server={database.Server};database={database.Schema};uid={database.Login};password={database.Password};";
        }
    }
}