using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

using System.ComponentModel.DataAnnotations;

namespace NetMarketGestor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarritoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly DbContext dbContext;

        public CarritoController(IMapper mapper, DbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        // GET: api/Carrito
        [HttpGet]
        public IActionResult Get()
        {
            var carritos = dbContext.Set<Carrito>();
            return Ok(carritos);
        }

        // GET: api/Carrito/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var carrito = dbContext.Set<Carrito>().Find(id);
            if (carrito == null)
            {
                return NotFound();
            }
            return Ok(carrito);
        }

        // POST: api/Carrito
        [HttpPost]
        public IActionResult Post([FromBody] Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.Set<Carrito>().Add(carrito);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = carrito.id }, carrito);
        }

        // PUT: api/Carrito/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Carrito carrito)
        {
            if (id != carrito.id)
            {
                return BadRequest();
            }

            dbContext.Entry(carrito).State = EntityState.Modified;
            dbContext.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Carrito/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var carrito = dbContext.Set<Carrito>().Find(id);
            if (carrito == null)
            {
                return NotFound();
            }

            dbContext.Set<Carrito>().Remove(carrito);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
