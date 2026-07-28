// DTOs/ItemDtos.cs
namespace ItemsMicroservice.DTOs
{
    public class ItemDto
    {
        public int IdItem { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearItemDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
    }

    public class ActualizarItemDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
    }
}