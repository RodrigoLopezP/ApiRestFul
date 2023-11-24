using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Modelos.DTO
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage ="El nombre usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage ="La password es obligatoria")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}