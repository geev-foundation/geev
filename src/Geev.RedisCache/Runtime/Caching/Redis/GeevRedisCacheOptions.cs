using System.Configuration;
using Geev.Configuration.Startup;
using Geev.Extensions;

namespace Geev.Runtime.Caching.Redis
{
    public class GeevRedisCacheOptions
    {
        public IGeevStartupConfiguration GeevStartupConfiguration { get; }

        private const string ConnectionStringKey = "Geev.Redis.Cache";

        private const string DatabaseIdSettingKey = "Geev.Redis.Cache.DatabaseId";

        public string ConnectionString { get; set; }

        public int DatabaseId { get; set; }

        public GeevRedisCacheOptions(IGeevStartupConfiguration geevStartupConfiguration)
        {
            GeevStartupConfiguration = geevStartupConfiguration;

            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        private static int GetDefaultDatabaseId()
        {
            var appSetting = ConfigurationManager.AppSettings[DatabaseIdSettingKey];
            if (appSetting.IsNullOrEmpty())
            {
                return -1;
            }

            int databaseId;
            if (!int.TryParse(appSetting, out databaseId))
            {
                return -1;
            }

            return databaseId;
        }

        private static string GetDefaultConnectionString()
        {
            var connStr = ConfigurationManager.ConnectionStrings[ConnectionStringKey];
            if (connStr == null || connStr.ConnectionString.IsNullOrWhiteSpace())
            {
                return "localhost";
            }

            return connStr.ConnectionString;
        }
    }
}