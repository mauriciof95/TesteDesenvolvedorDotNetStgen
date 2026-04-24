using Newtonsoft.Json;
using System.Reflection;


namespace GoodHamburger.Infrastructure.Configuration;

public class Configuration
{
    public static ApiConfiguration config { get; set; }
}

public class DatabaseConfig
{
    public string ConnectionString { get; set; }
}

public class ApiConfiguration
{
    public DatabaseConfig databaseConfig { get; set; }

    public static DatabaseConfig GetDatabaseConfig()
    {
        return LoadJson()?.databaseConfig;
    }

    private static ApiConfiguration LoadJson()
    {
        if (Configuration.config == null)
        {
            string file;

            file = "appsettings.Development.json";


            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configPath = Path.Combine(path, file);

            if (!File.Exists(configPath))
                configPath = Path.Combine("..", "GoodHamburger.Api", file);

            var reader = new JsonTextReader(new StringReader(File.ReadAllText(configPath)));

            var serializer = new JsonSerializer();
            var configuration = serializer.Deserialize<ApiConfiguration>(reader);

            if (configuration == null)
                throw new ArgumentException("Error on load appsettings.json");

            Configuration.config = configuration;
        }

        return Configuration.config;
    }
}
