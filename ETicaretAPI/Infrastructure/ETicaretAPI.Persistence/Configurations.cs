using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence
{
    public static class Configurations
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager manager = new ConfigurationManager();
                manager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../../Presentation/ETicaretAPI.API"));
                manager.AddJsonFile("appsettings.json");
                return manager.GetConnectionString("PostgreSQL");
            }
        }
    }
}
