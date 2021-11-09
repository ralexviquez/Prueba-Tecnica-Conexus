using Conexus.Common.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.FrontEnd.Models
{
    public class ProductoViewModel: Producto
    {


        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CategoriaId { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
    }
}
