using BDD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
            int cantidadProducto,
            DateTime nuevaFechaVenta)
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
                var cliente = BuscarClientePorCedula(cedulaCliente);

                // Buscar el producto por nombre de manera insensible a mayúsculas/minúsculas
                var producto = BuscarProductoPorNombre(nombreProducto);

                // Verificar si cliente y producto fueron encontrados
                if (producto == null || cliente == null)
                {
                    return NotFound("Producto o cliente no encontrado.");
                }

                // Calcular el MontoTotal multiplicando el precio del producto por la cantidad
                double montoTotal = producto.Precio * cantidadProducto;

                // Validar la nueva fecha
                if (nuevaFechaVenta.Year < 2000 || nuevaFechaVenta > DateTime.Now)
                {
                    return BadRequest("La fecha de venta debe estar entre el año 2000 y la fecha actual.");
                }

                // Actualizar los campos de la venta
                venta.FechaVenta = nuevaFechaVenta;
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

        private Cliente BuscarClientePorCedula(string cedula)
        {
            foreach (var cliente in _context.Clientes)
            {
                if (cliente.Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase))
                {
                    return cliente;
                }
            }
            return null;
        }

        private Producto BuscarProductoPorNombre(string nombre)
        {
            foreach (var producto in _context.Productos)
            {
                if (producto.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return producto;
                }
            }
            return null;
        }
    }
}
