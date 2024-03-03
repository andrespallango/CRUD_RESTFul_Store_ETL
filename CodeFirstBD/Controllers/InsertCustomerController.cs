using BDD;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CodeFirstBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertCustomerController : ControllerBase
    {
        private readonly StoreContext _context;
        public InsertCustomerController(StoreContext context)
        {
            _context = context;
        }
        [HttpPost("clientes")]
        public IActionResult InsertarCliente(string cedula, string nombre, string direccion, int edad)
        {
            try
            {
                if (!ValidarCedulaEcuatoriana(cedula))
                {
                    return BadRequest("Cédula incorrecta.");
                }
                var clientesConMismaCedula = _context.Clientes
                    .AsEnumerable()
                    .Where(c => string.Equals(c.Cedula, cedula, StringComparison.OrdinalIgnoreCase));

                if (clientesConMismaCedula.Any())
                {
                    return BadRequest("Ya existe un cliente con la misma cédula.");
                }
                if (edad <= 18 || edad >= 114)
                {
                    return BadRequest("La edad debe ser mayor a 18 y menor a 114 años.");
                }
                var nuevoCliente = new Cliente
                {
                    Cedula = cedula,
                    Nombre = nombre,
                    Direccion = direccion,
                    Edad = edad
                };

                _context.Clientes.Add(nuevoCliente);
                _context.SaveChanges();

                return Ok($"Cliente registrado con ID {nuevoCliente.ClienteId}.");
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
                        if (numero > 9) numero -= 9;
                        impares += numero;
                    }

                    var sumaTotal = pares + impares;
                    var decenaSuperior = (int)Math.Ceiling(sumaTotal / 10.0) * 10;
                    var digitoValidador = decenaSuperior - sumaTotal;

                    return digitoValidador == ultimoDigito;
                }
            }

            return false;
        }
    }
}
