using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD
{
    public class Venta
    {
        public int VentaId { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public List<Producto> Productos { get; set; } = new List<Producto>();
        // Otras propiedades de la venta si es necesario
    }

}
