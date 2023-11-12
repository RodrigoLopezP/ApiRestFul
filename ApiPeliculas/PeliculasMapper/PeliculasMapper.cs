using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper:Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria,CategoriaDto>().ReverseMap();
            CreateMap<Categoria,CrearCategoriaDto>().ReverseMap();
        }
    }
}