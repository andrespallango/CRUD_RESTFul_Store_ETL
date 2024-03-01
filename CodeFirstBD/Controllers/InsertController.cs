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

        // POST: api/Insert/ventas
        [HttpPost("ventas")]
        public IActionResult InsertarVenta(string cedulaCliente, string nombreProducto, int cantidadProducto, DateTime fechaVenta)
        {
            try
            {
                // Validar cédula
                if (!ValidarCedulaEcuatoriana(cedulaCliente))
                {
                    return BadRequest("Cédula incorrecta.");
                }

                // Obtener el cliente y producto de la base de datos por cédula y nombre
                var cliente = _context.Clientes.FirstOrDefault(c => c.Cedula == cedulaCliente);
                var producto = _context.Productos.FirstOrDefault(p => p.Nombre == nombreProducto);

                if (producto == null || cliente == null)
                {
                    return NotFound("Producto o cliente no encontrado.");
                }

                // Verificar si hay suficiente stock
                if (producto.Stock < cantidadProducto)
                {
                    return BadRequest("Stock insuficiente para completar la venta.");
                }

                // Calcular el MontoTotal multiplicando el precio del producto por la cantidad
                double montoTotal = producto.Precio * cantidadProducto;

                // Crear una nueva instancia de Venta con los datos obtenidos
                var nuevaVenta = new Venta
                {
                    FechaVenta = fechaVenta,
                    MontoTotal = montoTotal,
                    CantidadProducto = cantidadProducto,
                    Cliente = cliente,
                    Producto = producto
                };

                // Actualizar el stock del producto
                producto.Stock -= cantidadProducto;

                // Agregar la nueva venta al contexto y guardar cambios
                _context.Ventas.Add(nuevaVenta);
                _context.SaveChanges();

                return Ok($"Venta registrada con ID {nuevaVenta.VentaId}. Stock actualizado: {producto.Stock}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Función para validar cédula ecuatoriana
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
    }
}
