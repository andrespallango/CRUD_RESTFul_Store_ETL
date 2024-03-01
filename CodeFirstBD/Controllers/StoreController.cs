using BDD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TuProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly StoreContext _context;

        public StoreController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Store/ventas/cliente/{clienteId}
        [HttpGet("ventas/cliente/{clienteId}")]
        public IActionResult ObtenerVentasCliente(int clienteId)
        {
            var ventas = _context.Ventas
                .Where(v => v.ClienteId == clienteId)
                .Join(
                    _context.Clientes,
                    v => v.ClienteId,
                    c => c.ClienteId,
                    (v, c) => new
                    {
                        v.VentaId,
                        v.FechaVenta,
                        v.MontoTotal,
                        ClienteId = c.ClienteId,
                        ClienteNombre = c.Nombre,
                        ClienteDireccion = c.Direccion,
                        ClienteEdad = c.Edad,
                        ProductoId = v.ProductoId
                    }
                )
                .Join(
                    _context.Productos,
                    vc => vc.ProductoId,
                    p => p.ProductoId,
                    (vc, p) => new
                    {
                        vc.VentaId,
                        vc.FechaVenta,
                        vc.MontoTotal,
                        vc.ClienteId,
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
                    _context.Categorias,
                    pc => pc.CategoriaId,
                    cat => cat.CategoriaId,
                    (pc, cat) => new
                    {
                        pc.VentaId,
                        pc.FechaVenta,
                        pc.MontoTotal,
                        pc.ClienteNombre,
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
                return NotFound($"No se encontraron ventas para el cliente con ID {clienteId}.");
            }

            return Ok(ventas);
        }

        // DELETE: api/Store/ventas/{ventaId}
        [HttpDelete("ventas/{ventaId}")]
        public IActionResult EliminarVenta(int ventaId)
        {
            var venta = _context.Ventas.Find(ventaId);
            if (venta == null)
            {
                return NotFound($"Venta con ID {ventaId} no encontrada.");
            }

            _context.Ventas.Remove(venta);
            _context.SaveChanges();

            return NoContent();
        }
    }
}