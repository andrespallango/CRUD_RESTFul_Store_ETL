using BDD;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadProductController : ControllerBase
    {
        private readonly StoreContext _context;

        public ReadProductController(StoreContext context)
        {
            _context = context;
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
    }
}
