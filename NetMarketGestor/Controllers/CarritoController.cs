using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;
using NetMarketGestor;

using System.ComponentModel.DataAnnotations;


namespace NetMarketGestor.Controllers
{
    [Route("api/carritos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarritoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;

        public CarritoController(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        // GET: api/Carrito
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var carritos = await dbContext.Set<Carrito>().ToListAsync();
            return Ok(carritos);
        }

        // GET: api/Carrito/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var carrito = await dbContext.Set<Carrito>().FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            return Ok(carrito);
        }

        // POST: api/Carrito
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.Set<Carrito>().Add(carrito);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = carrito.id }, carrito);
        }

        // PUT: api/Carrito/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Carrito carrito)
        {
            if (id != carrito.id)
            {
                return BadRequest();
            }

            dbContext.Entry(carrito).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Carrito/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var carrito = await dbContext.Set<Carrito>().FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }

            dbContext.Set<Carrito>().Remove(carrito);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
