using Microsoft.Extensions.Configuration;

namespace WebsiteManagement.Application.IntegrationTests
{
    public static class Configuration
    {
        static Configuration()
        {
            Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public static IConfiguration Config { get; }
    }
}
