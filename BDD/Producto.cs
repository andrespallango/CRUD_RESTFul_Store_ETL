using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDD
{
    public class Producto 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}

