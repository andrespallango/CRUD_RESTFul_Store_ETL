using BDD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadController : ControllerBase
    {
        private readonly StoreContext _context;

        public ReadController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("ventas/cliente/{cedula}")]
        public IActionResult ObtenerVentasCliente(string cedula)
        {
            if (!ValidarCedulaEcuatoriana(cedula))
            {
                return BadRequest("Cédula incorrecta.");
            }

            var ventas = _context.Ventas
                .Where(v => v.Cliente.Cedula == cedula)
                .Join(
                    _context.Clientes, v => v.ClienteId, c => c.ClienteId, (v, c) => new
                    {
                        v.VentaId,
                        v.FechaVenta,
                        v.CantidadProducto,
                        v.MontoTotal,
                        ClienteId = c.ClienteId,
                        ClienteCedula = c.Cedula,
                        ClienteNombre = c.Nombre,
                        ClienteDireccion = c.Direccion,
                        ClienteEdad = c.Edad,
                        ProductoId = v.ProductoId
                    }
                )
                .Join(
                    _context.Productos,
                    vc => vc.ProductoId, p => p.ProductoId, (vc, p) => new
                    {
                        vc.VentaId,
                        vc.FechaVenta,
                        vc.CantidadProducto,
                        vc.MontoTotal,
                        vc.ClienteId,
                        vc.ClienteCedula,
                        vc.ClienteNombre,
                        vc.ClienteDireccion,
                        vc.ClienteEdad,
                        ProductoId = p.ProductoId,
                        ProductoNombre = p.Nombre,
                        ProductoPrecio = p.Precio,
                        ProductoStock = p.Stock,
                        CategoriaId = p.CategoriaId
                    }
                )
                .Join(
                    _context.Categorias, pc => pc.CategoriaId, cat => cat.CategoriaId, (pc, cat) => new
                    {
                        pc.VentaId,
                        pc.FechaVenta,
                        pc.CantidadProducto,
                        pc.MontoTotal,
                        pc.ClienteNombre,
                        pc.ClienteCedula,
                        pc.ClienteDireccion,
                        pc.ClienteEdad,
                        pc.ProductoNombre,
                        pc.ProductoPrecio,
                        CategoriaNombre = cat.Nombre
                    }
                )
                .ToList();

            if (ventas.Count == 0)
            {
                return NotFound($"No se encontraron ventas para el cliente con cédula {cedula}.");
            }

            return Ok(ventas);
        }

        [HttpGet("cliente")]
        public IActionResult ObtenerClientePorCedula([FromQuery] string cedula)
        {
            try
            {
                Cliente clienteEncontrado = null;

                foreach (var cliente in _context.Clientes)
                {
                    if (string.Equals(cliente.Cedula, cedula, StringComparison.OrdinalIgnoreCase))
                    {
                        clienteEncontrado = new Cliente
                        {
                            ClienteId = cliente.ClienteId,
                            Cedula = cliente.Cedula,
                            Nombre = cliente.Nombre,
                            Direccion = cliente.Direccion,
                            Edad = cliente.Edad
                        };
                        break;
                    }
                }
                if (clienteEncontrado == null)
                {
                    return NotFound("Cliente no encontrado.");
                }

                return Ok(clienteEncontrado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("producto")]
        public IActionResult ObtenerProductoPorNombre([FromQuery] string nombre)
        {
            try
            {
                Producto producto = null;

                foreach (var p in _context.Productos)
                {
                    if (string.Equals(p.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                    {
                        var categoria = _context.Categorias.Find(p.CategoriaId);

                        producto = new Producto
                        {
                            ProductoId = p.ProductoId,
                            Nombre = p.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Categoria = categoria
                        };

                        break;
                    }
                }
                if (producto == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                return Ok(producto);
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
    }
}
