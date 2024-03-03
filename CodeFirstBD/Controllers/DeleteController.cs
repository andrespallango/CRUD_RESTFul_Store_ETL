using BDD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TuProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly StoreContext _context;
        public DeleteController(StoreContext context)
        {
            _context = context;
        }
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
