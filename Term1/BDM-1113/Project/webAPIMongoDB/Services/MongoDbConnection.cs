namespace webAPIMongoDB.Services
{
    public class MongoDbConnection
    {
        private string connectionString = string.Empty;
        private string databaseName = string.Empty;

        /// <summary>
        /// Creates a new MongoDb connection object
        /// </summary>
        public MongoDbConnection()
        {
            var constructor = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connectionString = constructor.GetSection("MongoDbSettings:ConnectionString").Value.ToString();
            databaseName = constructor.GetSection("MongoDbSettings:DatabaseName").Value.ToString();
        }

        /// <summary>
        /// Return the string connection
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        /// <summary>
        /// Return the database name
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return databaseName;
            }
        }
    }
}
