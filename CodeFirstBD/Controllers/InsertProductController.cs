using BDD;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertProductController : ControllerBase
    {
        private readonly StoreContext _context;

        public InsertProductController(StoreContext context)
        {
            _context = context;
        }

        [HttpPost("productos")]
        public IActionResult InsertarProducto(string nombre, double precio, int stock, int categoriaId)
        {
            try
            {
                var productosConMismoNombre = _context.Productos.AsEnumerable()
                    .Where(p => string.Equals(p.Nombre, nombre, StringComparison.OrdinalIgnoreCase));

                if (productosConMismoNombre.Any())
                {
                    return BadRequest("Ya existe un producto con el mismo nombre.");
                }

                var categoria = BuscarCategoriaPorId(categoriaId);

                if (categoria == null)
                {
                    return NotFound("Categoría no encontrada.");
                }
                if (precio < 0 || stock < 0)
                {
                    return BadRequest("El precio y el stock no pueden ser números negativos.");
                }

                var nuevoProducto = new Producto
                {
                    Nombre = nombre,
                    Precio = precio,
                    Stock = stock,
                    CategoriaId = categoriaId,
                    Categoria = categoria
                };

                _context.Productos.Add(nuevoProducto);
                _context.SaveChanges();

                return Ok($"Producto registrado con ID {nuevoProducto.ProductoId}.");
            }
            catch (Exception ex){
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        private Categoria BuscarCategoriaPorId(int categoriaId)
        {
            foreach (var categoria in _context.Categorias.AsEnumerable())
            {
                if (categoria.CategoriaId == categoriaId)
                {
                    return categoria;
                }
            }
            return null;
        }
    }
}
