using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthUsuariosMicroservice.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARBINARY(64)")]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [Required]
        [Column(TypeName = "VARBINARY(128)")]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required]
        [StringLength(20)]
        public string Rol { get; set; } = "Usuario";

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}