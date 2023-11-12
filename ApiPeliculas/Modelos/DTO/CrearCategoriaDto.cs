using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Modelos.DTO
{
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage ="El nombre es obligatorio"),
        MaxLength(100, ErrorMessage ="Nombre obligatorio")]
        public string Nombre { get; set; }
    }
}