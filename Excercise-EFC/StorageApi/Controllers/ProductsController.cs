using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StorageApi.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StorageApiContext _context;
    private readonly IMapper _mapper;

    public ProductsController(StorageApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/products
    // Optional querystring: ?category=CategoryName
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct([FromQuery] string category = null)
    {
        IQueryable<Product> query = _context.Product;

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category == category);
        }

        var products = await query.ToListAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products).ToList();
    }

    // GET: api/products/stats
    [HttpGet("stats")]
    public async Task<ActionResult<ProductStatsDto>> GetProductStats()
    {
        var products = await _context.Product.ToListAsync();

        if (products.Count == 0)
        {
            return new ProductStatsDto
            {
                TotalProductCount = 0,
                TotalInventoryValue = 0,
                AveragePrice = 0
            };
        }

        var stats = new ProductStatsDto
        {
            TotalProductCount = products.Count,
            TotalInventoryValue = products.Sum(p => p.Price * p.Count),
            AveragePrice = (decimal)products.Average(p => p.Price)
        };

        return stats;
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Product.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return _mapper.Map<ProductDto>(product);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int? id, CreateProductDto createProductDto)
    {
        if (id == null || id <= 0)
        {
            return BadRequest();
        }

        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _mapper.Map(createProductDto, product);
        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        _context.Product.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, _mapper.Map<ProductDto>(product));
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int? id)
    {
        return _context.Product.Any(e => e.Id == id);
    }
}
