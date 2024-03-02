using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BDD
{
    public abstract class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoriaId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }       
    }
    public class Celular : Categoria
    {
        public string? Marca { get; set; }
    }
    public class Computadora : Categoria
    {
        public string? Procesador { get; set; }
    }
    public class LineaBlanca : Categoria
    {
        public string? TipoElectrodomestico { get; set; }
    }
}

