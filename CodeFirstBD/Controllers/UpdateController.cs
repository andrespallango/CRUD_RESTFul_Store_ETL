using BDD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly StoreContext _context;

        public UpdateController(StoreContext context)
        {
            _context = context;
        }

        [HttpPut("ventas/{id}")]
        public IActionResult ActualizarVenta(
            int id,
            string cedulaCliente,
            string nombreProducto,
            int cantidadProducto)
        {
            try
            {
                // Obtener la venta por ID
                var venta = _context.Ventas.Find(id);

                if (venta == null)
                {
                    return NotFound($"Venta con ID {id} no encontrada.");
                }

                // Buscar el cliente por cédula
                Cliente cliente = null;
                foreach (var c in _context.Clientes)
                {
                    if (c.Cedula == cedulaCliente)
                    {
                        cliente = c;
                        break;
                    }
                }

                // Buscar el producto por nombre
                Producto producto = null;
                foreach (var p in _context.Productos)
                {
                    if (p.Nombre == nombreProducto)
                    {
                        producto = p;
                        break;
                    }
                }

                // Verificar si cliente y producto fueron encontrados
                if (producto == null || cliente == null)
                {
                    return NotFound("Producto o cliente no encontrado.");
                }

                // Calcular el MontoTotal multiplicando el precio del producto por la cantidad
                double montoTotal = producto.Precio * cantidadProducto;

                // Actualizar los campos de la venta
                venta.FechaVenta = DateTime.Now;
                venta.MontoTotal = montoTotal;
                venta.CantidadProducto = cantidadProducto;
                venta.Cliente = cliente;
                venta.Producto = producto;

                // Guardar cambios
                _context.SaveChanges();

                return Ok($"Venta con ID {id} actualizada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
