using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.Common.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellidos { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FechaNacimiento { get; set; }
        [Display(Name = "Correo Electrónico")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El valor de {0} no es válido.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Correo { get; set; }
        [Display(Name = "Teléfono")]
        [MaxLength(15, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Telefono { get; set; }
    }
}
