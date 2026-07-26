using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsTrabajo.DAL.Entities
{
    public enum Relevancia
    {
        Baja = 1,
        Alta = 2
    }

    public class WorkItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public Relevancia Relevancia { get; set; }
        public bool Completado { get; set; }
        public string? UsuarioAsignado { get; set; }

        /// <summary>
        /// Determina si la fecha de entrega está próxima a vencer (menos de 3 días)
        /// </summary>
        public bool EsProximoAVencer(DateTime fechaReferencia)
        {
            var diasRestantes = (FechaEntrega.Date - fechaReferencia.Date).TotalDays;
            return diasRestantes < 3 && diasRestantes >= 0;
        }
    }
}
