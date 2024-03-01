using BDD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFirst.Controllers
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

        [HttpGet("ventas/{clienteId}")]
        public async Task<IActionResult> ObtenerVentasCliente(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);

            if (cliente == null)
            {
                return NotFound($"Cliente con ID {clienteId} no encontrado.");
            }

            var ventas = await _context.Ventas
                .Include(v => v.Productos)
                .Where(v => v.ClienteId == clienteId)
                .ToListAsync();

            return Ok(ventas);
        }
    }
}
