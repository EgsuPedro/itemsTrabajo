using ItemsTrabajo.DAL.Entities;

namespace ItemsTrabajo.DAL.Repositories
{
    public interface IWorkItemRepository
    {
        List<WorkItem> ObtenerTodos();
        WorkItem? ObtenerPorId(Guid id);
        void Agregar(WorkItem item);
    }

    public class WorkItemRepository : IWorkItemRepository
    {
        private static readonly List<WorkItem> _items = new();

        public List<WorkItem> ObtenerTodos() => _items;

        public WorkItem? ObtenerPorId(Guid id) => _items.FirstOrDefault(i => i.Id == id);

        public void Agregar(WorkItem item) => _items.Add(item);
    }
}