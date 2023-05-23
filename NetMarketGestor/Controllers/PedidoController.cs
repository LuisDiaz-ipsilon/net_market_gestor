using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

using System.ComponentModel.DataAnnotations;


namespace NetMarketGestor.Controllers
{
    [Route("api/pedidos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PedidoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public PedidoController(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        // GET: api/Pedido
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pedidos = await _dbContext.Set<Pedido>().ToListAsync();
            return Ok(pedidos);
        }

        // GET: api/Pedido/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pedido = await _dbContext.Set<Pedido>().FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        // POST: api/Pedido
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Set<Pedido>().Add(pedido);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = pedido.id }, pedido);
        }

        // PUT: api/Pedido/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Pedido pedido)
        {
            if (id != pedido.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(pedido).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Pedido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _dbContext.Set<Pedido>().FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _dbContext.Set<Pedido>().Remove(pedido);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
