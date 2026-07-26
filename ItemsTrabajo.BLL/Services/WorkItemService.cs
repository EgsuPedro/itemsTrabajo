using ItemsTrabajo.BLL.DTOs;
using ItemsTrabajo.DAL.Entities;
using ItemsTrabajo.DAL.Repositories;

namespace ItemsTrabajo.BLL.Services
{
    public interface IWorkItemService
    {
        Task<WorkItem?> CrearYDistribuirItemAsync(CrearWorkItemDto dto);
        List<WorkItem> ObtenerTodos();
    }

    public class WorkItemService : IWorkItemService
    {
        private readonly IWorkItemRepository _repository;
        private readonly IGestionUsuariosClient _usuariosClient;

        public WorkItemService(IWorkItemRepository repository, IGestionUsuariosClient usuariosClient)
        {
            _repository = repository;
            _usuariosClient = usuariosClient;
        }

        public async Task<WorkItem?> CrearYDistribuirItemAsync(CrearWorkItemDto dto)
        {
            var nuevoItem = new WorkItem
            {
                Id = Guid.NewGuid(),
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaEntrega = dto.FechaEntrega,
                Relevancia = dto.Relevancia,
                Completado = false
            };

            // 1. Consultar usuarios candidatos no saturados desde el microservicio GestionUsuarios
            var candidatos = await _usuariosClient.ObtenerUsuariosCandidatosAsync();

            // Filtrar nuevamente en memoria por seguridad (Regla de saturación)
            var usuariosDisponibles = candidatos.Where(u => !u.EstaSaturado).ToList();

            if (!usuariosDisponibles.Any())
            {
                // No hay usuarios disponibles
                return null;
            }

            UsuarioCandidatoDto? usuarioSeleccionado = null;
            DateTime fechaActual = DateTime.Now;

            // 2. APLICACIÓN DE LAS REGLAS DE DISTRIBUCIÓN

            // Regla A: Si la fecha está próxima a vencer (< 3 días), se asigna al de menos pendientes independientemente de relevancia
            if (nuevoItem.EsProximoAVencer(fechaActual))
            {
                usuarioSeleccionado = usuariosDisponibles
                    .OrderBy(u => u.TotalPendientes)
                    .ThenBy(u => u.CantidadAltaRelevancia)
                    .First();
            }
            // Regla B: Los ítems relevantes se asignan primero a quienes tienen menor lista de pendientes
            else
            {
                usuarioSeleccionado = usuariosDisponibles
                    .OrderBy(u => u.TotalPendientes)
                    .ThenBy(u => u.CantidadAltaRelevancia)
                    .First();
            }

            // 3. Registrar la asignación
            nuevoItem.UsuarioAsignado = usuarioSeleccionado.Username;
            _repository.Agregar(nuevoItem);

            // 4. Notificar a GestionUsuarios para actualizar su lista y reordenar sus pendientes
            await _usuariosClient.NotificarAsignacionAsync(
                usuarioSeleccionado.Username,
                nuevoItem.Id,
                nuevoItem.Titulo,
                nuevoItem.FechaEntrega,
                nuevoItem.Relevancia == Relevancia.Alta
            );

            return nuevoItem;
        }

        public List<WorkItem> ObtenerTodos() => _repository.ObtenerTodos();
    }
}