using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsMicroservice.Entities
{
    [Table("Items")]
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdItem { get; set; }

        [Required]
        [StringLength(30)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; } = 0.00m;

        // Soft Delete: 1 (Activo), 0 (Inactivo)
        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}