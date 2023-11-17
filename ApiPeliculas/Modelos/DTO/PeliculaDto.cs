using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Modelos.DTO
{
    public class PeliculaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre obligatorio")]
        public string Nombre { get; set; }

        public string RutaImagen { get; set; }

        [Required(ErrorMessage ="Descripcion obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage ="Duración obligatoria")]
        public int Duracion { get; set; }

        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }

        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }
    }
}