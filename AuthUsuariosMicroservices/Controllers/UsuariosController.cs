using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthUsuariosMicroservice.Data;
using AuthUsuariosMicroservice.DTOs;
using AuthUsuariosMicroservice.Entities;
using AuthUsuariosMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthUsuariosMicroservice.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IPasswordHasherService _hasherService;

        public UsuariosController(AuthDbContext context, IPasswordHasherService hasherService)
        {
            _context = context;
            _hasherService = hasherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Estado)
                .Select(u => new UsuarioResponseDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Rol = u.Rol,
                    Estado = u.Estado
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetUsuarioPorId(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null || !u.Estado) return NotFound(new { message = "Usuario no encontrado" });

            return Ok(new UsuarioResponseDto
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Rol = u.Rol,
                Estado = u.Estado
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CrearUsuario([FromBody] UsuarioCreateDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return BadRequest(new { message = "El correo electrónico ya está registrado." });

            _hasherService.CrearPasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Rol = dto.Rol,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarioPorId), new { id = usuario.IdUsuario }, new { id = usuario.IdUsuario, message = "Usuario creado con éxito" });
        }
    }
}