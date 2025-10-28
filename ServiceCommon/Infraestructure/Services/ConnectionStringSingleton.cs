namespace ServiceCommon.Infrastructure.Services
{
    public class ConnectionStringSingleton
    {
        private static ConnectionStringSingleton instance;
        private static readonly object _lock = new object();

        private readonly IConfiguration _configuration;

        private ConnectionStringSingleton()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static ConnectionStringSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectionStringSingleton();
                        }
                    }
                }
                return instance;
            }
        }

        public string PostgresConnection
        {
            get
            {
                return _configuration.GetConnectionString("Postgres");
            }
        }
    }
}