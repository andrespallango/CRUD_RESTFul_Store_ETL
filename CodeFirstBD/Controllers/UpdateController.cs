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
                var venta = _context.Ventas.Find(id);

                if (venta == null)
                {
                    return NotFound($"Venta con ID {id} no encontrada.");
                }
                if (cantidadProducto <= 0)
                {
                    return BadRequest("La cantidad de producto debe ser un número entero positivo.");
                }

                var cliente = BuscarClientePorCedula(cedulaCliente);
                var producto = BuscarProductoPorNombre(nombreProducto);

                if (producto == null || cliente == null)
                {
                    return NotFound("Producto o cliente no encontrado.");
                }

                int cantidadActual = venta.CantidadProducto;

                if (cantidadProducto > cantidadActual)
                {
                    int cantidadRestar = cantidadProducto - cantidadActual;
                    if (cantidadRestar > producto.Stock)
                    {
                        return BadRequest("La cantidad de producto especificada es mayor que el stock disponible.");
                    }
                    producto.Stock -= cantidadRestar;
                }
                else if (cantidadProducto < cantidadActual)
                {
                    int cantidadAumentar = cantidadActual - cantidadProducto;
                    producto.Stock += cantidadAumentar;
                }
                double montoTotal = producto.Precio * cantidadProducto;
                if (nuevaFechaVenta.Year < 2000 || nuevaFechaVenta > DateTime.Now)
                {
                    return BadRequest("La fecha de venta debe estar entre el año 2000 y la fecha actual.");
                }
                venta.FechaVenta = nuevaFechaVenta;
                venta.MontoTotal = montoTotal;
                venta.CantidadProducto = cantidadProducto;
                venta.Cliente = cliente;
                venta.Producto = producto;
                _context.SaveChanges();
                return Ok($"Venta con ID {id} actualizada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        private bool ValidarCedulaEcuatoriana(string cedula)
        {
            if (cedula.Length == 10)
            {
                var digitoRegion = cedula.Substring(0, 2);

                if (int.TryParse(digitoRegion, out int region) && region >= 1 && region <= 24)
                {
                    var ultimoDigito = int.Parse(cedula.Substring(9, 1));

                    var pares = int.Parse(cedula.Substring(1, 1)) + int.Parse(cedula.Substring(3, 1)) +
                                int.Parse(cedula.Substring(5, 1)) + int.Parse(cedula.Substring(7, 1));

                    var impares = 0;
                    for (var i = 0; i < 9; i += 2)
                    {
                        var numero = int.Parse(cedula.Substring(i, 1)) * 2;
                        impares += (numero > 9) ? (numero - 9) : numero;
                    }

                    var sumaTotal = pares + impares;
                    var primerDigitoSuma = int.Parse(sumaTotal.ToString().Substring(0, 1));
                    var decena = (primerDigitoSuma + 1) * 10;
                    var digitoValidador = decena - sumaTotal;

                    if (digitoValidador == 10)
                        digitoValidador = 0;

                    return digitoValidador == ultimoDigito;
                }
            }

            return false;
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
