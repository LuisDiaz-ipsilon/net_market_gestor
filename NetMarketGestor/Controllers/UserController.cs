﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

using System.ComponentModel.DataAnnotations;
using NetMarketGestor;
using NetMarketGestor.DTOs;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;

namespace NetMarketGestor.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(IMapper mapper, ApplicationDbContext dbContext, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        // GET: api/User
        [HttpGet]
        [AllowAnonymous]
        [EnableCors("CorsForNinja")]
        public async Task<ActionResult<List<GetUserDTO>>> Get()
        {
            var users = await _dbContext.Users.ToListAsync();
            return _mapper.Map<List<GetUserDTO>>(users);
        }

        // GET: api/User/5
        [HttpGet("{id:int}", Name = "obteneruser")]
        [EnableCors("CorsForNinja")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _dbContext.Set<User>()
                .Include(u => u.Carrito)
                .ThenInclude(c => c.productos)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{nombre}")]
        [AllowAnonymous]
        [EnableCors("CorsForNinja")]
        public async Task<ActionResult<List<GetUserDTO>>> Get([FromRoute] string nombre)
        {
            var users = await _dbContext.Users.Where(userDB => userDB.Nombre.Contains(nombre)).ToListAsync();
            
            return _mapper.Map<List<GetUserDTO>>(users);
        }

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] UserDTO userDTO)
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            //var email = emailClaim.Value;
            var email = userDTO.Email;

            //var usuarioid = usuario.Id;

            var existeUsuario = await _dbContext.Users.AnyAsync(usersDB => usersDB.Email == email);
            if (existeUsuario)
            {
                return BadRequest($"Ya existe un user con el correo {email}");
            }
            var usuario= await userManager.FindByEmailAsync(email);

            var user = _mapper.Map<User>(userDTO);

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            var userDto = _mapper.Map<GetUserDTO>(user);

            return CreatedAtRoute("obteneruser", new { id = user.Id }, userDto);
        }

        // PUT: api/User/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(UserCreacionDTO userCreacionDTO, int id)
        {
            var exist = await _dbContext.Users.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var user = _mapper.Map<User>(userCreacionDTO);
            user.Id = id;

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _dbContext.Users.AnyAsync(x => x.Id == id);
            if (!user)
            {
                return NotFound("No se encontro el user que desea eliminar");
            }

            _dbContext.Remove(new User()
                {
                    Id = id
                });
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("recomendaciones/{userId}")]
        public async Task<ActionResult<List<Product>>> GetRecomendaciones(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Pedidos)
                .ThenInclude(p => p.Productos)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var pedidos = user.Pedidos;

            // Obtener las tres categorías más comunes de los productos de los pedidos
            var categoriasRelevantes = pedidos
                .SelectMany(p => p.Productos)
                .GroupBy(p => p.Categoria)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            // Obtener los productos recomendados de las categorías relevantes
            var recomendaciones = await _dbContext.Products
                .Where(p => categoriasRelevantes.Contains(p.Categoria))
                .ToListAsync();

            return recomendaciones;
        }



    }
}
