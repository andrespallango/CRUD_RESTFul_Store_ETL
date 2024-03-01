using BDD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ObtenerVenta(int ventaId)
        {
            var venta = await _context.Ventas
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
                        // Agrega más propiedades del cliente según sea necesario
                    }
                )
                .FirstOrDefaultAsync();

            if (venta == null)
            {
                return NotFound($"Venta con ID {ventaId} no encontrada.");
            }

            return Ok(venta);
        }

        // DELETE: api/Store/ventas/{ventaId}
        [HttpDelete("ventas/{ventaId}")]
        public async Task<IActionResult> EliminarVenta(int ventaId)
        {
            var venta = await _context.Ventas.FindAsync(ventaId);
            if (venta == null)
            {
                return NotFound($"Venta con ID {ventaId} no encontrada.");
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
