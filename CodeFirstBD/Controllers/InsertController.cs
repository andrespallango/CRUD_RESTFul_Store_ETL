using BDD;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertController : ControllerBase
    {
        private readonly StoreContext _context;

        public InsertController(StoreContext context)
        {
            _context = context;
        }

        [HttpPost("ventas")]
        public IActionResult InsertarVenta(string cedulaCliente, string nombreProducto, int cantidadProducto, 
            DateTime fechaVenta)
        {
            try
            {
                if (cantidadProducto <= 0)
                {
                    return BadRequest("La cantidad de producto debe ser un número entero positivo.");
                }

                if (fechaVenta.Year < 2020 || fechaVenta > DateTime.Now)
                {
                    return BadRequest("La fecha de la venta debe ser a partir del año 2020 y no puede ser una fecha futura.");
                }

                if (!ValidarCedulaEcuatoriana(cedulaCliente))
                {
                    return BadRequest("Cédula incorrecta.");
                }

                Cliente cliente = null;
                foreach (var c in _context.Clientes)
                {
                    if (c.Cedula == cedulaCliente)
                    {
                        cliente = c;
                        break;
                    }
                }

                if (cliente == null)
                {
                    return NotFound("Cliente no encontrado.");
                }

                Producto producto = null;
                foreach (var p in _context.Productos)
                {
                    if (string.Equals(p.Nombre, nombreProducto, StringComparison.OrdinalIgnoreCase))
                    {
                        producto = p;
                        break;
                    }
                }

                if (producto == null)
                {
                    return NotFound("Producto no encontrado.");
                }

                if (producto.Stock < cantidadProducto)
                {
                    return BadRequest("Stock insuficiente para completar la venta.");
                }

                double montoTotal = producto.Precio * cantidadProducto;

                var nuevaVenta = new Venta
                {
                    FechaVenta = fechaVenta,
                    MontoTotal = montoTotal,
                    CantidadProducto = cantidadProducto,
                    Cliente = cliente,
                    Producto = producto
                };

                producto.Stock -= cantidadProducto;

                _context.Ventas.Add(nuevaVenta);
                _context.SaveChanges();

                return Ok($"Venta registrada con ID {nuevaVenta.VentaId}. Stock actualizado: {producto.Stock}");
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
                        if (numero > 9) numero -= 9;
                        impares += numero;
                    }

                    var sumaTotal = pares + impares;
                    var decenaSuperior = (int)Math.Ceiling(sumaTotal / 10.0) * 10;
                    var digitoValidador = decenaSuperior - sumaTotal;

                    return digitoValidador == ultimoDigito;
                }
            }
            return false;
        }
    }
}

