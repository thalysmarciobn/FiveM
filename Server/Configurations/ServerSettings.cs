namespace Server.Configurations
{
    public class ServerSettings
    {
        public Database Database { get; set; }
    }

    public class Database
    {
        public string Server { get; set; }
        public string Schema { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}