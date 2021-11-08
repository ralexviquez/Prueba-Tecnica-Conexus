using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Conexus.API.Data;
using Conexus.Common.Entities;

namespace Conexus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosClientesController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductosClientesController(DataContext context)
        {
            _context = context;
        }


        // GET: api/ProductosClientes/5
        [HttpGet("{ClienteId}")]
        public async Task<ActionResult<IEnumerable<ProductosCliente>>> GetProductosClientes(int ClienteId)
        {
            var productosCliente = await _context.ProductosClientes.Where(pc =>
                                           pc.cliente.Id == ClienteId).ToListAsync();

            if (productosCliente == null)
            {
                return NotFound();
            }

            return productosCliente;
        }

        // GET: api/ProductosClientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductosCliente>> GetProductosCliente(int id)
        {
            var productosCliente = await _context.ProductosClientes.FindAsync(id);

            if (productosCliente == null)
            {
                return NotFound();
            }

            return productosCliente;
        }

        // PUT: api/ProductosClientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductosCliente(int id, ProductosCliente productosCliente)
        {
            if (id != productosCliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(productosCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductosClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductosClientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductosCliente>> PostProductosCliente(ProductosCliente productosCliente)
        {
            _context.ProductosClientes.Add(productosCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductosCliente", new { id = productosCliente.Id }, productosCliente);
        }

        // DELETE: api/ProductosClientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductosCliente(int id)
        {
            var productosCliente = await _context.ProductosClientes.FindAsync(id);
            if (productosCliente == null)
            {
                return NotFound();
            }

            _context.ProductosClientes.Remove(productosCliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductosClienteExists(int id)
        {
            return _context.ProductosClientes.Any(e => e.Id == id);
        }
    }
}
