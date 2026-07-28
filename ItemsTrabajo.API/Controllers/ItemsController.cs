using ItemsMicroservice.Data;
using ItemsMicroservice.DTOs;
using ItemsMicroservice.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItemsMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protegido por JWT Token
    public class ItemsController : ControllerBase
    {
        private readonly ItemsDbContext _context;

        public ItemsController(ItemsDbContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
        {
            var items = await _context.Items
                .Select(i => new ItemDto
                {
                    IdItem = i.IdItem,
                    Codigo = i.Codigo,
                    Nombre = i.Nombre,
                    Descripcion = i.Descripcion,
                    Precio = i.Precio,
                    Estado = i.Estado,
                    FechaCreacion = i.FechaCreacion
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
                return NotFound(new { mensaje = "El ítem no existe o fue eliminado." });

            return Ok(new ItemDto
            {
                IdItem = item.IdItem,
                Codigo = item.Codigo,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Estado = item.Estado,
                FechaCreacion = item.FechaCreacion
            });
        }

        // POST: api/items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CrearItem([FromBody] CrearItemDto dto)
        {
            // Validar unicidad de Codigo (incluso entre inactivos usando IgnoreQueryFilters)
            bool codigoExiste = await _context.Items
                .IgnoreQueryFilters()
                .AnyAsync(i => i.Codigo == dto.Codigo);

            if (codigoExiste)
                return BadRequest(new { mensaje = $"El código '{dto.Codigo}' ya está registrado." });

            var item = new Item
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            var result = new ItemDto
            {
                IdItem = item.IdItem,
                Codigo = item.Codigo,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Estado = item.Estado,
                FechaCreacion = item.FechaCreacion
            };

            return CreatedAtAction(nameof(GetItem), new { id = item.IdItem }, result);
        }

        // PUT: api/items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarItem(int id, [FromBody] ActualizarItemDto dto)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
                return NotFound(new { mensaje = "Ítem no encontrado." });

            item.Nombre = dto.Nombre;
            item.Descripcion = dto.Descripcion;
            item.Precio = dto.Precio;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/items/5 (SOFT DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
                return NotFound(new { mensaje = "Ítem no encontrado." });

            // En lugar de Remove, marcamos Estado = false
            item.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Ítem desactivado exitosamente (Soft Delete)." });
        }
    }
}