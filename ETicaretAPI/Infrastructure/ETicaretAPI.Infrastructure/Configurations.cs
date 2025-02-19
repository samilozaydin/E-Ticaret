using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public static class Configurations
    {
        public static string RedisConnectionString
        {
            get
            {
                ConfigurationManager manager = new ConfigurationManager();
                /* try
                 {
                     manager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ETicaretAPI.API"));
                     manager.AddJsonFile("appsettings.json");
                 }
                 catch
                 {
                     manager.AddJsonFile("appsettings.json");
                 }*/
                Console.WriteLine($"BURADA REDİS KONFIGURASYONU BULUNUYOR {manager.GetConnectionString("Redis")}");
                return manager.GetConnectionString("Redis");
            }
        }
    }
}
