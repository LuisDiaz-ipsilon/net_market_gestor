using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMarketGestor.Models;

using System.ComponentModel.DataAnnotations;
using NetMarketGestor;
using NetMarketGestor.DTOs;
using System.Collections.Generic;

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

        public UserController(IMapper mapper, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            this.configuration = configuration;
        }

        // GET: api/User
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetUserDTO>>> Get()
        {
            var users = await _dbContext.Users.ToListAsync();
            return _mapper.Map<List<GetUserDTO>>(users);
        }

        // GET: api/User/5
        [HttpGet("{id:int}", Name = "obteneruser")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _dbContext.Set<User>().FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetUserDTO>>> Get([FromRoute] string nombre)
        {
            var users = await _dbContext.Users.Where(userDB => userDB.Nombre.Contains(nombre)).ToListAsync();
            
            return _mapper.Map<List<GetUserDTO>>(users);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDTO userDTO)
        {
            var existeUserMismoNombre = await _dbContext.Users.AnyAsync(x => x.Nombre == userDTO.Nombre);

            if (existeUserMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {userDTO.Nombre}");
            }

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
    }
}
