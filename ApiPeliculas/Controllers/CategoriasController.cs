using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.DTO;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApiPeliculas.Controllers
{
    //QUE SIGNIFICA [controller]?
    /*Con [controller] se refiere al nome de la clase menos el sufijo Controller
    Aqui debajo por ejemplo CategoriasController, el url del api resulta 
    api/Categorias
    */
    [ApiController]
    // [Route("api/[controller]")] Opcional
    [Route("[controller]")] 
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _ctRepo=categoriaRepositorio;
            _mapper=mapper;
        }

        [HttpGet("getcategorias")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias(){
            var listCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDTO= new List<CategoriaDto>();
            foreach (var lista in listCategorias)
            {
                listaCategoriasDTO.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDTO);
        }

        [HttpGet("{cateId:int}" ,Name ="GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int cateId)
        {
            var itemCategoria= _ctRepo.GetCategoria(cateId);
            if(itemCategoria ==null)
                return NotFound();
            var itemCategoriaDto= _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            //ModelState: se i DATA ANNOTATION en el DTO no son respetados, esto saldra FALSE
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new{cateId=categoria.Id}, categoria);
        }

        [HttpPatch("{categoriaId:int}",Name ="ActualizarPatchCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
             //ModelState: se i DATA ANNOTATION en el DTO no son respetados, esto saldra FALSE
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo saliò mal haciendo UPDATE de Categoria{categoria.Nombre}");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }
       
       
        [HttpDelete("{categoriaId:int}",Name ="BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if(!_ctRepo.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }
            Categoria categoria=_ctRepo.GetCategoria(categoriaId);
            // var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo saliò mal haciendo DELETE de Categoria{categoria.Nombre}");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }

    }
}