using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD
{
    public interface IProducto
    {
        int ProductoId { get; set; }
        string Nombre { get; set; }
        decimal Precio { get; set; }
        int Stock { get; set; }
    }

    public class Producto : IProducto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        // Otras propiedades relacionadas con el producto según sea necesario
    }


}
