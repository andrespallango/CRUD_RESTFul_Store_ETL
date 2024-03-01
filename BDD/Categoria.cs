using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDD
{

    public abstract class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoriaId { get; set; }

        public string Nombre { get; set; }
        
    }


    public class CelularCategoria : Categoria
    {
        // Propiedad específica de CelularCategoria
        public string Marca { get; set; }
    }


    public class ComputadoraCategoria : Categoria
    {

        public string Procesador { get; set; }
    }


    public class LineaBlancaCategoria : Categoria
    {

        public string TipoElectrodomestico { get; set; }
    }
}