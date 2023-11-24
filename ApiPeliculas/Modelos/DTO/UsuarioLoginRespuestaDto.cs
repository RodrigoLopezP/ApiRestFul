using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Modelos.DTO
{
    public class UsuarioLoginRespuestaDto
    {
        public Usuario Usuario { get; set; }
        public string Token  { get; set; }
    }
}