using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

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
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = peliculaRepositorio.GetPelicula(peliculaId);
            if (itemPelicula == null)
                return NotFound();
            var itemPeliculaDto = mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreaPelicula([FromBody] PeliculaDto peliculaDto)
        {
            //ModelState: se i DATA ANNOTATION en el DTO no son respetados, esto saldra FALSE
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (peliculaRepositorio.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = mapper.Map<Pelicula>(peliculaDto);
            if (!peliculaRepositorio.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal durante la creacion de la pelicula {pelicula.Nombre}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetPelicula", new{peliculaId=pelicula.Id}, pelicula);
        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliDto)
        {
        //ModelState: se i DATA ANNOTATION en el DTO no son respetados, esto saldra FALSE
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (peliDto == null || peliculaId != peliDto.Id)
            {
                return BadRequest(ModelState);
            }

            Pelicula? pelicula = mapper.Map<Pelicula>(peliDto);
            if (!peliculaRepositorio.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo saliò mal haciendo UPDATE de Pelicula {pelicula.Nombre}");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }
     
        [HttpDelete("{peliculaId:int}",Name ="BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if(!peliculaRepositorio.ExistePelicula(peliculaId))
            {
                return NotFound();
            }
            Pelicula pelicula=peliculaRepositorio.GetPelicula(peliculaId);
            if (!peliculaRepositorio.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo saliò mal haciendo DELETE de Pelicula {pelicula.Nombre}");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }

        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculasEnCategoria(int categoriaId){
            ICollection<Pelicula>? listPeliculas = peliculaRepositorio.GetPeliculasEnCategoria(categoriaId);
            if(listPeliculas == null){
                return NotFound();
            }
            var itemPelicula= new List<PeliculaDto>();
            foreach (var lista in listPeliculas)
            {
                PeliculaDto newItem=mapper.Map<PeliculaDto>(lista);
                itemPelicula.Add(newItem);
            }
            return Ok(itemPelicula);
        }
        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Buscar(string peliculaNombre)
        {
            try
            {
                ICollection<Pelicula>? listPeliculas = peliculaRepositorio.BuscarPelicula(peliculaNombre.Trim());
                if (listPeliculas.Any())
                {
                    return Ok(listPeliculas);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Algo saliò mal durante Buscar pelicula por nombre");
            }
        }
    }
}