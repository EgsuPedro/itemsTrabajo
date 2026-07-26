using GestionUsuarios.BLL.DTOs;
using GestionUsuarios.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsuarios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult ObtenerTodos() => Ok(_usuarioService.ObtenerTodos());

        [HttpGet("candidatos")]
        public IActionResult ObtenerCandidatos() => Ok(_usuarioService.ObtenerCandidatos());

        [HttpPost("{username}/asignar")]
        public IActionResult AsignarItem(string username, [FromBody] ItemResumenDto item)
        {
            var exito = _usuarioService.AsignarItem(username, item);
            if (!exito) return BadRequest("No se pudo asignar el ítem. El usuario no existe o está saturado.");
            return Ok(new { Mensaje = $"Item asignado correctamente a {username}" });
        }

        [HttpPatch("{username}/items/{itemId}/completar")]
        public IActionResult CompletarItem(string username, Guid itemId)
        {
            var exito = _usuarioService.CompletarItem(username, itemId);
            if (!exito) return NotFound("Usuario o ítem no encontrado.");
            return Ok(new { Mensaje = "Ítem marcado como completado." });
        }
    }
}