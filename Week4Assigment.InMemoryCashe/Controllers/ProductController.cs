using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Week4Assigment.InMemoryCashe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IMemoryCache _memoryCache;
        AppDbContext _appDbContext;
        public ProductController(IMemoryCache memoryCache, AppDbContext appDbContext)
        {
            _memoryCache = memoryCache;
            _appDbContext = appDbContext;
        }
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            const string key = "products";

            if (_memoryCache.TryGetValue(key, out object list))
                return Ok(list);
            var productList = _appDbContext.Products.Select(x => new
            {
                x.ProductName,
                x.Price,
                x.Stock
            }).ToList();
            _memoryCache.Set(key, productList, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                Priority = CacheItemPriority.Normal
            });
            return Ok(productList);
        }
    }
}
