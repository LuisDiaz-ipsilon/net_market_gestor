using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.DTOs;
using NetMarketGestor.Models;
using NetMarketGestor.Utilidades;
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
            var carritos = await _dbContext.Carritos.ToListAsync();
            foreach (var pedido in pedidos)
            {
                Carrito userCarrito = new Carrito();
                foreach (var carrito in carritos)
                {
                    if (carrito.UserId == pedido.UserId) {
                        userCarrito = carrito;
                        break;
                    }
                }
                var productosinCarrito = await _dbContext.CarritoProductos.ToListAsync();
                List<Product> productos = new List<Product>();
                if (userCarrito.id != 0)
                {
                    foreach (var producto in productosinCarrito)
                    {
                        if (producto.CarritoId == userCarrito.id)
                        {
                            var productInCarrito = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == producto.ProductId);
                            productos.Add(productInCarrito);
                        }
                    }
                }
                pedido.Productos = productos;
            }
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

            int userId = pedidoDTO.userId;

            // Buscar el carrito del usuario y obtener los productos
            var carrito = await _dbContext.Carritos
                .FirstOrDefaultAsync(c => c.id == pedidoDTO.carritoId);

            if (carrito != null)
            {
                var carritoProducts = await _dbContext.CarritoProductos.ToListAsync();
                var carritoProductsResponse = new List<Product>();
                foreach (var productoCId in carritoProducts)
                {
                    if (productoCId.CarritoId == carrito.id)
                    {
                        var product = await _dbContext.Set<Product>().FindAsync(productoCId.ProductId);
                        carritoProductsResponse.Add(product);
                    }
                }
                carrito.productos = carritoProductsResponse;
            }

            if (carrito == null || carrito.productos.Count == 0)
            {
                return BadRequest("El carrito del usuario está vacío");
            }


            // Verificar si todos los productos tienen existencia mayor a cero
            bool existenciaValida = carrito.productos.All(p => p.Existencia == 0);

            if (existenciaValida)
            {
                return BadRequest("Existencia de productos inválida en el carrito");
            }

            //Agregar los productos del carrito al pedido


            //Restar 1 a cada producto consumido
            // Restar uno a la existencia de cada producto en el carrito
            foreach (var producto in carrito.productos)
            {
                producto.Existencia -= 1;
                _dbContext.Entry(producto).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();



            var pedido = _mapper.Map<Pedido>(pedidoDTO);
            pedido.Productos = new List<Product>();

            //Se agregan los productos del carrito del usuario al pedido
            foreach (var producto in carrito.productos)
            {
                pedido.Productos.Add(producto);
            }

            //???

            //carrito = await _dbContext.Set<Carrito>().FindAsync(pedidoDTO.carritoId);
            //pedido.Productos.AddRange(carrito.productos);
            pedido.User = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (pedido.User != null)
            { 
                _dbContext.Add(pedido);
                await _dbContext.SaveChangesAsync();

                var pedidoDto = _mapper.Map<GetPedidoDTO>(pedido);

            
                return CreatedAtRoute("obtenerpedido", new {id = pedido.id}, pedidoDto);
            }
            else
            {
                return NotFound("El usuario no existe");
            }

        }

        // PUT: api/Pedido/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(PUTPedidoDTO pedidoCreacionDTO, int id)
        {
            var exist = await _dbContext.Pedidos.AnyAsync(x => x.id == id);
            var pedidos = await _dbContext.Pedidos.ToListAsync();
            
            if (!exist)
            {
                return NotFound();
            }

            var pedido = _mapper.Map<Pedido>(pedidoCreacionDTO);
            pedido.id = id;

            _dbContext.Update(pedido);
            await _dbContext.SaveChangesAsync();

            //Enviar correo...
            var destinatario = pedido.User.Email;

            //Enviar correo para actualizacion
            Correos correo = new Correos();
            Models.User usuario = new Models.User();
            foreach (var pedidoDto in pedidos)
            {
                if (pedidoDto.id == id)
                {
                    usuario = pedidoDto.User;
                    break;
                }
            }

            if (usuario.Email != "") { 
                correo.EnviarCorreo(usuario.Email, "Nuevo Pedido MarketGestor", "El estatus de tu pedido cambio a" + pedidoCreacionDTO.Estatus);
            }


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

        // GET: api/pedidos/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<GetPedidoDTO>>> GetByUser(int userId)
        {
            var pedidos = await _dbContext.Pedidos
                .Where(pedido => pedido.User.Id == userId)
                .ToListAsync();

            return _mapper.Map<List<GetPedidoDTO>>(pedidos);
        }

    }
}
