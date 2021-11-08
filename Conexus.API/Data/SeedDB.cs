using Conexus.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriasAsync();
        }

        private async Task CheckCategoriasAsync()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.Add(new Categoria { Descripcion = "Cloud" });
                _context.Categorias.Add(new Categoria { Descripcion = "Virtualización" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
