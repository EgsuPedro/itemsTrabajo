using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsTrabajo.DAL.Entities;

namespace ItemsTrabajo.BLL.DTOs
{
    public class UsuarioCandidatoDto
    {
        public string Username { get; set; } = string.Empty;
        public int TotalPendientes { get; set; }
        public int CantidadAltaRelevancia { get; set; }
        public bool EstaSaturado { get; set; }
    }
}
