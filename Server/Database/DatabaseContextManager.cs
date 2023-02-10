using FiveM.Server.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Database
{
    public static class DatabaseContextManager
    {
        private static string _connectionString { get; set; }

        public static FiveMContext Context => new FiveMContext(_connectionString);

        public static void Build(Configurations.Database database) =>
            _connectionString = $"server={database.Server};database={database.Schema};uid={database.Login};password={database.Password};";
    }
}
