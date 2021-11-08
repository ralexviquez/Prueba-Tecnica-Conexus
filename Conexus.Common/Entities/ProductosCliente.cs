using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.Common.Entities
{
    public class ProductosCliente
    {
        public int Id { get; set; }
        public Cliente cliente { get; set; }
        public Producto producto { get; set; }
    }
}
