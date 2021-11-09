﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Conexus.Common.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Conexus.FrontEnd.Models
{
    public class ProductosClientesViewModel : ProductosCliente
    {

        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProductoId { get; set; }
        public IEnumerable<SelectListItem> Productos { get; set; }
    }
}