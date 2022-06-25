using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Week4Assigment.DistributedCacheProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        AppDbContext _appDbContext;
        public ProductController(AppDbContext appDbContext, IDistributedCache distributedCache)
        {
            _appDbContext = appDbContext;
            _distributedCache = distributedCache;
        }
        [HttpGet("[action]")]
        public async Task<List<Product>> GetAll()
        {
            const string cacheKey = "products";

            var cachedItem = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedItem))
            {
                return JsonSerializer.Deserialize<List<Product>>(cachedItem);
            }
            else
            {
                List<Product> productList = _appDbContext.Products.ToList();
                await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(productList));
                return productList;
            }
        }
    }
}
