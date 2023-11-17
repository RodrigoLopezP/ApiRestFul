using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos.DTO;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio peliculaRepositorio;
        private readonly IMapper mapper;

        public PeliculasController(IPeliculaRepositorio peliculaRepositorio, IMapper mapper)
        {
            this.peliculaRepositorio = peliculaRepositorio;
            this.mapper = mapper;
        }
        [HttpGet("getpeliculas")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas(){
            var listPeliculas = peliculaRepositorio.GetPeliculas();
            var listaPeliculasDTO= new List<PeliculaDto>();
            foreach (var lista in listPeliculas)
            {
                listaPeliculasDTO.Add(mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDTO);
        }
       
    }
}