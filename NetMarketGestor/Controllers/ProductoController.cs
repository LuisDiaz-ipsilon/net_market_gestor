using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

using System.ComponentModel.DataAnnotations;


namespace NetMarketGestor.Controllers
{
    [Route("api/productos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public ProductController(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _dbContext.Set<Product>().ToListAsync();
            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _dbContext.Set<Product>().FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Set<Product>().Add(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Set<Product>().FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Set<Product>().Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
