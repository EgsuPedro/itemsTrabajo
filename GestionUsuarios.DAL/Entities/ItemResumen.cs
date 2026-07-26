namespace GestionUsuarios.DAL.Entities
{
    public class ItemResumen
    {
        public Guid ItemId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public bool EsAltaRelevancia { get; set; }
    }
}
