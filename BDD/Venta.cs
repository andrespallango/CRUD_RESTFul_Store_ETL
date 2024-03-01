using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDD
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VentaId { get; set; }

        // Llave foránea al cliente
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        // Relación muchos a muchos con Producto (manejada a través de la tabla intermedia VentaProducto)
        public List<Producto> Productos { get; set; } = new List<Producto>();
        // Otras propiedades de la venta si es necesario
    }
}
