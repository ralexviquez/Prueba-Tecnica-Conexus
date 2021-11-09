using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Conexus.API.Data;
using Conexus.Common.Entities;
using Microsoft.Data.SqlClient;

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
        [HttpGet("[action]/{ClienteId}")]
        public async Task<ActionResult<IEnumerable<ProductosCliente>>> GetProductosClientes(int ClienteId)
        {
            var productosCliente = await _context.ProductosClientes.Where(pc =>
                                           pc.cliente.Id == ClienteId).Include(pc => pc.cliente).Include(pc => pc.producto).ToListAsync();

            if (productosCliente == null)
            {
                return NotFound();
            }

            return productosCliente;
        }

        // GET: api/ProductosClientes/5
        [HttpGet("[action]/{ClienteId}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosNoRegistrados(int ClienteId)
        {
            var productosCliente = await (from p in _context.Productos
                                          join c in _context.ProductosClientes on p.Id equals c.producto.Id into productos
                                          from pc in productos.DefaultIfEmpty()
                                          where pc.cliente.Id != ClienteId
                                          select p ).ToListAsync();

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
            var productosCliente = await _context.ProductosClientes.Where(pc =>
                                           pc.Id == id).Include(pc => pc.cliente).Include(pc => pc.producto).FirstOrDefaultAsync();

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

            _context.ProductosClientes.Update(productosCliente).State = EntityState.Modified;

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
            try
            {
                SqlParameter pam1 = new SqlParameter("@ClienteId", productosCliente.cliente.Id);
                SqlParameter pam2 = new SqlParameter("@ProductoId", productosCliente.producto.Id);

                int retorno = await _context.Database.ExecuteSqlRawAsync("Ps_AddProductosClientes @ClienteId, @ProductoId", pam1, pam2);
                productosCliente.Id = retorno;


                return CreatedAtAction("GetProductosCliente", new { id = productosCliente.Id }, productosCliente);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return NotFound("Ya existe este procedimiento.");
                }
                else
                {
                    return NotFound(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
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
