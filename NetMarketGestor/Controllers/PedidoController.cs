using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.DTOs;
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
        private readonly IConfiguration configuration;

        public PedidoController(IMapper mapper, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            this.configuration = configuration;
        }

        // GET: api/Pedido
        [HttpGet]
        public async Task<ActionResult<List<GetPedidoDTO>>> Get()
        {
            var pedidos = await _dbContext.Pedidos.ToListAsync();
            return Ok(pedidos);
        }

        // GET: api/Pedido/5
        [HttpGet("{id:int}", Name = "obtenerpedido")]
        public async Task<ActionResult> Get(int id)
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
        public async Task<ActionResult> Post([FromBody] PedidoDTO pedidoDTO)
        {
            var existePedido = await _dbContext.Pedidos.AnyAsync(x => x.id == pedidoDTO.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest("Ya existe este carrito");
            }

            var pedido = _mapper.Map<Pedido>(pedidoDTO);

            _dbContext.Add(pedido);
            await _dbContext.SaveChangesAsync();

            var pedidoDto = _mapper.Map<GetPedidoDTO>(pedido);

            return CreatedAtRoute("obtenerpedido", new {id = pedido.id}, pedidoDto);
        }

        // PUT: api/Pedido/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(PedidoCreacionDTO pedidoCreacionDTO, int id)
        {
            var exist = await _dbContext.Pedidos.AnyAsync(x => x.id == id);
            
            if (!exist)
            {
                return NotFound();
            }

            var pedido = _mapper.Map<Pedido>(pedidoCreacionDTO);
            pedido.id = id;

            _dbContext.Update(pedido);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Pedido/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var exist = await _dbContext.Pedidos.AnyAsync(x => x.id == id);

            if (!exist)
            {
                return NotFound("No se encontro el pedido a eliminar");
            }

            _dbContext.Remove(
                new Pedido()
                {
                    id = id
                });
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
