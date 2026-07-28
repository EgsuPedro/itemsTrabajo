using ItemsTrabajo.BLL.DTOs;
using ItemsTrabajo.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItemsTrabajo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // <-- Requiere JWT válido para cualquier Endpoint de Ítems
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemsController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        [HttpGet]
        public IActionResult ObtenerTodos() => Ok(_workItemService.ObtenerTodos());

        [HttpPost]
        public async Task<IActionResult> CrearItem([FromBody] CrearWorkItemDto dto)
        {
            var itemCreado = await _workItemService.CrearYDistribuirItemAsync(dto);

            if (itemCreado == null)
            {
                return BadRequest("No fue posible asignar el ítem de trabajo. Todos los usuarios están saturados o no existen usuarios disponibles.");
            }

            return CreatedAtAction(nameof(ObtenerTodos), new { id = itemCreado.Id }, itemCreado);
        }
    }
}