using ItemsTrabajo.DAL.Entities;

namespace ItemsTrabajo.BLL.DTOs
{
    public class CrearWorkItemDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public Relevancia Relevancia { get; set; }
    }

}