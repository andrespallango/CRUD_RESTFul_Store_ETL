using BDD;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProductController : ControllerBase
    {
        private readonly StoreContext _context;

        public UpdateProductController(StoreContext context)
        {
            _context = context;
        }

        [HttpPut("actualizarStock")]
        public IActionResult ActualizarStockProducto(string nombreProducto, int nuevoStock)
        {
            try
            {
                if (nuevoStock < 0)
                {
                    return BadRequest("El stock no puede ser un número negativo.");
                }

                Producto productoEncontrado = null;
                foreach (var producto in _context.Productos)
                {
                    if (string.Equals(producto.Nombre, nombreProducto, StringComparison.OrdinalIgnoreCase))
                    {
                        productoEncontrado = producto;
                        break;
                    }
                }

                if (productoEncontrado == null)
                {
                    return NotFound("Producto no encontrado.");
                }

                productoEncontrado.Stock = nuevoStock;
                _context.SaveChanges();

                return Ok($"Stock del producto '{nombreProducto}' actualizado a {nuevoStock}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
