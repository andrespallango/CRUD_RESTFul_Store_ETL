using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDD
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }
        [Required]
        public string? Cedula { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Direccion { get; set; }
        public int Edad { get; set; }
    }
}

