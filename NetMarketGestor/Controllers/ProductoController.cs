using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.DTOs;
using NetMarketGestor.Models;

using System.ComponentModel.DataAnnotations;


namespace NetMarketGestor.Controllers
{
    [Route("api/productos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration configuration;

        public ProductController(IMapper mapper, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            this.configuration = configuration;
        }

        // GET: api/Product
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetProductDTO>>> Get()
        {
            var products = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<GetProductDTO>>(products);
        }

        // GET: api/Product/5
        [HttpGet("{id:int}", Name = "obtenerproduct")]
        public async Task<ActionResult> Get(int id)
        {
            var product = await _dbContext.Set<Product>().FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Get by name Product
        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetProductDTO>>> Get([FromRoute] string nombre)
        {
            var products = await _dbContext.Products.Where(productsDB => productsDB.Nombre.Contains(nombre)).ToListAsync();
            
            return _mapper.Map<List<GetProductDTO>>(products);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
        {

            var existeProductoMismoNombre = await _dbContext.Products.AnyAsync(x => x.Nombre == productDTO.Nombre);

            if (existeProductoMismoNombre)
            {
                return BadRequest("Ya existe el producto con el mismo nombre");
            }

            var product = _mapper.Map<Product>(productDTO);

            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();

            var productDto = _mapper.Map<GetProductDTO>(product);

            return CreatedAtRoute("obtenerproduct", new {id = product.Id}, productDto);
        }

        // PUT: api/Product/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ProductCreacionDTO productCreacionDTO, int id)
        {

            var exist = await _dbContext.Products.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var product = _mapper.Map<Product>(productCreacionDTO);
            product.Id = id;

            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Product/5
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProductDTO> patchDocument)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDTO = _mapper.Map<ProductDTO>(product);

            patchDocument.ApplyTo(productDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(productDTO, product);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Product/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.AnyAsync(x => x.Id == id);
            if (!product)
            {
                return NotFound("No se encuentra el producto a eliminar");
            }

            _dbContext.Remove(new Product()
            {
                Id = id
            });
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
