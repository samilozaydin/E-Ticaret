using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Infrastructure.Services.Cache.FileCache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        public ICacheService _cache;

        public CacheController(ICacheService cache)
        {
            this._cache = cache;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Cache(string name)
        {
            var key = name;
            var value = new List<string>() { "blabla", "asafsada", "vasfasf" };
            var expirationTime = TimeSpan.FromSeconds(90);

            _cache.Set(key, value, expirationTime);

            var get = _cache.Get(key);
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(string name)
        {
            var get = _cache.Get(name);
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Var(string name)
        {
            var get = _cache.Contains(name);
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> IsExpired(string name)
        {
            var key = name;

            var get = _cache.IsExpired(key);
            return Ok(get);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(string name)
        {
            var key = name;

            var get = _cache.Remove(key);
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Entries()
        {
            var key = "file_name";

            var get = _cache.GetItems();
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Keys()
        {
            var key = "file_name";

            var get = _cache.GetKeys();
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Values()
        {
            var key = "file_name";

            var get = _cache.GetValues();
            return Ok(get);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Contains()
        {

            var get = _cache.Contains("file_name");
            return Ok(get);
        }
    }
    }
