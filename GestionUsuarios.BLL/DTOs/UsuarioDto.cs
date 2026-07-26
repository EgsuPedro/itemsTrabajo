using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUsuarios.BLL.DTOs
{
    public class UsuarioDto
    {
        public string Username { get; set; } = string.Empty;
        public int TotalPendientes { get; set; }
        public int CantidadAltaRelevancia { get; set; }
        public int CantidadCompletados { get; set; }
        public bool EstaSaturado { get; set; }
        public List<ItemResumenDto> ItemsPendientes { get; set; } = new();
    }

    public class ItemResumenDto
    {
        public Guid ItemId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public bool EsAltaRelevancia { get; set; }
    }
}
