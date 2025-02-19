using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.DTOs.Caches;
using ETicaretAPI.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Cache.FileCache
{
    public class FileCache : Cache, ICache
    {
        private static readonly JsonSerializerOptions opt = new() { WriteIndented = true };
        private readonly string fileDir;
        private const string FileExtension = ".json";
        private const string SearchPattern = $"*{FileExtension}";
        private const string SearchPatternFormatForKey = $"{{0}}{FileExtension}";

        private readonly FileCacheOptions _fileCacheOptions;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileCache(IWebHostEnvironment webHostEnvironment, FileCacheOptions fileCacheOptions)
        {
            _webHostEnvironment = webHostEnvironment;
            fileDir = Path.Combine(_webHostEnvironment.WebRootPath, "cache-files");
            _fileCacheOptions = fileCacheOptions;
        }

        public bool Contains(string key)
        {
            var format = string.Format(SearchPatternFormatForKey,key);
            var files = Directory.GetFiles(fileDir, searchPattern: format);
            return files.Length > 0;
        }

        public CacheItem Get(string key)
        {
            var filePath = GenerateFileName(key);

            if (!File.Exists(filePath))
                return null;

            using var fs = new FileStream(filePath, FileMode.Open,FileAccess.Read);
            var cacheItem = JsonSerializer.Deserialize<CacheItem>(fs);

            return cacheItem;
        }

        public ReadOnlyDictionary<string, CacheItem> GetItems()
        {
            var files = Directory.GetFiles(fileDir, searchPattern: SearchPattern)
                    .Select(file => Path.GetFileNameWithoutExtension(file));

            var dictfile = files.ToDictionary(key => key, val => Get(val));

            return new ReadOnlyDictionary<string, CacheItem>(dictfile);


        }

        public IEnumerable<string> GetKeys()
        {
            var files = Directory.GetFiles(fileDir, searchPattern: SearchPattern);

            foreach(var file in files)
            {
                yield return Path.GetFileNameWithoutExtension(file);
            }
            

        }
        public IEnumerable<CacheItem> GetValues()
        {
            var entries = GetItems();

            return entries.Values;

        }
        public T GetValue<T>(string key)
        {
            var cacheItem = Get(key) ?? throw new ArgumentException("Item cannot be found");
            return cacheItem is null ? default : cacheItem.GetValue<T>();
        }



        public bool IsExpired(string key)
        {
            var cacheItem = Get(key) ?? throw new ArgumentException("Item cannot be found");
            return cacheItem.ExpirationTime is not null && cacheItem.ExpirationTime < DateTime.UtcNow;
        }

        public bool Remove(string key)
        {
            var filePath = GenerateFileName(key);
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch(FileNotFoundException)
            {
                return false;
            }

        }

        public bool RemoveIfExpired(string key) =>  (IsExpired(key) && Remove(key));
        

        public bool Set(string key, object value, TimeSpan? expirationTime = null)
        {
            var jsonValue = value is not string
                            ? JsonSerializer.Serialize(value, opt)
                            : value.ToString();

            var cacheItem = new CacheItem(key, jsonValue, GetExpiryDate(expirationTime));
            var filePath = GenerateFileName(key);

            if(!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
         
            File.WriteAllText(filePath, JsonSerializer.Serialize(cacheItem, opt));
            return true;
        }

        public bool TryGet(string key, out CacheItem item)
        {
            item = Get(key);

            return item is not null;

        }
        private string GenerateFileName(string key)
        {
            var filename = $"{key}{FileExtension}";
            return Path.Combine(fileDir, filename);
        }
        private DateTime? GetExpiryDate(TimeSpan? expiry)
        {
            TimeSpan? expiryDate = expiry ?? _fileCacheOptions.DefaultExpiry;

            return expiryDate.HasValue
                ? DateTime.UtcNow.Add(expiryDate.Value)
                : default;
        }
    }
}
