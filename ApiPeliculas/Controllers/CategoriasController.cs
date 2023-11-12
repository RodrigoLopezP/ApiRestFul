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
    //QUE SIGNIFICA [controller]?
    /*Con [controller] se refiere al nome de la clase menos el sufijo Controller
    Aqui debajo por ejemplo CategoriasController, el url del api resulta 
    api/Categorias
    */
    [ApiController]
    // [Route("api/[controller]")] Opcional
    [Route("api/categorias")] 
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _ctRepo=categoriaRepositorio;
            _mapper=mapper;
        }

        [HttpGet]
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
    }
}