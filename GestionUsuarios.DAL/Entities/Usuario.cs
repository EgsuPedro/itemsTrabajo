using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUsuarios.DAL.Entities
{
    public class Usuario
    {
        public string Username { get; set; } = string.Empty;
        public int CantidadCompletados { get; set; }
        public List<ItemResumen> ItemsPendientes { get; set; } = new();

        // Total de ítems pendientes actuales
        public int TotalPendientes => ItemsPendientes.Count;

        // Cantidad de ítems altamente relevantes pendientes
        public int CantidadAltaRelevancia => ItemsPendientes.Count(i => i.EsAltaRelevancia);

        // Regla de Negocio: Si cuenta con más de 3 ítems altamente relevantes, está saturado
        public bool EstaSaturado => CantidadAltaRelevancia > 3;

        /// <summary>
        /// Ordena la lista de pendientes por fecha de entrega y relevancia
        /// </summary>
        public void ReordenarPendientes()
        {
            ItemsPendientes = ItemsPendientes
                .OrderBy(i => i.FechaEntrega)
                .ThenByDescending(i => i.EsAltaRelevancia)
                .ToList();
        }
    }
}
