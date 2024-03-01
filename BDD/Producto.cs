using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDD
{
    public interface IProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int ProductoId { get; set; }

        string Nombre { get; set; }
        double Precio { get; set; }
        int Stock { get; set; }
    }

    public class Producto : IProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductoId { get; set; }

        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }

        // Llave foránea a la categoría
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
