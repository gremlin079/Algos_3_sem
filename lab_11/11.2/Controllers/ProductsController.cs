using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Api.Data;
using Northwind.Api.Mappings;
using Northwind.Shared.Products;

namespace Northwind.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(NorthwindContext context) : ControllerBase
{
    private readonly NorthwindContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .OrderBy(p => p.ProductId)
            .Select(p => p.ToDto())
            .ToListAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:int}", Name = nameof(GetByIdAsync))]
    public async Task<ActionResult<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] UpsertProductDto request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity();

        _context.Products.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtRoute(nameof(GetByIdAsync), new { id = entity.ProductId }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> UpdateAsync(int id, [FromBody] UpsertProductDto request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        entity.UpdateFrom(request);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(entity.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}

