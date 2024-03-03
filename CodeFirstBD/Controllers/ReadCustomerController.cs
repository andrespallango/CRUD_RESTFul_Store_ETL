using BDD;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadCustomerController : ControllerBase
    {
        private readonly StoreContext _context;

        public ReadCustomerController(StoreContext context)
        {
            _context = context;
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
    }
}
