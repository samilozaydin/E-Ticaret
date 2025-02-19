using Azure.Core.Pipeline;
using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Abstractions.Services.RabbitMQ;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Options;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Cache;
using ETicaretAPI.Infrastructure.Services.Cache.FileCache;
using ETicaretAPI.Infrastructure.Services.Cache.MemoryCache;
using ETicaretAPI.Infrastructure.Services.Cache.Redis;
using ETicaretAPI.Infrastructure.Services.Configurations;
using ETicaretAPI.Infrastructure.Services.RabbitMQ;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection,IConfiguration configure)
        {
            serviceCollection.AddScoped<IStorageService,StorageService>();
            serviceCollection.AddScoped<ICacheService, CacheService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<IApplicationService, ApplicationService>();
            serviceCollection.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = configure["ConnectionStrings:Redis"];
            });
            serviceCollection.AddScoped<IRabbitMQService, RabbitMQService>();

        }
        public static void AddCache<T>(this IServiceCollection serviceCollection, IConfiguration configure) where T : Cache, ICache
        {
            if(typeof(FileCache) == typeof(T))
            {
                FileCacheOptions opt = new FileCacheOptions { DefaultExpiry = TimeSpan.FromMinutes(1) };
                serviceCollection.AddSingleton(opt);
                serviceCollection.AddScoped<ICache, T>();

            }
            else if (typeof(MemoryCache) == typeof(T))
            {
                var memoryCacheOptions = new MemoryCacheOptionsBuilder()
                                            .SetDefaultExpiry(TimeSpan.FromMinutes(1))
                                            .SetCapacity(3,CacheEvictionPolicies.LeastFrequentlyUsed)
                                            .Build();
                serviceCollection.AddSingleton(memoryCacheOptions);
                serviceCollection.AddSingleton<ICache, T>();

            }else if (typeof(RedisCache) == typeof(T))
            {
                var redisOptions = new RedisCacheOptionsBuilder()
                            .SetDefaultExpiry(TimeSpan.FromMinutes(1))
                            .Build();
                serviceCollection.AddSingleton(redisOptions);
                serviceCollection.AddScoped<ICache, T>();
                Console.WriteLine($"BAK ------------------------- : {configure["ConnectionStrings:Redis"]}");
                serviceCollection.AddSingleton<IConnectionMultiplexer,ConnectionMultiplexer>(sp =>
                {
                    var configuration = ConfigurationOptions.Parse(configure["ConnectionStrings:Redis"], true);
                    return ConnectionMultiplexer.Connect(configuration);
                });
            }
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T: Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType) 
        {
            switch(storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();

                    break;
                case StorageType.AWS:

                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }

        }
    }
}
