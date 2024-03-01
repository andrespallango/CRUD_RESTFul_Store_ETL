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

        // GET: api/Store/ventas/{ventaId}
        [HttpGet("ventas/{ventaId}")]
        public IActionResult ObtenerVenta(int ventaId)
        {
            var venta = _context.Ventas
                .Where(v => v.VentaId == ventaId)
                .Join(
                    _context.Clientes,
                    v => v.ClienteId,
                    c => c.ClienteId,
                    (venta, cliente) => new
                    {
                        venta.VentaId,
                        venta.FechaVenta,
                        venta.MontoTotal,
                        ClienteId = cliente.ClienteId,
                        ClienteNombre = cliente.Nombre,
                        ClienteDireccion = cliente.Direccion,
                        ClienteEdad = cliente.Edad
                    }
                )
                .FirstOrDefault();

            if (venta == null)
            {
                return NotFound($"Venta con ID {ventaId} no encontrada.");
            }

            return Ok(venta);
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
