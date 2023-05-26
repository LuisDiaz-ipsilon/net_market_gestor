using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;
using NetMarketGestor;

using System.ComponentModel.DataAnnotations;
using NetMarketGestor.DTOs;

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

        public CarritoController(IMapper mapper, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        // GET: api/Carrito
        /*[HttpGet]
        public async Task<ActionResult<List<GetCarritoDTO>>> Get()
        {
            var carritos = await dbContext.Set<Carrito>().ToListAsync();
            return Ok(carritos);
        }*/

        // GET: api/Carrito/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var carrito = await dbContext.Set<Carrito>().FindAsync(id);

            if (carrito == null)
            {
                return NotFound();
            }

            var carritoProducts = await dbContext.CarritoProductos.ToListAsync();
            var carritoProductsResponse = new List<Product>();
            foreach (var productoCId in carritoProducts) {
                if (productoCId.CarritoId == id) {
                    var product = await dbContext.Set<Product>().FindAsync(productoCId.ProductId);
                    carritoProductsResponse.Add(product);
                }
            }
            carrito.productos = carritoProductsResponse;
            return Ok(carrito);
        }

        // POST: api/Carrito
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarritoDTO carritoDTO)
        {

            var existeCarritoMismoUser = await dbContext.Carritos.AnyAsync(x => x.user.Id == carritoDTO.UserId);
            if (existeCarritoMismoUser)
            {
                return BadRequest("Ya exite un carrito para el mismo usuario");
            }

            var carrito = mapper.Map<Carrito>(carritoDTO);
            carrito.user = await dbContext.Set<User>().FindAsync(carrito.UserId);

            dbContext.Add(carrito);
            await dbContext.SaveChangesAsync();

            var carritoDto = mapper.Map<GetCarritoDTO>(carrito);

            return CreatedAtRoute("obtenercarrito", new { id = carrito.id }, carritoDTO);
        }

        // PUT: api/Carrito/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(CarritoCreacionDTO carritoCreacionDTO, int id)
        {
            var exist = await dbContext.Carritos.AnyAsync(x => x.id == id);
            if (!exist)
            {
                return NotFound();
            }

            var carrito = mapper.Map<Carrito>(carritoCreacionDTO);
            carrito.id = id;

            dbContext.Update(carrito);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Carrito/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var carrito = await dbContext.Carritos.AnyAsync(x => x.id == id);
            if (!carrito)
            {
                return NotFound("No se enucentra el carrito que desea eliminar");
            }

            dbContext.Remove(new Carrito()
            {
                id= id
            });
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Carritos/{id}/agregarProducto
        [HttpPost("{id}/agregarProducto")]
        public async Task<ActionResult> AgregarProducto(int id, [FromBody] AgregarProductCarritoDTO agregarProductCarritoDTO)
        {
            CarritoProducto carritoProducto = new CarritoProducto();
            var carrito = await dbContext.Carritos.Include(c => c.productos).FirstOrDefaultAsync(c => c.id == id);
            if (carrito == null)
            {
                return NotFound("Carrito no encontrado");
            }
            var prdoductObj = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == agregarProductCarritoDTO.Id);
            if (prdoductObj != null)
            {
                carrito.productos.Add(prdoductObj);
                carritoProducto.CarritoId = id;
                carritoProducto.ProductId = agregarProductCarritoDTO.Id;
                dbContext.Update(carrito);
                dbContext.Add(carritoProducto);
                await dbContext.SaveChangesAsync();
                return Ok("Producto agregado al carrito");
            }
            else
            {
                return BadRequest("No se encuentra el producto que desea agregar");
            }


        }

        [HttpDelete("{id}/eliminarProducto")]
        public async Task<IActionResult> EliminarProductoDeCarrito(int id, [FromBody] DTOProductoEnCarrito dtoProductoEnCarrito)
        {
            List<CarritoProducto> carritoProductos = await dbContext.CarritoProductos.ToListAsync<CarritoProducto>();
            foreach (var carritoProducto in carritoProductos) 
            {
                if (carritoProducto.CarritoId == id && dtoProductoEnCarrito.Id == carritoProducto.ProductId)
                {
                    dbContext.Remove(carritoProducto);
                    await dbContext.SaveChangesAsync();
                    break;
                }
            }

            return Ok();
        }

    }
}
