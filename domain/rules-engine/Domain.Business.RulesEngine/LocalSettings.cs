using System.Collections.Generic;
using Domain.RulesEngine.Interface;
using Microsoft.Extensions.Configuration;

namespace Domain.RulesEngine.Business
{
    public class LocalSettings : ILocalSettings
    {
        public LocalSettings(IConfiguration configuration)
        {
            ConnectionStrings = new Dictionary<string, string>
            {
                ["RulesEngine"] = configuration.GetSection("ConnectionStrings:RulesEngine").Value

            };
            int.TryParse(configuration["AppSettings:CommandTimeout"], out commandTimeout);
            int.TryParse(configuration["AppSettings:CacheItemExpiry"], out cacheItemExpiry);
        }

        public Dictionary<string, string> ConnectionStrings { get; set; }
        public int CommandTimeout => commandTimeout;
        public int CacheItemExpiry => cacheItemExpiry;

        private int commandTimeout;
        private int cacheItemExpiry;
    }
}